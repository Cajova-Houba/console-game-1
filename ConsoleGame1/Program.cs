using System;
using System.Threading;
using ConsoleGame1.Objects.Core;

namespace ConsoleGame1
{
    class Program
    {
        static Game game = new Game();

        static void Main(string[] args)
        {
            PrintWelcome();
            GameLoop();
            EndGame();
        }

        private static void EndGame()
        {
            Console.SetCursorPosition(0, (int)Game.GAME_HEIGHT + 2);
        }

        static void PrintWelcome()
        {
            Console.WriteLine("Welcome to my game!");
            Console.WriteLine("Pres ESC to quit.");
            Console.CursorVisible = false;
            game.NewLevel();
        }

        static void GameLoop()
        {
            bool endGame = false;
            while (!endGame)
            {
                while (!Console.KeyAvailable)
                {
                    CheckBombs();
                    DrawGame();
                    DrawInfo();
                }

                ConsoleKeyInfo input = Console.ReadKey(true);
                switch (input.Key)
                {
                    case ConsoleKey.W:
                        game.Player.MoveUp();
                        break;

                    case ConsoleKey.S:
                        game.Player.MoveDown();
                        break;

                    case ConsoleKey.A:
                        game.Player.MoveLeft();
                        break;

                    case ConsoleKey.D:
                        game.Player.MoveRight();
                        break;

                    case ConsoleKey.N:
                        game.NewLevel();
                        break;

                    case ConsoleKey.Spacebar:
                        game.Player.PlantBomb();
                        break;

                    case ConsoleKey.Escape:
                        endGame = true;
                        break;
                }

                Thread.Sleep(100);
            }
        }

        private static void DrawInfo()
        {
            Console.SetCursorPosition(20, 2);
            Console.Write("Player info");
            Console.SetCursorPosition(20, 3);
            Console.Write("===========");
            Console.SetCursorPosition(20, 4);
            Console.Write("Bombs: " + game.Player.BombsLeft.ToString());
            Console.SetCursorPosition(20, 5);
            Console.Write("Diamonds: " + game.Player.Diamonds.ToString());
        }

        /// <summary>
        /// Check bombs for explosion.
        /// </summary>
        private static void CheckBombs()
        {
            foreach (Bomb bomb in game.Bombs)
            {
                if (bomb.IsExplosion())
                {
                    game.BombExplosion(bomb);
                } else if (bomb.ExplosionEnded())
                {
                    bomb.Die();
                } else
                {
                    bomb.Blink();
                }
            }
            game.Bombs.RemoveAll(b => b.IsDead());
        }

        static void DrawGame()
        {

            // buffer for printing map
            char[][] mapBuffer = new char[Game.GAME_HEIGHT][];

            // draw map to the buffer
            for (uint i = 0; i < Game.GAME_HEIGHT; i++)
            {
                mapBuffer[i] = new char[Game.GAME_WIDTH];
                for (uint j = 0; j < Game.GAME_WIDTH; j++)
                {
                    mapBuffer[i][j] = game.Map[i, j];
                }
            }

            // bombs
            foreach (Bomb bomb in game.Bombs)
            {
                mapBuffer[bomb.Y][bomb.X] = bomb.Character;
                if (bomb.State == BombState.EXPLODED)
                {
                    foreach (uint[] explosionPoint in bomb.GetExplosionPoints())
                    {
                        mapBuffer[explosionPoint[1]][ explosionPoint[0]] = 'X';
                    }
                }
            }

            // player
            mapBuffer[game.Player.Y][game.Player.X] = game.Player.Character;

            // print the buffer
            Console.SetCursorPosition(0, 2);
            foreach (char[] row in mapBuffer)
            {
                Console.WriteLine(row);
            }
        }
    }
}
