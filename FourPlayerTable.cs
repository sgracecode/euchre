using System;
using System.Collections;

namespace Euchre
{
    class FourPlayerTable
    {
        /*Table class includes who is sitting where and keeps score.
        * 
        * Table also must deal because we don't trust players.
        * The dealer is tracked by a number that goes through 0-3 and back to 0 (in the seats array)
        */

        private Player[] seats = new Player[4];

        private Hand currentHand;

        private int dealer; //corresponds to index in seats

        protected Deck deck;
        protected Deck kitty;

        public int team02Score;
        public int team13Score;
        public bool gameOver { get; set; }
        public int playUntil = 3;

        public FourPlayerTable(Player one, Player two, Player three, Player four)
        {
            gameOver = false; 

            seats[0] = one;
            seats[1] = two;
            seats[2] = three;
            seats[3] = four;

            //Set first dealer and inform that player they're dealer
            dealer = 0;
            seats[dealer].isDealer = true;

            //Tell players who their teammates are
            //Player one and three are teammates, two and four are teammates
            seats[0].teammate = seats[2];
            seats[2].teammate = seats[0];
            Console.WriteLine("");
            Console.WriteLine("Player " + seats[0] + ", your partner is Player " + seats[2]);
            seats[1].teammate = seats[3];
            seats[3].teammate = seats[1];
            Console.WriteLine("Player " + seats[1] + ", your partner is Player " + seats[3]);
        }

        public Player shiftDeal()
        {
            seats[dealer].isDealer = false;
            dealer = (dealer + 1) % 4;  //If player four is dealer, shift around to player one
            //Inform players of dealer change
            seats[dealer].isDealer = true;
            return seats[dealer];
        }

        public void deal()
        {
            currentHand = new Hand(seats, dealer);
            Console.WriteLine("");
            Console.WriteLine("Dealer this round is " + seats[dealer] + ".");
            deck = new Deck();
            
            //Everyone gets 5 cards and the kitty gets the rest.
            for(int i = 0; i < 5; i++)
            {
                seats[0].dealtCard(deck.draw());
                seats[1].dealtCard(deck.draw());
                seats[2].dealtCard(deck.draw());
                seats[3].dealtCard(deck.draw());
            }
            currentHand.flipped = deck.draw();
            Console.WriteLine("A " + currentHand.flipped + " was flipped.");
            kitty = deck;
        }

        public void chooseTrump()
        {
            //See if flipped is called
            for(int i=1; i<=4; i++)
            {
                if(seats[(dealer + i) % 4].orderUp(currentHand.flipped))
                {
                    Console.WriteLine(seats[dealer] + " picks up the " + currentHand.flipped);
                    currentHand.trump = currentHand.flipped.suit;
                    currentHand.caller = (dealer + i) % 4;
                    seats[dealer].orderedUp(currentHand.flipped);
                    Console.WriteLine("");
                    Console.WriteLine(seats[currentHand.caller] + " has declared trump " + currentHand.trump);
                    return;
                }
            }
            //See who wants to call trump now
            for(int i=1; i<=3; i++)
            {
                //Response should arrive pre-cleansed to be valid (N or proper suit)
                string response = seats[(dealer + i) % 4].callTrump(currentHand.flipped);
                if(!response.Equals("N",StringComparison.OrdinalIgnoreCase))
                {
                    currentHand.trump = response;
                    currentHand.caller = (dealer + i) % 4;
                    Console.WriteLine("");
                    Console.WriteLine(seats[currentHand.caller] + " has declared trump " + currentHand.trump);
                    return;
                }
            }
            //NEED TO CODE FORCED TRUMP CALLING RESPONSE
            Console.WriteLine("");
            Console.WriteLine("Screw the dealer - you must call trump.");
            string dealerResponse = seats[dealer].callTrump(currentHand.flipped);
            currentHand.trump = dealerResponse;
            currentHand.caller = dealer;
            Console.WriteLine("");
            Console.WriteLine(seats[currentHand.caller] + " has declared trump " + currentHand.trump);
            return;
        }

        //Info about cards that have been played are in a Hand object that players may see
        public void playHand()
        {
            //Play within trick is handled in trick class, which was set up in dealing/trump choosing
            //5 hands occur, no short circuiting, 
            for (int i = 0; i < 5; i++ )
            {
                currentHand.nextTrick();
            }
            //For scoring, check which team greater 
            if(currentHand.team02Tricks > currentHand.team13Tricks)
            {
                //If won all 5 or euchred, get extra point
                if (team02Score == 5 || (currentHand.caller != 0 && currentHand.caller != 2))
                    team02Score++;
                team02Score++;
                Console.WriteLine("\nPlayers " + seats[0] + " and " + seats[2] + 
                    " won this hand with " + currentHand.team02Tricks + " tricks.");
            }
            else if(currentHand.team02Tricks < currentHand.team13Tricks)
            {
                if (team13Score == 5 || (currentHand.caller != 1 && currentHand.caller != 3))
                    team13Score++;
                team13Score++;
                Console.WriteLine("\nPlayers " + seats[1] + " and " + seats[3] +
                    " won this hand with " + currentHand.team13Tricks + " tricks.");
            }
            else
            {
                Console.WriteLine("ERROR: Problem determining hand winner in Table");
            }
            //Now check if someone reached the max score
            if (team02Score >= playUntil || team13Score >= playUntil)
            {
                gameOver = true;
            }
        }

        //Declare winner
        public void congratulate()
        {
            if (team02Score > team13Score)
            {
                Console.WriteLine("\n\nCongratulations, Players " + seats[0] + 
                    " and " + seats[2] + " have won!");
            }
            else if (team02Score < team13Score)
            {
                Console.WriteLine("\n\nCongratulations, Players " + seats[1] +
                    " and " + seats[3] + " have won!");
            }
        }
    }
}
