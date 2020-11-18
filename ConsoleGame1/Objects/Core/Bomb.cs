using ConsoleGame1.Objects.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame1.Objects.Core
{
    public class Bomb : BaseObject
    {
        // in ms
        public readonly uint TIMER_COUNT = 5000;
        public readonly uint BLINKING = 500;
        public readonly uint EXPLOSION_DURATION = 1500;

        // time in ms when the bomb was planted
        public long Created { get; private set; }

        public long ExplosionTime { get; private set; }

        public BombState State { get; private set; }

        public override char Character { get
            {
                if (blink)
                {
                    return 'B';
                } else
                {
                    return 'b';
                }
            } 
        }

        private long blinkingTime;
        private bool blink;

        public Bomb()
        {
            Created = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            State = BombState.TICKING;
            blinkingTime = Created;
            blink = false;
        }

        public void Blink()
        {
            if (State == BombState.TICKING && (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - BLINKING > blinkingTime)
            {
                blink = !blink;
                blinkingTime = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
            }
        }

        public bool IsExplosion()
        {
            return State == BombState.TICKING && (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - TIMER_COUNT > Created;
        }

        public void Explode()
        {
            State = BombState.EXPLODED;
            ExplosionTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public bool ExplosionEnded()
        {
            return State == BombState.EXPLODED && (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - EXPLOSION_DURATION > ExplosionTime;
        }

        public bool IsDead()
        {
            return State == BombState.DEAD;
        }

        public void Die()
        {
            State = BombState.DEAD;
        }

        public IEnumerable<uint[]> GetExplosionPoints()
        {
            List<uint[]> points = new List<uint[]>();
            points.Add(new uint[] { X, Y });
            points.Add(new uint[] { (X+1)%Game.GAME_WIDTH, Y });
            points.Add(new uint[] {  X, (Y+1)%Game.GAME_HEIGHT });
            if (X == 0)
            {
                points.Add(new uint[] { Game.GAME_WIDTH - 1, Y });
            } else
            {
                points.Add(new uint[] { X-1, Y });
            }

            if (Y == 0)
            {
                points.Add(new uint[] { X, Game.GAME_HEIGHT - 1 });
            } else
            {
                points.Add(new uint[] { X, Y-1 });
            }

            return points;
        }
    }
}
