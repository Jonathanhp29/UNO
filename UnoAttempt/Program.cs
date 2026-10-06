using System;
using UnoAttempt;

namespace Uno
{
    class Program
    {
        static void Main(string[] args)
        {
            GameManager manager = new();
            Deck deck = new();

            Player player = new Player(deck, 0);

            /*foreach(Card card in deck.deck)
            {
                Console.WriteLine(card);
                Console.WriteLine(card.compare("Blue", 10) + "\n");
            }
            //manager.StartGame(deck);
            //Player player = new Player(deck, 1);
            //Console.WriteLine(player.selectCard());*/
        }
    }
}