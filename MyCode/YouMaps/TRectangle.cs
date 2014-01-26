using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouMaps
{
    public struct TRectangle
    {
        public TRectangle(int x, int y, int width, int height) : this()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;

        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public override int GetHashCode()
        {
            return X ^ Y ^ Width ^ Height;
        }

        public override bool Equals(object obj)
        {
            return obj is TRectangle && (TRectangle)obj == this;
        }

        public static bool operator ==(TRectangle rect1, TRectangle rect2)
        {
            return rect1.X == rect2.X && rect1.Y == rect2.Y && rect1.Width == rect2.Width && rect1.Height == rect2.Height;
        }

        public static bool operator !=(TRectangle rect1, TRectangle rect2)
        {
            return !(rect1 == rect2);
        }
    }


}
