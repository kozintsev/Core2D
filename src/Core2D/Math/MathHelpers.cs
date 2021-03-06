﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;
using static System.Math;

namespace Core2D.Math
{
    /// <summary>
    /// Math helper methods.
    /// </summary>
    public static class MathHelpers
    {
        /// <summary>
        /// Converts an angle in decimal degrees to radians.
        /// </summary>
        /// <param name="angleInDegrees">Angle in decimal degrees.</param>
        /// <returns>Angle in radians.</returns>
        public static double DegreesToRadians(double angleInDegrees)
        {
            return angleInDegrees * (PI / 180.0);
        }

        /// <summary>
        /// Converts an angle in radians to decimal degrees.
        /// </summary>
        /// <param name="angleInRadians">Angle in radians</param>
        /// <returns>Angle in decimal degrees.</returns>
        public static double RadiansToDegrees(double angleInRadians)
        {
            return angleInRadians * (180.0 / PI);
        }

        /// <summary>
        /// Calculates angle between two lines.
        /// </summary>
        /// <param name="line1Start">The first line start point.</param>
        /// <param name="line1End">The first line end point.</param>
        /// <param name="line2Start">The second line start point.</param>
        /// <param name="line2End">The second line end point.</param>
        /// <returns>The angle between line in degrees.</returns>
        public static double AngleLineSegments(Point2 line1Start, Point2 line1End, Point2 line2Start, Point2 line2End)
        {
            double angle1 = Atan2(line1Start.Y - line1End.Y, line1Start.X - line1End.X);
            double angle2 = Atan2(line2Start.Y - line2End.Y, line2Start.X - line2End.X);
            double result = (angle2 - angle1) * 180.0 / PI;
            if (result < 0)
                result += 360.0;
            return result;
        }

        /// <summary>
        /// Calculate ellipse line segment intersection points.
        /// </summary>
        /// <param name="rect">The ellipse defining rectangle.</param>
        /// <param name="p1">The line segment start point.</param>
        /// <param name="p2">The line segment end point.</param>
        /// <param name="onlySegment">Include only line segment solutions.</param>
        /// <returns>The ellipse line segment intersection point list.</returns>
        public static IList<Point2> FindEllipseSegmentIntersections(Rect2 rect, Point2 p1, Point2 p2, bool onlySegment)
        {
            if ((rect.Width == 0) || (rect.Height == 0) || ((p1.X == p2.X) && (p1.Y == p2.Y)))
                return new Point2[] { };

            if (rect.Width < 0)
            {
                rect.X = rect.Right;
                rect.Width = -rect.Width;
            }

            if (rect.Height < 0)
            {
                rect.Y = rect.Bottom;
                rect.Height = -rect.Height;
            }

            double cx = rect.Left + rect.Width / 2.0;
            double cy = rect.Top + rect.Height / 2.0;

            rect.X -= cx;
            rect.Y -= cy;

            p1.X -= cx;
            p1.Y -= cy;
            p2.X -= cx;
            p2.Y -= cy;

            double a = rect.Width / 2.0;
            double b = rect.Height / 2.0;

            double A = (p2.X - p1.X) * (p2.X - p1.X) / a / a + (p2.Y - p1.Y) * (p2.Y - p1.Y) / b / b;
            double B = 2 * p1.X * (p2.X - p1.X) / a / a + 2 * p1.Y * (p2.Y - p1.Y) / b / b;
            double C = p1.X * p1.X / a / a + p1.Y * p1.Y / b / b - 1;

            var solutions = new List<double>();

            double discriminant = B * B - 4 * A * C;
            if (discriminant == 0)
            {
                solutions.Add(-B / 2 / A);
            }
            else if (discriminant > 0)
            {
                solutions.Add((-B + Sqrt(discriminant)) / 2 / A);
                solutions.Add((-B - Sqrt(discriminant)) / 2 / A);
            }

            var points = new List<Point2>();

            foreach (var t in solutions)
            {
                if (!onlySegment || ((t >= 0f) && (t <= 1f)))
                {
                    double x = p1.X + (p2.X - p1.X) * t + cx;
                    double y = p1.Y + (p2.Y - p1.Y) * t + cy;
                    points.Add(Point2.Create(x, y));
                }
            }

            return points;
        }

        /// <summary>
        /// Calculate distance between two points.
        /// </summary>
        /// <param name="x1">The X coordinate of first point.</param>
        /// <param name="y1">The Y coordinate of first point.</param>
        /// <param name="x2">The X coordinate of second point.</param>
        /// <param name="y2">The Y coordinate of second point.</param>
        /// <returns>The distance between two points.</returns>
        public static double Distance(double x1, double y1, double x2, double y2)
        {
            double dx = x1 - x2;
            double dy = y1 - y2;
            return Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Calculate coordinates of line segment middle point.
        /// </summary>
        /// <param name="x1">The X coordinate of line segment start point.</param>
        /// <param name="y1">The Y coordinate of line segment start point.</param>
        /// <param name="x2">The X coordinate of line segment end point.</param>
        /// <param name="y2">The Y coordinate of line segment end point.</param>
        /// <returns>The coordinates of line segment middle point.</returns>
        public static Vector2 Middle(double x1, double y1, double x2, double y2)
        {
            return new Vector2((x1 + x2) / 2.0, (y1 + y2) / 2.0);
        }

        /// <summary>
        /// Calculate coordinates of nearest point on line to the specified point.
        /// </summary>
        /// <param name="a">The line segment start point.</param>
        /// <param name="b">The line segment start end.</param>
        /// <param name="p">The point</param>
        /// <returns>The coordinates of nearest point on line to the specified point.</returns>
        public static Vector2 NearestPointOnLine(Vector2 a, Vector2 b, Vector2 p)
        {
            double ax = p.X - a.X;
            double ay = p.Y - a.Y;
            double bx = b.X - a.X;
            double by = b.Y - a.Y;
            double t = (ax * bx + ay * by) / (bx * bx + by * by);
            if (t < 0.0)
            {
                return new Vector2(a.X, a.Y);
            }
            else if (t > 1.0)
            {
                return new Vector2(b.X, b.Y);
            }
            return new Vector2(bx * t + a.X, by * t + a.Y);
        }

        /// <summary>
        /// Check if line intersects with rectangle using Liang-Barsky line clipping algorithm.
        /// </summary>
        /// <param name="rect">The rectangle shape.</param>
        /// <param name="p0">The line start point.</param>
        /// <param name="p1">The line end point.</param>
        /// <returns>True if line intersects with rectangle.</returns>
        public static bool LineIntersectsWithRect(Rect2 rect, Point2 p0, Point2 p1)
        {
            double left = rect.Left;
            double right = rect.Right;
            double bottom = rect.Bottom;
            double top = rect.Top;
            double x0 = p0.X;
            double y0 = p0.Y;
            double x1 = p1.X;
            double y1 = p1.Y;

            double t0 = 0.0;
            double t1 = 1.0;
            double dx = x1 - x0;
            double dy = y1 - y0;
            double p = 0.0, q = 0.0, r;

            for (int edge = 0; edge < 4; edge++)
            {
                if (edge == 0)
                {
                    p = -dx;
                    q = -(left - x0);
                }
                if (edge == 1)
                {
                    p = dx;
                    q = (right - x0);
                }
                if (edge == 2)
                {
                    p = dy;
                    q = (bottom - y0);
                }
                if (edge == 3)
                {
                    p = -dy;
                    q = -(top - y0);
                }

                r = q / p;

                if (p == 0.0 && q < 0.0)
                {
                    return false;
                }

                if (p < 0.0)
                {
                    if (r > t1)
                    {
                        return false;
                    }
                    else if (r > t0)
                    {
                        t0 = r;
                    }
                }
                else if (p > 0.0)
                {
                    if (r < t0)
                    {
                        return false;
                    }
                    else if (r < t1)
                    {
                        t1 = r;
                    }
                }
            }

            // Calculate clipped line position.
            // x0clip = x0 + t0 * dx;
            // y0clip = y0 + t0 * dy;
            // x1clip = x0 + t1 * dx;
            // y1clip = y0 + t1 * dy;

            return true;
        }
    }
}
