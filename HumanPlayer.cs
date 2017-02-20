using System;
using System.Collections;

namespace Euchre
{
    class HumanPlayer : Player
    {
        /*This class inherits from a generic player and allows a human player to make all player choices.
         * Developed primarily as debugging method for testing euchre game mechanics.
         */

        public HumanPlayer()
        {
            Console.WriteLine("Welcome to the game, Human Player!");
            //Temp name code
            Console.Write("Please choose a name: ");
            name = Console.ReadLine();

            hand = new ArrayList();
        }

        override public Card leadCard(Hand tableHand)
        {
            analyzeHand();
            Console.WriteLine(name + ", which card would you like to lead?");
            Console.Write("Index number: ");
            int choice;
            Int32.TryParse(Console.ReadLine(), out choice);

            Card play = (Card)hand[choice];
            hand.Remove(play);
            return play;
        }
        override public Card playCard(Hand tableHand)
        {
            Console.WriteLine(tableHand.currentTrick.ToString());
            analyzeHand();
            Console.WriteLine(name + ", which card would you like to play?");
            Console.Write("Index number: ");
            int choice;
            Int32.TryParse(Console.ReadLine(), out choice);

            Card play = (Card)hand[choice];
            while(isLegalMove(tableHand.currentTrick, play) != true)
            {
                Console.WriteLine(play.ToString() + " is not a valid move. You must follow suit.");
                Console.WriteLine(name + ", which card would you like to play?");
                Console.Write("Index number: ");
                Int32.TryParse(Console.ReadLine(), out choice);

                play = (Card)hand[choice];
            }
            hand.Remove(play);
            return play;
        }

        public void analyzeHand()
        {
            Console.WriteLine("");
            Console.WriteLine(name + ", your hand:");
            for(int i=0; i<hand.Count; i++)
            {
                Console.WriteLine("\t" + i + ": " + hand[i].ToString());
            }
        }

        public override Boolean orderUp(Card flipped)
        {
            analyzeHand();
            Console.Write(name + "'s decision: order up a " + flipped + "? Y/N  ");
            Boolean valid = false;
            while(!valid)
            {
                string response = Console.ReadLine();
                if (response.Equals("N", StringComparison.OrdinalIgnoreCase) || response.Equals("No", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                else if (response.Equals("Y", StringComparison.OrdinalIgnoreCase) || response.Equals("Yes", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else
                    Console.Write("Please pick a valid resonse. Y/N  ");
            }
            return valid;
        }
        public override Card orderedUp(Card flipped)
        {
            analyzeHand();
            Console.WriteLine(name + ", which card would you like to discard to the kitty?");
            Console.Write("Index number: ");
            int choice; 
            Int32.TryParse(Console.ReadLine(), out choice);
            Card discard = (Card)hand[choice];

            hand.Add(flipped);
            hand.Remove(discard);
            return discard;
        }
        public override String callTrump(Card flipped)
        {
            analyzeHand();
            Console.Write(name + ", call trump? N/SuitName  ");
            string choice = "";
            Boolean valid = false;
            while (!valid)
            {
                choice = Console.ReadLine();
                if (choice.Equals("Hearts", StringComparison.OrdinalIgnoreCase) && !choice.Equals(flipped.suit, StringComparison.OrdinalIgnoreCase))
                {
                    return "Hearts";
                }
                else if (choice.Equals("Diamonds", StringComparison.OrdinalIgnoreCase) && !choice.Equals(flipped.suit, StringComparison.OrdinalIgnoreCase))
                {
                    return "Diamonds";
                }
                else if (choice.Equals("Spades", StringComparison.OrdinalIgnoreCase) && !choice.Equals(flipped.suit, StringComparison.OrdinalIgnoreCase))
                {
                    return "Spades";
                }
                else if (choice.Equals("Clubs", StringComparison.OrdinalIgnoreCase) && !choice.Equals(flipped.suit, StringComparison.OrdinalIgnoreCase))
                {
                    return "Clubs";
                }
                else if (choice.Equals("N", StringComparison.OrdinalIgnoreCase) || choice.Equals("No", StringComparison.OrdinalIgnoreCase))
                {
                    return "N";
                }
                else
                    Console.Write("Please pick a valid resonse. N/SuitName  ");
            }
            return choice;
        }
    }
}
