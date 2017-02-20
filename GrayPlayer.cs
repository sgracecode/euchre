using System;
using System.Collections;

namespace Euchre
{
    class GrayPlayer : Player
    {
        /*This class inherits from a generic player. Implementation is centered around the developer's (Gray's) play style.
         */

        public GrayPlayer()
        {
            Console.WriteLine("Welcome to the game, Gray!");
            name = "Gray";

            hand = new ArrayList();
        }
        //This version is in case I want a unique name for reasons of telling them apart
        public GrayPlayer(string extraName)
        {
            name = "Gray" + extraName;
            Console.WriteLine("Welcome to the game, " + name + "!");

            hand = new ArrayList();
        }

        override public Card leadCard(Hand tableHand)
        {
            printHand();
            Card play = null;

            //Early rounds: offsuit high or shortsuit
            if (hand.Count > 3)
            {
                //offsuit ace
                if (findOffsuitValue(tableHand, 14) > -1)
                {
                    play = (Card)hand[findOffsuitValue(tableHand, 14)];
                }
                //offsuit king
                else if (findOffsuitValue(tableHand, 13) > -1)
                {
                    play = (Card)hand[findOffsuitValue(tableHand, 13)];
                }
                //shortsuit self
                else if (findShortsuitableLow(tableHand) > -1)
                {
                    play = (Card)hand[findShortsuitableLow(tableHand)];
                }
                else
                    play = (Card)hand[findWorst(tableHand)];
            }
            else
                play = (Card)hand[findWorst(tableHand)];

            Console.WriteLine(name + " will lead with " + play);
           
            hand.Remove(play);
            return play;
        }
        

        override public Card playCard(Hand tableHand)
        {
            printHand();
            Card play = null;

            //Only consider valid moves
            ArrayList validMoves = new ArrayList();
            foreach(Card c in hand)
            {
                if(isLegalMove(tableHand.currentTrick, c))
                    validMoves.Add(c);
            }
            //If there's only one option that's cool
            if (validMoves.Count == 1)
            {
                play = (Card)validMoves[0];
            }
            //There shouldn't be a mix of on or off suit, so check one should be fine
            else if (tableHand.currentTrick.onSuitCheck((Card)validMoves[0]))
            {
                //The return value from this method is a value in validMoves
                play = (Card)validMoves[findOnsuitPlay(tableHand.currentTrick, validMoves)];
            }
            //We're able to play offsuit here, oh boy!
            else
            {
                //Easiest situation: our parner has already played, and we'd like to let our partner win or win
                if(tableHand.currentTrick.playCount >= 2)
                {
                    //Check if partner is winning because if so, we want to throw off
                    if(tableHand.seats[tableHand.currentTrick.currentWinner] == teammate)
                    {
                        if (findShortsuitableLow(tableHand) > -1)
                            play = (Card)hand[findShortsuitableLow(tableHand)];
                        else
                            play = (Card)hand[findWorst(tableHand)];
                    }
                    //So partner is not winning, let's win if we can
                    else
                    {
                        if (findTrumpLow(tableHand, true) > -1)
                            play = (Card)hand[findTrumpLow(tableHand, true)];
                        //If you can't play a trump and you can't follow suit, it's impossible to win.
                        else if (findShortsuitableLow(tableHand) > -1)
                            play = (Card)hand[findShortsuitableLow(tableHand)];
                        else
                            play = (Card)hand[findWorst(tableHand)];
                    }
                }
                //Most important situation: we are second to play, and we can do offsuit
                else
                {
                    //We can play any trump if we have it. Just go low as possible
                    if (findTrumpLow(tableHand, false) > -1)
                        play = (Card)hand[findTrumpLow(tableHand, false)];
                    //Otherwise guess we're boned again
                    else if (findShortsuitableLow(tableHand) > -1)
                        play = (Card)hand[findShortsuitableLow(tableHand)];
                    else
                        play = (Card)hand[findWorst(tableHand)];
                }
            }
            if (!isLegalMove(tableHand.currentTrick, play))
                Console.WriteLine("ERROR: ILLEGAL PLAY BY " + ToString());
            Console.WriteLine(ToString() + " has decided to play a " + play);
            hand.Remove(play);
            return play;
        }
        

        //This is the points-based analysis of the strength of the hand based on a particular suit being trump
        //Only looks at own cards, not partner or field
        public int analyzeHand(string trumpSuit)
        {
            int score = 0;
            foreach (Card c in hand)
            {
                //either bower +4
                if (c.value == 11)
                {
                    if ((trumpSuit == "Spades" || trumpSuit == "Clubs") && (c.suit == "Spades" || c.suit == "Clubs"))
                        score += 4;
                    else if ((trumpSuit == "Hearts" || trumpSuit == "Diamonds") && (c.suit == "Hearts" || c.suit == "Diamonds"))
                        score += 4;
                }
                //King or Ace of trump +3
                else if ((c.value > 12) && (c.suit == trumpSuit))
                    score += 3;
                //Other trump +2
                else if (c.suit == trumpSuit)
                    score += 2;
                //Offsuit Ace +2
                else if ((c.value == 14) && (c.suit != trumpSuit))
                    score += 2;
            }
            //Short suited +1
            if (suitCount(trumpSuit, hand) == 3)
                score += 1;
            else if (suitCount(trumpSuit, hand) <= 2)
                score += 2;

            return score;
        }
        //For the purposes of shortsuiting
        private int suitCount(string trumpSuit, ArrayList cards)
        {
            int hasHearts = 0;
            int hasDiamonds = 0;
            int hasSpades = 0;
            int hasClubs = 0;

            foreach (Card c in cards)
            {
                if (c.suit == "Hearts")
                {
                    if (trumpSuit == "Diamonds" && c.value == 11)
                        hasDiamonds = 1;
                    else
                        hasHearts = 1;
                }
                else if (c.suit == "Diamonds")
                {
                    if (trumpSuit == "Hearts" && c.value == 11)
                        hasHearts = 1;
                    else
                        hasDiamonds = 1;
                }
                else if (c.suit == "Spades")
                {
                    if (trumpSuit == "Clubs" && c.value == 11)
                        hasClubs = 1;
                    else
                        hasSpades = 1;
                }
                else if (c.suit == "Clubs")
                {
                    if (trumpSuit == "Spades" && c.value == 11)
                        hasSpades = 1;
                    else
                        hasClubs = 1;
                }
                else
                    Console.WriteLine("ERROR: Unrecognized suit in Gray's suitCount method.");
            }

            return hasHearts + hasDiamonds + hasSpades + hasClubs;
        }

        public override Boolean orderUp(Card flipped)
        {
            int handStrength = analyzeHand(flipped.suit);
            //Decision modification based on dealer
            if(teammate.isDealer == true)
            {
                if (flipped.value > 12 || flipped.value == 11)
                    handStrength += 2;
            }
            else if(isDealer == true)
            {
                //Implement +1 if able to shortsuit self

                //Will be adding new card to hand SO
                if (flipped.value == 11)
                    handStrength += 4;
                else if (flipped.value > 12)
                    handStrength += 3;
                else
                    handStrength += 2;
            }
            //Opposing team has deal
            else
            {
                if (flipped.value == 11)
                    handStrength -= 3;
                else if (flipped.value > 11)
                    handStrength -= 2;
                else
                    handStrength -= 1;
            }
            //Final modifying decisions
            //Implement chicken out and force call

            //Temp visuals
            printHand();
            Console.WriteLine(ToString() + " has " + handStrength + " points.");
            //Choice
            if (handStrength > 8)
                return true;
            else
                return false;
        }
        public override Card orderedUp(Card flipped)
        {
            //discard preference: short suit low, low card off trump, short suit ace, lowest card
            ArrayList tempHand = new ArrayList(hand);
            Card bestChoice = null;
            bool shortSuitable = false;

            Card c;
            for (int i = 0; i < hand.Count; i++ )
            {
                c = (Card)tempHand[0];
                tempHand.Remove(c);
                //not trump card
                if (trumpCheck(c, flipped.suit) == 0)
                {
                    //We can short suit by removing c
                    if (suitCount(flipped.suit, tempHand) < suitCount(flipped.suit, hand))
                    {
                        //There was already a shortsuitable card
                        if (shortSuitable)
                        {
                            if (bestChoice != null && c.value < bestChoice.value)
                                bestChoice = c;
                        }
                        //This card is shortsuitable and is not an Ace
                        else if (c.value < 14)
                        {
                            shortSuitable = true;
                            bestChoice = c;
                        }
                    }
                    //Still not trump, but not shortsuitable, so pick the lower value (or we haven't picked one yet)
                    if (bestChoice == null || c.value < bestChoice.value)
                        bestChoice = c;
                }
                //Go around again so put c back in
                tempHand.Add(c);
            }
            //If for whatever reason we haven't picked a best choice..... just pick lowest
            if(bestChoice == null)
            {
                bestChoice = (Card)hand[0];
                foreach (Card card in hand)
                {
                    if (card.value < bestChoice.value)
                        bestChoice = card;
                }
            }
            //Now we should definitely have our choice so commit to it
            printHand();
            hand.Add(flipped);
            hand.Remove(bestChoice);
            Console.WriteLine(ToString() + " chooses to discard a " + bestChoice);
            return bestChoice;
        }
        public override String callTrump(Card flipped)
        {
            int hearts = analyzeHand("Hearts");
            Console.WriteLine(ToString() + " has " + hearts + " points for hearts");
            int diamonds = analyzeHand("Diamonds");
            Console.WriteLine(ToString() + " has " + diamonds + " points for diamonds");
            int spades = analyzeHand("Spades");
            Console.WriteLine(ToString() + " has " + spades + " points for spades");
            int clubs = analyzeHand("Clubs");
            Console.WriteLine(ToString() + " has " + clubs + " points for clubs");

            printHand();

            //We'll call trump if we meet the threshold and it's legal
            if (hearts > 9 && flipped.suit != "Hearts")
            {
                if (hearts >= diamonds && hearts >= spades && hearts >= clubs)
                    return "Hearts";
            }
            if (diamonds > 9 && flipped.suit != "Diamonds")
            {
                if (diamonds >= hearts && diamonds >= spades && diamonds >= clubs)
                    return "Diamonds";
            }
            if (spades > 9 && flipped.suit != "Spades")
            {
                if (spades >= hearts && spades >= diamonds && spades >= clubs)
                    return "Spades";
            }
            if (clubs > 9 && flipped.suit != "Clubs")
            {
                if (clubs >= hearts && clubs >= diamonds && clubs >= spades)
                    return "Clubs";
            }
            //Screw the dealer forced call
            if (isDealer)
            {
                if (hearts >= diamonds && hearts >= spades && hearts >= clubs)
                    return "Hearts";
                if (diamonds >= hearts && diamonds >= spades && diamonds >= clubs)
                    return "Diamonds";
                if (spades >= hearts && spades >= diamonds && spades >= clubs)
                    return "Spades";
                if (clubs >= hearts && clubs >= diamonds && clubs >= spades)
                    return "Clubs";
            }
            //We don't want to call trump
            return "N";
        }

        /*
         * FINDER METHODS - for looking up certain cards in hand
         * 
         */

        //-1 means there wasn't an offsuit of that value
        private int findOffsuitValue(Hand tableHand, int value)
        {
            int index = -1;
            for (int i = 0; i < hand.Count; i++)
            {
                //Card is an ace and isn't trump
                if (((Card)hand[i]).value == value && tableHand.currentTrick.trumpCheck((Card)hand[i]) == 0)
                {
                    //Possibly implement picking /which/ offsuit ace/etc to go with
                    index = i;
                }
            }
            return index;
        }
        //-1 means it wasn't possible to shortsuit, also this method ignores trump
        private int findShortsuitableLow(Hand tableHand)
        {
            int index = -1;
            Card c;
            ArrayList tempHand = new ArrayList(hand);
            for (int i = 0; i < hand.Count; i++)
            {
                c = (Card)tempHand[0];
                tempHand.Remove(c);
                //Card isn't trump
                if (tableHand.currentTrick.trumpCheck(c) == 0)
                {
                    //would be further shortsuited without c
                    if (suitCount(tableHand.currentTrick.trumpSuit, tempHand) < suitCount(tableHand.currentTrick.trumpSuit, hand))
                    {
                        //there has not been a shortsuitable card, OR
                        //there was another shortsuitable card, but it was higher than this one so we'll keep it
                        if ((index > -1 && ((Card)hand[index]).value > c.value) || index == -1)
                        {
                            index = i;
                        }
                    }
                }
                tempHand.Add(c);
            }
            return index;
        }
        //Finds the worst card left in the hand, based on trump. Must return a valid index.
        private int findWorst(Hand tableHand)
        {
            int index = 0;
            for (int i = 1; i < hand.Count; i++)
            {
                //Index overtrumps i card
                if (trumpCheck((Card)hand[index], tableHand.currentTrick.trumpSuit) > trumpCheck((Card)hand[i], tableHand.currentTrick.trumpSuit))
                    index = i;
                //Equal level trump so check value
                else if (trumpCheck((Card)hand[index], tableHand.currentTrick.trumpSuit) == trumpCheck((Card)hand[i], tableHand.currentTrick.trumpSuit))
                {
                    if (((Card)hand[index]).value > ((Card)hand[i]).value)
                        index = i;
                }
            }
            return index;
        }
        //Finds the lowest trump card in hand , return -1 if no trump fits criteria
        //Send the bool TRUE if you want to only do it if you can win the trick
        private int findTrumpLow(Hand tableHand, bool win)
        {
            int index = -1;
            //Collect the opponent's moves
            ArrayList opponentMoves = new ArrayList(tableHand.currentTrick.getMoves());
            for (int i = 0; i < hand.Count; i++)
            {
                //If it is a trump card
                if (trumpCheck((Card)hand[i], tableHand.currentTrick.trumpSuit) > 0)
                {
                    //If we don't already have a trump and we don't care about winning
                    if (index == -1 && !win)
                        index = i;
                    //We don't have a trump but we DO care about winning
                    else if(index == -1 && win)
                    {
                        index = i;
                        //So if we find an opponent card that can beat it, it's not a valid move
                        for(int j=0; j<opponentMoves.Count; j++)
                        {
                            if ((Card)opponentMoves[j] != null && (isBetterCard((Card)hand[i], (Card)opponentMoves[j], tableHand.currentTrick.trumpSuit)))
                                index = -1;
                        }
                    }
                    //So we do do have a trump somewhere
                    else if(index > -1 && !win)
                    {
                        //If the new one is worse, replace it
                        if (isBetterCard((Card)hand[i], (Card)hand[index], tableHand.currentTrick.trumpSuit))
                            index = i;
                    }
                    //So we want to be able to win, and we already have a card that can
                    else if(index > -1 && win)
                    {
                        //Replace only if the new card can beat the opponent moves but is beaten by the old index
                        bool canWin = true;
                        for (int j = 0; j < opponentMoves.Count; j++)
                        {
                            if ((Card)opponentMoves[j] != null && (isBetterCard((Card)hand[i], (Card)opponentMoves[j], tableHand.currentTrick.trumpSuit)))
                                canWin = false;
                        }
                        if (canWin)
                            if (isBetterCard((Card)hand[i], (Card)hand[index], tableHand.currentTrick.trumpSuit))
                                index = i;
                    }
                }
            }
            return index;
        }

        //Finds the best onsuit play, which is win with the worst of the best, or just go with the worst
        //It's given that there's at least two valid moves. Must return a valid move
        public int findOnsuitPlay(Trick currentTrick, ArrayList validMoves)
        {
            int index = 0;
            bool winnable = false;
            for (int i = 0; i < validMoves.Count; i++)
            {
                //If we can beat the current card
                if (isBetterCard(currentTrick.getMoves()[currentTrick.currentWinner], (Card)validMoves[i], currentTrick.trumpSuit))
                {
                    //There was no previous winning card
                    if (!winnable)
                    {
                        winnable = true;
                        index = i;
                    }
                    //There is already a winnable card
                    else
                    {
                        //If the old card can beat the new card, we want to use the new one to save the better
                        if (isBetterCard((Card)validMoves[i], (Card)validMoves[index], currentTrick.trumpSuit))
                            index = i;
                    }
                }
                //This card can't beat the current card, and there isn't yet a card that can
                else if (!winnable)
                {
                    //If the old card can beat the new card, we want to use the new to save our better one
                    if (isBetterCard((Card)validMoves[i], (Card)validMoves[index], currentTrick.trumpSuit))
                        index = i;
                }
            }
            return index;
        }
    }
}
