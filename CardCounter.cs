using System;
using System.Collections;
using System.Linq;

namespace Euchre
{
    class CardCounter
    {
        public ArrayList existingCards = new ArrayList();
        public ArrayList playedCards = new ArrayList();

        /*This class is for computer players to conveniently keep track of which cards have and have not been played
         * 
         */ 
        public CardCounter()
        {
            //Start with all cards existing
            for (int i = 0; i < 4; i++)
            {
                for (int j = 9; j < 15; j++)
                {
                    Card temp = new Card(i, j);
                    existingCards.Add(temp);
                }
            }
        }

        public void removeGroup(ArrayList cards)
        {
            foreach (Card c in cards)
            {
                existingCards.Remove(c);
            }
        }
        public void removeCard(Card card)
        {
            existingCards.Remove(card);
        }

        public int trumpRemaining(string trumpSuit)
        {
            int trumpCount = 0;
            foreach(Card c in existingCards)
            {
                if (c.suit == trumpSuit)
                    trumpCount++;
                if (trumpSuit == "Hearts")
                {
                    if (c.suit == "Diamonds" && c.value == 11)
                        trumpCount++;
                }
                else if (trumpSuit == "Diamonds")
                {
                    if (c.suit == "Hearts" && c.value == 11)
                        trumpCount++;
                }
                else if (trumpSuit == "Spades")
                {
                    if (c.suit == "Clubs" && c.value == 11)
                        trumpCount++;
                }
                else if (trumpSuit == "Clubs")
                {
                    if (c.suit == "Spades" && c.value == 11)
                        trumpCount++;
                }
            }
            return trumpCount;
        }
        //This is for a nontrump suit. Trump is above.
        public int suitRemaining(string suit, string trumpSuit)
        {
            int suitCount = 0;
            foreach (Card c in existingCards)
            {
                if (c.suit == suit)
                    suitCount++;
                if (trumpSuit == "Hearts")
                {
                    if (c.suit == "Diamonds" && c.value == 11)
                        suitCount--;
                }
                else if (trumpSuit == "Diamonds")
                {
                    if (c.suit == "Hearts" && c.value == 11)
                        suitCount--;
                }
                else if (trumpSuit == "Spades")
                {
                    if (c.suit == "Clubs" && c.value == 11)
                        suitCount--;
                }
                else if (trumpSuit == "Clubs")
                {
                    if (c.suit == "Spades" && c.value == 11)
                        suitCount--;
                }
            }
            return suitCount;
        }

        public override string ToString()
        {
            string returnString = "";
            foreach (Card c in existingCards)
            {
                returnString += c.ToString();
                returnString += "\n";
            }
            return returnString;
        }
    }
}
