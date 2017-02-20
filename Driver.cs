using System;

namespace Euchre
{
    class Driver
    {
        static void Main(string[] args)
        {
            EuchreGameController game = new EuchreGameController();
            game.UserGameSetup();
            game.playGame();

            /*If you feel like simulating 50 games at a time:
            for (int i = 0; i < 50; i++)
            {
                game = new EuchreGameController();
                game.UserGameSetup();
                game.playGame();
            }
             */


            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();            
        }
    }
}
