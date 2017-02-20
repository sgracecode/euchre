using System;
using System.Collections;

namespace Euchre
{
    class EuchreGameController
    {
        /*This class should control 1 entire euchre game from start to finish. 
         * 
         */

        //Make this more generic later
        FourPlayerTable table;

        //Set up what kind of game will occur
        public void UserGameSetup()
        {
            Console.WriteLine("Welcome to Euchre!");
            Console.WriteLine("If there were options, I would give you some.\n");

            //Determine what type of computer players will be in our game

            //COMMENT THIS SECTION IN FOR 3 COMPUTERS AND 1 PLAYER
            Player playerOne = new GrayPlayer();
            Player playerTwo = new GrayPlayer("Acie");
            Player playerThree = new GrayPlayer("Ve");
            Player playerFour = new HumanPlayer();
            

            /* COMMENT THIS SECTION IN FOR 4 HUMAN PLAYERS
            Player playerOne = new HumanPlayer();
            Player playerTwo = new HumanPlayer();
            Player playerThree = new HumanPlayer();
            Player playerFour = new HumanPlayer();
             * */

            // If you're feeling adventurous, COMMENT THIS SECTION FOR 4 COMP PLAYERS
            /*Player playerOne = new GrayPlayer();
            Player playerTwo = new GrayPlayer("Gray");
            Player playerThree = new GrayPlayer("Two");
            Player playerFour = new GrayPlayer("GrayGray");
            */
            //Instantiate the correct table
            table = new FourPlayerTable(playerOne, playerTwo, playerThree, playerFour);

        }

        public void playGame()
        {
            Console.WriteLine("The game begins.");
            int round = 1;

            while(table.gameOver != true)
            {
                Console.WriteLine("\nHand " + round);
                Console.WriteLine("Current score " + table.team02Score + " to " + table.team13Score + "\n");
                table.deal();
                table.chooseTrump();
                table.playHand();

                round++;
                table.shiftDeal();
            }

            //Congratulate winner 
            table.congratulate();
        }
    }
}
