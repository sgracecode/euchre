using System;
using System.Collections;
using System.Linq;

namespace Euchre
{
    abstract class Player
    {
        /*This base player should be applicable to both computer and human players.
         * No player is aware of their position on the field, but teammate and dealer are kept track of.
         * 
         */
        public String name { get; set; }

        public Player teammate { get; set; }
        public bool isDealer { get; set; }

        CardCounter counter = new CardCounter();

        protected ArrayList hand = new ArrayList();

        abstract public Card leadCard(Hand currentHand);    //Player is the first to play a card for the trick
        abstract public Card playCard(Hand tableHand);    //Player must follow rules of play with this method
        //abstract public int analyzeHand();             //Player able to look at all cards; computer ranks itself
        abstract public Boolean orderUp(Card flipped);  //Player able to determine if they want to call the flipped card trump
        abstract public Card orderedUp(Card flipped);   //Player takes in the flipped card and discards one
        abstract public String callTrump(Card flipped);   //Player calls trump, but not of the flipped card suit

        public void dealtCard(Card givenCard)
        {
            hand.Add(givenCard);
        }

        public void updatedCardCount(Trick pastTrick)
        {
            counter.removeGroup(new ArrayList(pastTrick.getMoves()));
        }

        public void sortCards()
        {
            
        }

        //This method MUST be called by all children in their playCard method.
        public Boolean isLegalMove(Trick currentTrick, Card attemptedPlay)
        {
            Boolean isLegal = true;
            //If this card is not already onsuit, must check tobe sure hand has no onsuit cards in it
            if(!currentTrick.onSuitCheck(attemptedPlay))
            {
                for(int i=0; i<hand.Count; i++)
                {
                    //So, one of the cards in the hand is the attemptedPlay, and we need to ignore that one
                    //Otherwise, as soon as you find an onsuit card elsewhere, this play is illegal
                    if(currentTrick.onSuitCheck((Card)hand[i]) == true && hand[i] != attemptedPlay)
                    {
                        isLegal = false;
                    }
                }
            }
            return isLegal;
        }

        //This method is meant to handle the precedence of jacks in trump checking
        //Return of 0 means no, 1 means yes, 2 means left bower, 3 means right bower
        //Copied from the Trick method in order to provide comp players a more convenient means of checking
        public int trumpCheck(Card check, string trumpSuit)
        {
            //Is it trump?
            if (check.suit == trumpSuit)
            {
                //Check for right bower
                if (check.value == 11)
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
        //This method was copied from the Trick class in order to allow players to assess whether their own cards are better than other cards
        //Returns true if card 2 is a better card than card 1
        public Boolean isBetterCard(Card card1, Card card2, string trumpSuit)
        {
            int trump1Check = trumpCheck(card1, trumpSuit);
            int trump2Check = trumpCheck(card2, trumpSuit);

            //If the old one outtrumps the new one with any level of trump outcome
            //This step will take care of all jack infighting because jacks will never tie
            if (trump2Check > trump1Check)
            {
                return true;
            }
            else if (trump1Check > trump2Check)
            {
                return false;
            }
            //Equal level trump
            else if (trump1Check == 1 && trump2Check == 1)
            {
                if (card2.value > card1.value)
                    return true;
                else
                    return false;
            }
            //Check for onsuit plays, and all trump should've been covered before this
            //Note that the currentWinner will always be trump or onsuit, if not both
            else if (card2.suit == card1.suit)
            {
                if (card2.value > card1.value)
                    return true;
                else
                    return false;
            }
            //Okay so the new play must be offsuit at this point
            return false;
        }

        public override string ToString()
        {
            return name;
        }

        public void printHand()
        {
            Console.WriteLine(name + "'s hand:");
            for (int i = 0; i < hand.Count; i++)
            {
                Console.WriteLine("\t" + hand[i].ToString());
            }
        }
    }
}
