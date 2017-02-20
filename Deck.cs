using System;
using System.Collections;
using System.Linq;

namespace Euchre
{
    /*The deck class is just meant to be a holder of all the cards in the euchre class.
     * Every round should start with a new deck. What's left in the deck after dealing is the kitty.
     * 
     */
    class Deck
    {
        private ArrayList deck = new ArrayList();
       
        public Deck()
        {
            //Create all cards
            deck = new ArrayList();

            for (int i = 0; i < 4; i++ )
            {
                for (int j = 9; j < 15; j++)
                {
                    Card temp = new Card(i, j);
                    deck.Add(temp);
                }
            }
            shuffle();
        }

        //Randomize the order of all cards 
        //Typically called by dealer, not by deck itself
        public void shuffle()
        {
            ArrayList temp = new ArrayList();
            Random rand = new Random();

            int index;
            while(deck.Count > 0)
            {
                index = rand.Next(deck.Count);
                temp.Add(deck[index]);
                deck.RemoveAt(index);
            }
            deck = temp;
        }

        public Card draw()
        {
            //Dealing off the bottom of my deck like my no-good cousin Caroline
            Card c = (Card)deck[0];
            deck.RemoveAt(0);
            return c;
        }

        public void newDeck()
        {
            ArrayList newDeck = new ArrayList();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 9; j < 15; j++)
                {
                    Card temp = new Card(i, j);
                    newDeck.Add(temp);
                }
            }
            deck = newDeck;
        }

        public override string ToString()
        {
            string returnString = "";
            foreach (Card c in deck)
            {
                returnString += c.ToString();
                returnString += "\n";
            }
            return returnString;
        }
    }
}
