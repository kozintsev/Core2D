// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Core2D.Project;
using Core2D.Shape;
using Core2D.Shapes;
using Core2D.Style;
using Core2D.Math;

namespace Core2D.Editor.Tools.Selection
{
    /// <summary>
    /// 
    /// </summary>
    public class CircleSelection
    {
        private readonly XLayer _layer;
        private readonly XEllipse _arc;
        private readonly ShapeStyle _style;
        private readonly BaseShape _point;
        private XLine _startLine;
        private XEllipse _ellipse;
        private XPoint _centerHelperPoint;
        private XPoint _endHelperPoint;

        /// <summary>
        /// Initialize new instance of <see cref="ArcSelection"/> class.
        /// </summary>
        /// <param name="layer">The selection shapes layer.</param>
        /// <param name="shape">The selected shape.</param>
        /// <param name="style">The selection shapes style.</param>
        /// <param name="point">The selection point shape.</param>
        public CircleSelection(XLayer layer, XEllipse shape, ShapeStyle style, BaseShape point)
        {
            _layer = layer;
            _arc = shape;
            _style = style;
            _point = point;
        }

        /// <summary>
        /// Transfer selection state to <see cref="ToolState.One"/>.
        /// </summary>
        public void ToStateOne()
        {
            _centerHelperPoint = XPoint.Create(0, 0, _point);
            _endHelperPoint = XPoint.Create(0, 0, _point);
            _startLine = XLine.Create(0, 0, _style, null);

            _layer.Shapes = _layer.Shapes.Add(_centerHelperPoint);
            _layer.Shapes = _layer.Shapes.Add(_endHelperPoint);
            _layer.Shapes = _layer.Shapes.Add(_startLine);
        }


        /// <summary>
        /// Move selection.
        /// </summary>
        public void Move()
        {
            //var a = WpfArc.FromXArc(_arc);
            var p1 = Point2.Create(_ellipse.TopLeft.X, _ellipse.TopLeft.Y);
            var p2 = Point2.Create(_ellipse.BottomRight.X, _ellipse.BottomRight.Y);
            var rect = Rect2.Create(p1, p2);
            var center = Point2.Create(rect.X + rect.Width / 2.0, rect.Y + rect.Height / 2.0);
            double offsetX = center.X - rect.X;
            double offsetY = center.Y - rect.Y;
            var R = center.Y + offsetY;

            if (_startLine != null)
            {
                _startLine.Start.X = center.Y;
                _startLine.Start.Y = center.X;
                _startLine.End.X = center.X;
                _startLine.End.Y = R;
            }


            if (_centerHelperPoint != null)
            {
                _centerHelperPoint.X = center.X;
                _centerHelperPoint.Y = center.X;
            }


            if (_endHelperPoint != null)
            {
                _endHelperPoint.X = center.X;
                _endHelperPoint.Y = R;
            }

            _layer.Invalidate();
        }

        /// <summary>
        /// Remove selection.
        /// </summary>
        public void Remove()
        {

            if (_startLine != null)
            {
                _layer.Shapes = _layer.Shapes.Remove(_startLine);
                _startLine = null;
            }


            if (_centerHelperPoint != null)
            {
                _layer.Shapes = _layer.Shapes.Remove(_centerHelperPoint);
                _centerHelperPoint = null;
            }


            if (_endHelperPoint != null)
            {
                _layer.Shapes = _layer.Shapes.Remove(_endHelperPoint);
                _endHelperPoint = null;
            }

            _layer.Invalidate();
        }
    }
}
