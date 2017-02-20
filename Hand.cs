using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euchre
{
    class Hand
    {
        /*This class holds all 5 tricks that happen in a hand. Trump is constant. Trick winning score is kept here.
         * The main purpose is for players and computers to be able to count cards. Everything in hand is public knowledge.
         */
        public Player[] seats;
        public int dealer;      //corresponds to index in seats
        public int leader;      //corresponds to index in seats

        public int caller;      //corresponds to index in seats
        public Card flipped;
        public string trump;

        public Trick currentTrick;
        public ArrayList pastTricks = new ArrayList();

        public int team02Tricks = 0; //team with seats 0 and 2
        public int team13Tricks = 0; //team with seats 1 and 3

        //A new hand gets initiated by the table as dealership and score have been set up properly.
        public Hand(Player[] players, int handDealer)
        {
            seats = players;
            dealer = handDealer;
            leader = ((dealer + 1) % 4);
        }

        public void nextTrick()
        {
            if(currentTrick != null)    //only the first trick should move past this
            {
                pastTricks.Add(currentTrick);
                foreach(Player p in seats)
                {
                    p.updatedCardCount(currentTrick);
                }
            }
            currentTrick = new Trick(seats, trump);
            //Leader plays, then rotate around whole table
            Card played = seats[leader].leadCard(this);
            currentTrick.leadPlay(played, leader);

            for (int i = 1; i <= 3; i++)
            {
                played = seats[(leader + i) % 4].playCard(this);
                currentTrick.addPlay(played, ((leader + i) % 4));
            }
            //update next leader as winner of trick
            int winner = currentTrick.winner();
            leader = winner;
            //Update cumulative trick-winning total
            if(winner == 0 || winner == 2)
            {
                team02Tricks++;
            }
            else if (winner == 1 || winner == 3)
            {
                team13Tricks++;
            }
            else
            {
                Console.WriteLine("ERROR: Problem determining trick winner in Hand");
            }
        }
    }
}
