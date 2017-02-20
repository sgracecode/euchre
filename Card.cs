using System;

namespace Euchre
{
    class Card
    {
        public string suit { get; set; }
        public int value { get; set; }

        public Card(string newSuit, int newValue)
        {
            suit = newSuit;
            value = newValue;
        }
        public Card(int newSuit, int newValue)
        {
            if (newSuit == 0)
                suit = "Hearts";
            else if (newSuit == 1)
                suit = "Diamonds";
            else if (newSuit == 2)
                suit = "Spades";
            else if (newSuit == 3)
                suit = "Clubs";
            else
                Console.WriteLine("Error: Attempting to assign an invalid suit to a card.");

            value = newValue;
        }

        public override bool Equals(System.Object obj)
        {
            if((Card)obj == null)
                return false;
            return (((Card)obj).value == this.value && ((Card)obj).suit == this.suit);
        }

        public override string ToString()
        {
            string valueString;
            if (value < 11)
                valueString = value.ToString();
            else if (value == 11)
                valueString = "Jack";
            else if (value == 12)
                valueString = "Queen";
            else if (value == 13)
                valueString = "King";
            else if (value == 14)
                valueString = "Ace";
            else
                valueString = "Card Error: Invalid value.";

            return valueString + " of " + suit;
        }
    }
}
