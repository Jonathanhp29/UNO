using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Linq;
using System.Xml;

namespace UnoAttempt
{
    public class Card
    {
        //private dictonary used to assign action cards
        private Dictionary<int, string> actions = new Dictionary<int, string>
        {
            {10, "Skip"},
            {11, "Reverse"},
            {12, "+2"},
            {13, "Wild"},
            {14, "Wild +4" }
        };
        public string colour;
        public int value;
        public string action;


        public Card(string colour,int Value)
        {
            this.colour = colour;
            this.value = Value;
            //Checks if the card is an action
            if (Value > 9)
            {
                this.action = actions[value];
                
                //Checks for if wild
                if (this.value > 13)
                {
                    this.colour = "None";
                }
            }
            else
            {
                this.action = "None";
            }
        }

        public string compare(string Colour, int Value)
        {
            if (Colour == colour & Value == value)
            {
                return "Both";
            }
            else if (value > 12)
            {
                return "Wild";
            }
            else if (Colour == colour)
            {
                return "Colour";
            }
            else if (Value == value)
            {
                return "Value";
            }
            
            else
            {
                return "None";
            }
        }

        public override string ToString()
        {
            //Checks for wilds || actions
            if (value < 10)
            {
                return $"{colour} {value}";
            }
            else if (value < 13)
            {
                return $"{colour} {actions[value]}";
            }
            else
            {
                return $"{actions[value]}";
            }
        }
    }

    public class Player
    {
        public List<Card> hand;
        public int playerID;

        public override string ToString()
        {
            return $"Player {playerID + 1}";
        }

        public Player(Deck deck, int ID)
        {
            // Creates a 7 carded hand using GiveHand method
            hand = deck.GiveHand(7);

            // ID used for player order
            playerID = ID;
        }

        public Card selectCard(bool error = false)
        {
            // Checks and sees if theres been an error
            if (error)
            {
                Console.WriteLine("You can not play that card");
            }

            Console.WriteLine();
            // shows the whole hand
            for (int i = 0; i < hand.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {hand[i]}");
            }
            Console.WriteLine();

            int choice;

            // Loop will repeate untill valid input
            while (true)
            {
                Console.WriteLine($"Enter number 1 - {hand.Count}: ");
                // Checks if is number
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    // Checks if valid number
                    if (choice < 1 || choice > hand.Count)
                    {
                        Console.WriteLine("Please enter valid number\n");
                    }
                    else
                    {
                        break;
                    }
                }

                else
                {
                    Console.WriteLine("Please enter a number\n");
                }
            }

            return hand[choice - 1];
        }

    }

    public class Deck
    {
        //Private array used to create cards
        private String[] colours = ["blue", "red", "yellow", "green"];
        public List<Card> deck;

        public Deck()
        {
            deck = [];
            
            foreach (string colour in colours)
            {
                //Repeats process for each colour
                for (int i = 0; i < 15; i++)
                {
                    //Checks for 0 or wild to only add 1 of that colour
                    if (i == 0 || i > 12)
                    {
                        //Creates card then adds to deck (Scard = special card)
                        Card Scard = new(colour, i);
                        deck.Add(Scard);
                        continue;
                    }

                    //Creates card then adds to deck twice
                    Card card = new(colour, i);
                    deck.Add(card);
                    deck.Add(card);
                }
            }
        }

        public List<Card> GiveHand(int num, bool topCard = false)
        {
            // Checks if the request is too high and reshuffles the deck if so
            if (num > deck.Count())
            {
                foreach (string colour in colours)
                {
                    //Repeats process for each colour
                    for (int i = 0; i < 15; i++)
                    {
                        //Checks for 0 or wild to only add 1 of that colour
                        if (i == 0 || i > 12)
                        {
                            //Creates card then adds to deck (Scard = special card)
                            Card Scard = new(colour, i);
                            deck.Add(Scard);
                            continue;
                        }

                        //Creates card then adds to deck twice
                        Card card = new(colour, i);
                        deck.Add(card);
                        deck.Add(card);
                    }
                }
            }

            //Creates random numebr generator
            Random r = new();

            //Creates empty list for hand
            List<Card> hand = new();

            //Checks if a top card is being drawn
            if (topCard)
            {
                //Generates a random num
                int randNum = r.Next(0, deck.Count);

                //Checks if card is wild
                while (deck[randNum].value > 12)
                {
                    randNum = r.Next(0, deck.Count);
                }

                //Once card isnt wild adds to hand and removes from deck
                hand.Add(deck[randNum]);
                deck.RemoveAt(randNum);

                return hand;
            }

            //Repeats num times for an uno hand
            for (int i=0; i < num; i++)
            {
                //generates a random num adds to hand and removes from deck
                int randNum = r.Next(0, deck.Count);

                hand.Add(deck[randNum]);
                deck.RemoveAt(randNum);
            }

            return hand;
        }
    }

    public class GameManager
    {
        private string[] colours = ["blue", "red", "yellow", "green"];
        private string gameColour;
        private int topValue;
        private bool direction = true;
        private int activePlayer;
        private bool activeAction = false;

        public List<Player> playerList = new();

        public void StartGame(Deck deck)
        {
            int playerCount;
            // Asks for player count 
            Console.Write("Enter player count (2-4): ");

            // Loop continues until the user provides a string that successfully converts to an int
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out playerCount))
                {
                    if (playerCount > 4 || playerCount < 2)
                    {
                        Console.WriteLine("Please input valid number:\n");
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Please input valid number:\n");
                }
            }
            
            
            // Creates correct number of players and adds IDs to playerOrder
            for (int i = 0; i < playerCount; i++)
            {
                Player player = new(deck, i);
                playerList.Add(player);
            }

            //Creates a card
            Card card = deck.GiveHand(1, true)[0];

            topValue = card.value;
            gameColour = card.colour;
        }

        private bool checkValid(Card card)
        {
            // Checks if wild
            if (card.value > 12)
            {
                return true;
            }
            
            // Chekcs if values are equal
            else if (card.value == topValue)
            {
                return true;
            }

            // Checks if colour is same
            else if (card.colour == gameColour)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        private bool checkAbleToPlay(Player player)
        {
            // Checks every card in the players hand
            foreach (Card card in player.hand)
            {
                // Checks if card is able to play
                if (checkValid(card))
                {
                    return true;
                }
            }

            return false;
        }
        
        private Card aiSelection(Player player)
        {
            // Looks at every card in hand and 
            List<Card> validCards = [];
            foreach(Card card in player.hand)
            {
                if (checkValid(card))
                {
                    validCards.Add(card);
                }
            }

            // If theres only 1 card valid returns that
            if (validCards.Count == 1)
            {
                return validCards[0];
            }

            // Randomly selects a card from the players hand and returns it
            Random r = new();
            int randNum = r.Next(0, validCards.Count);
            return validCards[randNum];

        }

        private bool isAction(Card card)
        {
            if (card.action == "None")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void wildColourChange(Player player, bool ai = false)
        {
            // Checks if ai is set to true and selects a colour based on hand
            if (ai)
            {
                // Creates a sorted list of colours by number of cards of a colour
                List<string> groupedColours = player.hand.GroupBy(c => c.colour)
                                                .OrderByDescending(g => g.Count())
                                                .SelectMany(g => g)
                                                .Select(g => g.colour)
                                                .ToList();

                // Selects the most common colour and sets top colour as selection
                gameColour = groupedColours[0];
                return;
            }
            
            // Asks for a colour choice and loop does not end untill matches
            string choice = "None";
            // num used to detect 1st loop iteration
            int num = 0;
            while (!colours.Contains(choice.ToLower()))
            {
                if (num != 0)
                {
                    Console.WriteLine("Thats not a valid colour ");
                }
                Console.WriteLine("Colour selection: ");
                choice = Console.ReadLine();
                num++;
            }

            gameColour = choice;
            return;
        }

        private void playCard(Player player, Card card, bool ai = false)
        {
            // Checks how card is valid and changes colour and value appropatetly
            string matches = card.compare(gameColour, topValue);
            switch (matches)
            {
                case "Both":
                    break;
                
                case "Colour":
                    topValue = card.value;
                    break;

                case "Value":
                    gameColour = card.colour;
                    break;
                
                case "Wild":
                    wildColourChange(player, ai);
                    topValue = card.value;
                    break;
            }


            // Changes if needed active action and direction
            if (isAction(card))
            {
                switch (card.value)
                {
                    case 10:
                        player.hand.Remove(card);
                        activeAction = true;
                        return;
                    case 11:
                        // Checks the direction then flips it
                        if (direction)
                        {
                            player.hand.Remove(card);
                            direction = false;
                            return;
                        }
                        else
                        {
                            player.hand.Remove(card);
                            direction = true;
                            return;
                        }
                    case >= 12:
                        player.hand.Remove(card);
                        activeAction = true;
                        return;

                }

            }

            player.hand.Remove(card);
            return;
        }

        private void aiTurn(Player player, Deck deck)
        {
            // Checks if theres an active action happening then performs action
            if (activeAction)
            {
                activeAction = false;
                /* 
            {10, "Skip"},
            {11, "Reverse"},
            {12, "+2"},
            {13, "Wild"},
            {14, "Wild +4" }
                */

                switch (topValue)
                {
                    case 10:
                        Console.WriteLine($"\nPlayer {activePlayer} has been skipped");
                        return;

                    case 12:
                        Console.WriteLine($"\nPlayer {activePlayer} picked up 2 cards.\nPlayer {activePlayer} now has {player.hand.Count + 2} cards.");
                        List<Card> cards2 = deck.GiveHand(2);
                        player.hand.AddRange(cards2);
                        return;
                    case 14:
                        Console.WriteLine($"\nPlayer {activePlayer} picked up 4 cards.\nPlayer {activePlayer} now has {player.hand.Count + 4} cards.");
                        List<Card> cards4 = deck.GiveHand(4);
                        player.hand.AddRange(cards4);
                        return;
                }
            }
            
            // Checks if can play
            bool canPlay = checkAbleToPlay(player);

            // Terminates turn if needed
            if (!canPlay)
            {
                Console.WriteLine($"\nPlayer {activePlayer} picked up a card.\nPlayer {activePlayer} now has {player.hand.Count + 1} cards.");
                List<Card> Ncard = deck.GiveHand(1);
                player.hand.AddRange(Ncard);

                return;
            }

            

            // Selects and plays a card
            Card selectedCard = aiSelection(player);

            if (selectedCard.value > 12)
            {
                playCard(player, selectedCard, true);
                Console.WriteLine($"\nPlayer {activePlayer} played a {selectedCard}");
                Console.WriteLine($"The new colour is {gameColour}");
                if (selectedCard.value == 14)
                {
                    topValue = 14;
                }
                return;
            }

            playCard(player, selectedCard, true);
            Console.WriteLine($"\nPlayer {activePlayer} played a {selectedCard}");
            return;
        } 

        private void playerTurn(Player player, Deck deck)
        {
            // Creates a top card to compare with
            Card topCard = new(gameColour, topValue);

            // Checks if theres an active action happening then performs action
            if (activeAction)
            {
                activeAction = false;
                
                switch (topValue)
                {
                    case 10:
                        Console.WriteLine($"\nYou have been skipped.");
                        return;

                    case 12:
                        Console.WriteLine($"\nYou picked up 2 cards.");
                        List<Card> cards2 = deck.GiveHand(2);
                        Console.WriteLine($"A {cards2[0]} and a {cards2[1]}");
                        player.hand.AddRange(cards2);
                        return;

                    case 14:
                        Console.WriteLine($"\nYou picked up 4 cards.\nYou now has {player.hand.Count + 4} cards.");
                        List<Card> cards4 = deck.GiveHand(4);
                        Console.WriteLine($"A {cards4[0]}, a {cards4[1]}, a {cards4[2]} and a {cards4[3]}");
                        player.hand.AddRange(cards4);
                        return;
                }
            }

            // Checks if you can play
            bool canPlay = checkAbleToPlay(player);

            // Tells the player the top card
            Console.WriteLine($"The top card is a {topCard}");
            // Terminates if needed
            if (!canPlay)
            {
                Console.WriteLine($"\nYou can not play");
                List<Card> card = deck.GiveHand(1);
                Console.WriteLine($"You picked up a {card[0]}");
                player.hand.Add(card[0]);
                return;
            }


            //creates impossible card for 1st iteration
            Card cardS = new("None", -1);
            bool first = true;
            // Loop checking if the card can be played and wont stop till a valid card is selected
            while (cardS.compare(gameColour, topValue) == "None")
            {
                if (first)
                {
                    first = false;
                    cardS = player.selectCard();
                    continue;
                }
                cardS = player.selectCard(true);
            }

            playCard(player, cardS);
        }

        public Player playGame(Deck deck)
        {
            // Does round prep
            StartGame(deck);

            int playerIndex = 0;
            Player currentPlayer = playerList[0];
            while (true)
            {
                // selects the current player
                currentPlayer = playerList[playerIndex];
                activePlayer = playerIndex + 1;

                // Checks if its the player's or AI's turn
                if (playerIndex == 0)
                {
                    playerTurn(currentPlayer, deck);
                }
                else
                {
                    aiTurn(currentPlayer, deck);
                    Console.ReadKey();
                }

                // Checks if they have won
                if (currentPlayer.hand.Count() < 1)
                {
                    break;
                }

                // Checks direction and changes the playerIndex
                if (direction)
                {
                    if (playerIndex + 1 < playerList.Count())
                    {
                        playerIndex++;
                    }
                    else
                    {
                        playerIndex = 0;
                    }
                }
                else
                {
                    if (playerIndex - 1 > -1)
                    {
                        playerIndex--;
                    }
                    else
                    {
                        playerIndex = playerList.Count() - 1;
                    }
                }

            }

            return currentPlayer;
        }

    }
}
