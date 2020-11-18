using ConsoleGame1.Objects.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame1.Objects.Core
{
    public class Player : BaseObject
    {
        public readonly uint INIT_BOMBS = 5;

        public Game Game { get; set; }

        public uint BombsLeft { get; private set; }
        public uint Diamonds { get; set; }

        public override char Character { get
            {
                return 'P';
            } 
        }

        public Player()
        {
            BombsLeft = INIT_BOMBS;
            Diamonds = 0;
        }

        public void AddBombs()
        {
            BombsLeft = INIT_BOMBS;
        }

        public void PlantBomb()
        {
            if(BombsLeft > 0 && Game.PlantBomb(X, Y))
            {
                BombsLeft--;
            }
        }

        public void MoveUp()
        {
            uint newY = Y;
            if (Y == 0)
            {
                newY = Game.GAME_HEIGHT - 1;
            } else
            {
                newY -= 1;
            }

            Move(X, newY);
        }

        public void MoveDown()
        {
            uint newY = (Y + 1) % Game.GAME_HEIGHT;

            Move(X, newY);
        }

        public void MoveLeft()
        {
            uint newX = X;
            if (newX == 0)
            {
                newX = Game.GAME_WIDTH - 1;
            }
            else
            {
                newX -= 1;
            }

            Move(newX, Y);
        }

        public void MoveRight()
        {
            uint newX = (X + 1) % Game.GAME_WIDTH;

            Move(newX, Y);
        }

        private void Move(uint newX, uint newY)
        {
            if (!Game.IsWall(newX, newY))
            {
                X = newX;
                Y = newY;

                if (Game.PickUpDiamond(X, Y))
                {
                    Diamonds++;
                }
            }
        }
    }
}
