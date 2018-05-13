using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace rasterizer
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var points = GetPointsOnLine(2.2F, 1F, 8.2F, 4F);
            var list = new List<Point>();
            foreach (var point in points)
            {
                Console.WriteLine(point);
            }

        }
        static void line(int x1, int y1, int x2, int y2)
        {
            int w = x2 - x1;
            int h = y2 - y1;
            int x = x1;
            int y = y1;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest && (x != x2 || y != y2); i++)
            {
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
                Console.WriteLine("x:" + x + " Y: " + y);
            }
        }

        //x0 and y0 are initial point
        //x1 and y1 are final points
        //For example, if direction of line is upwards, y0 must be less than y1 so that outer tiles are rasterized.
        // Similarly, if the direction is right x0 must be less than x1
        public static IEnumerable<PointF> GetPointsOnLine(float x0, float y0, float x1, float y1)
        {
            float dy = Math.Abs(y0 - y1);
            float dx = Math.Abs(x0 - x1);
            bool steep = dy > dx;
            if (steep)
            {
                bool directionDown = y0 > y1;
                float m = dx / dy;

                //return the point tile as it is
                var initialTile = new Point(Convert.ToInt32(Math.Floor(x0)), Convert.ToInt32(Math.Floor(y0)));
                var finalTile = new Point(Convert.ToInt32(Math.Floor(x1)), Convert.ToInt32(Math.Floor(y1)));
                yield return initialTile;
                if (initialTile == finalTile || initialTile.Y == finalTile.Y + 1)
                {
                    yield return finalTile;
                    yield break;
                }
                float xTemp = directionDown ? (x0 - (y0 % 1) * m) - m : (1 - (y0 % 1)) * m + x0; //x-coordinates for second tile
                if (directionDown)
                {
                    for (int y = initialTile.Y - 1; y > finalTile.Y; y--)
                    {
                        int x = Convert.ToInt32(Math.Floor(xTemp));
                        yield return new Point(x, y);
                        xTemp -= m;
                    }
                }
                else
                {
                    for (int y = initialTile.Y + 1; y < finalTile.Y; y++)
                    {
                        int x = Convert.ToInt32(Math.Floor(xTemp));
                        yield return new Point(x, y);
                        xTemp += m;
                    }
                }
                yield return finalTile;
            }
            else
            {
                bool directionLeft = x0 > x1;
                float m = dy / dx;

                //return the point tile as it is
                var initialTile = new Point(Convert.ToInt32(Math.Floor(x0)), Convert.ToInt32(Math.Floor(y0)));
                var finalTile = new Point(Convert.ToInt32(Math.Floor(x1)), Convert.ToInt32(Math.Floor(y1)));
                yield return initialTile;
                if (initialTile == finalTile || initialTile.X == finalTile.X + 1)
                {
                    yield return finalTile;
                    yield break;
                }
                float yTemp = directionLeft ? (y0 - (x0 % 1) * m) - m : (1 - (x0 % 1)) * m + y0; //x-coordinates for second tile
                if (directionLeft)
                {
                    for (int x = initialTile.X - 1; x > finalTile.X; x--)
                    {
                        int y = Convert.ToInt32(Math.Floor(yTemp));
                        yield return new Point(x, y);
                        yTemp -= m;
                    }
                }
                else
                {
                    for (int x = initialTile.X + 1; x < finalTile.X; x++)
                    {
                        int y = Convert.ToInt32(Math.Floor(yTemp));
                        yield return new Point(x, y);
                        yTemp += m;
                    }
                }
                yield return finalTile;
            }
            yield break;
        }

        public static IEnumerable<PointF> GetPointsOnLine2(float x0, float y0, float x1, float y1)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                float t;
                t = x0; // swap x0 and y0
                x0 = y0;
                y0 = t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
            }
            if (x0 > x1)
            {
                float t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = y0; // swap y0 and y1
                y0 = y1;
                y1 = t;
            }
            float dx = x1 - x0;
            float dy = Math.Abs(y1 - y0);
            float error = dx / 2;
            float ystep = (y0 < y1) ? 1 : -1;
            float y = y0;
            for (float x = x0; x <= x1; x++)
            {
                yield return new PointF((steep ? y : x), (steep ? x : y));
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
            yield break;
        }
    }
}
