using System;
using UnoAttempt;

namespace Uno
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

                GameManager manager = new();
                Deck deck = new();

                // plays a round
                Player winner = manager.playGame(deck);

                // Announcess the winner and asks if user wants to play again
                Console.WriteLine($"\n{winner} has won!");
                Console.WriteLine("Play agian(Y/N): ");
                string answer = Console.ReadLine();

                if (answer.ToLower() != "y")
                {
                    break;
                }

            }
            
        }
    }
}