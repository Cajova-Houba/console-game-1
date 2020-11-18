using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame1.Objects.Core
{
    public class Game
    {
        public static readonly uint GAME_WIDTH = 10;
        public static readonly uint GAME_HEIGHT = 10;

        public static readonly uint PLAYER_START_X = 5;
        public static readonly uint PLAYER_START_Y = 5;

        public static readonly double DIAMOND_CHANCE = 0.3;

        public const char DIAMOND_CHAR = 'D';
        public const char WALL_CHAR = 'W';
        public const char ROOM_CHAR = ' ';

        public char[,] Map { get; private set; }

        public Player Player { get; }

        public List<Bomb> Bombs { get; private set; }

        private Random r;

        public Game()
        {
            Player = new Player() { X = Game.PLAYER_START_X, Y = Game.PLAYER_START_Y };
            Player.Game = this;
            r = new Random();
        }

        public void BombExplosion(Bomb bomb)
        {
            bomb.Explode();
            IEnumerable<uint[]> explosionPoints = bomb.GetExplosionPoints();
            foreach (uint[] explosionPoint in explosionPoints)
            {
                switch(Map[explosionPoint[1], explosionPoint[0]])
                {
                    case WALL_CHAR:
                        Map[explosionPoint[1], explosionPoint[0]] = r.NextDouble() > (1-DIAMOND_CHANCE) ? DIAMOND_CHAR : ROOM_CHAR;
                        break;
                }
            }
        }

        public bool PickUpDiamond(uint x, uint y)
        {
            if (IsDiamond(x,y))
            {
                Map[y, x] = ROOM_CHAR;
                return true;
            }

            return false;
        }

        public bool ContainsBomb(uint x, uint y)
        {
            foreach (Bomb bomb in Bombs)
            {
                if (bomb.IsHere(x,y))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to place a bomb on given coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>True if the bomb was planted, false otherwise.</returns>
        public bool PlantBomb(uint x, uint y)
        {
            if (!ContainsBomb(x, y))
            {
                Bombs.Add(new Bomb() { X = x, Y = y });
                return true;
            }

            return false;
        }

        internal bool IsDiamond(uint x, uint y)
        {
            return Map[y, x] == DIAMOND_CHAR;
        }

        public void NewLevel()
        {
            Bombs = new List<Bomb>();
            Player.X = Game.PLAYER_START_X;
            Player.Y = Game.PLAYER_START_Y;
            Player.AddBombs();
            GenerateMap();
        }

        public void GenerateMap()
        {
            Map = new char[GAME_HEIGHT,GAME_WIDTH];
            for (int i = 0; i < GAME_HEIGHT; i++)
            {
                for (int j = 0; j < GAME_WIDTH; j++)
                {
                    Map[i,j] = ROOM_CHAR;
                    if (i == PLAYER_START_Y && j == PLAYER_START_X)
                    {
                        continue;
                    } else if (r.NextDouble() > 0.5)
                    {
                        Map[i,j] = WALL_CHAR;
                    }
                }
            }
        }

        public bool IsWall(uint x, uint y)
        {
            return Map[y, x] == WALL_CHAR;
        }
    }
}
