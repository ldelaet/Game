using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLorenzo
{
    static class Collission
    {
        const int penMargin = 5;
        public static bool isOnTopOf(this Rectangle r1, Rectangle r2)
        {
            return (r1.Bottom >= r2.Top - penMargin &&
                r1.Bottom <= r2.Top + (r2.Height / 2) &&
                r1.Right >= r2.Left + 5 &&
                r1.Left <= r2.Right - 5);
        }
        public static bool isOnBottomOf(this Rectangle r1, Rectangle r2)
        {
            return (r1.Top >= r2.Bottom - penMargin &&
            r1.Top <= r2.Bottom + (r2.Height / 2) - 40 &&
            r1.Right >= r2.Left - 5 &&
                r1.Left <= r2.Right + 5);
        }
        public static bool isOnLeft(this Rectangle r1, Rectangle r2)
        {
            return (
                r1.Left >= r2.Right - 2 &&
                r1.Left <= r2.Right + 2 &&
                r1.Bottom >= r2.Top + penMargin &&
                r1.Top <= r2.Bottom - penMargin
                );
        }
        public static bool isOnRight(this Rectangle r1, Rectangle r2)
        {
            return (
                r1.Right >= r2.Left - 2 &&
                r1.Right <= r2.Left + 2 &&
                r1.Bottom >= r2.Top + penMargin &&
                r1.Top <= r2.Bottom - penMargin
                );
        }
    }
}
