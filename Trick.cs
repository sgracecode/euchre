using System;
using System.Collections;

namespace Euchre
{
    class Trick
    {
        /*This class is currently outfitted to work with the 4player table. Better handling of generic lengths later.
         * Seats and Moves are parallel, so player in seat 2 made move 2. Null moves haven't been made yet.
         * These also MUST be parallel to the table's indexes.
         * Tricks are created by Hands, and there are 5 tricks in a hand. A trick determines the winner for the hand to score.
         */
        Player[] seats;
        Card[] moves;
        public string trumpSuit;
        public string leadSuit;

        public int playCount;   //how many players have gone so far
        public int currentWinner;  //corresponds to index in moves

        public Trick(Player[] players, string trump)
        {
            seats = players;
            moves = new Card[players.Length];
            playCount = 0;
            trumpSuit = trump;
        }

        public void leadPlay(Card move, int i)
        {
            //Have to check if left bower lead - this will catch either bower
            if(trumpCheck(move) > 1)
            {
                leadSuit = trumpSuit;
            }
            else
            {
                leadSuit = move.suit;
            }
            moves[i] = move;
            currentWinner = i;
        }

        //Cards that are played are already checked for legality
        public void addPlay(Card move, int i)
        {
            moves[i] = move;
            playCount++;
            if(isBetterCard(i))
            {
                currentWinner = i;
            }
        }

        public Boolean isBetterCard(int newMove)
        {
            int trumpCurrentCheck = trumpCheck(moves[currentWinner]);
            int trumpNewCheck = trumpCheck(moves[newMove]);

            //If the old one outtrumps the new one with any level of trump outcome
            //This step will take care of all jack infighting because jacks will never tie
            if(trumpNewCheck > trumpCurrentCheck)
            {
                return true;
            }
            else if(trumpCurrentCheck > trumpNewCheck)
            {
                return false;
            }
            //Equal level trump
            else if (trumpNewCheck == 1 && trumpCurrentCheck == 1)
            {
                if (moves[newMove].value > moves[currentWinner].value)
                    return true;
                else
                    return false;
            }
            //Check for onsuit plays, and all trump should've been covered before this
            //Note that the currentWinner will always be trump or onsuit, if not both
            else if(moves[newMove].suit == leadSuit)
            {
                if (moves[newMove].value > moves[currentWinner].value)
                    return true;
                else
                    return false;
            }
            //Okay so the new play must be offsuit at this point
            return false;
        }

        //This method is meant to handle the precedence of jacks in trump checking
        //Return of 0 means no, 1 means yes, 2 means left bower, 3 means right bower
        public int trumpCheck(Card check)
        {
            //Is it trump?
            if (check.suit == trumpSuit)
            {
                //Check for right bower
                if(check.value == 11)
                    return 3;
                else
                    return 1;
            }
            //Must check if card is left bower
            else if (check.value == 11)
            {
                //This is only acceptable because the right is already eliminated in the first if
                if (trumpSuit == "Clubs" || trumpSuit == "Spades")
                {
                    if (check.suit == "Clubs" || check.suit == "Spades")
                        return 2;
                }
                else if (trumpSuit == "Hearts" || trumpSuit == "Diamonds")
                {
                    if (check.suit == "Hearts" || check.suit == "Diamonds")
                        return 2;
                }
            }
            //Not trump or bower
            return 0;
        }
        //This is used primarily by the playCard's isLegalMove method checking if it is indeed a legal play
        public Boolean onSuitCheck(Card check)
        {
            //3 cases: definitely onsuit, left bower only matching trump suit, or offsuit for sure
            Boolean isLegal = false;
            //So, if it matches suit and it's not the left bower
            if(check.suit == leadSuit && trumpCheck(check) != 2)
            {
                isLegal = true;
            }
            //Now we have to include the left bower as legal
            else if(leadSuit == trumpSuit && trumpCheck(check) == 2)
            {
                isLegal = true;
            }
            return isLegal;
        }

        public int winner()
        {
            Console.WriteLine("");
            ToString();
            Console.WriteLine(seats[currentWinner].name + " wins this trick with a " + moves[currentWinner].ToString());
            return currentWinner;
        }


        public override string ToString()
        {
            Console.WriteLine("");
            string returnString = "\tPlaying field: \n";
            for(int i=0; i<seats.Length; i++)
            {
                if(moves[i] != null)
                {
                    returnString += ("\t\tPlayer " + seats[i] + " played " + moves[i] + "\n");
                }
                else
                {
                    returnString += ("\t\tPlayer " + seats[i] + " has not played yet." + "\n");
                }
            }
            return returnString;
        }

        public Card[] getMoves()
        {
            return moves;
        }
    }
}
