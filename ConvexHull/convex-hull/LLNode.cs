using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace _2_convex_hull
{
    class LLNode
    {
        private LLNode _next;
        private LLNode _prev;
        private PointF _point;

        public LLNode(PointF p)
        {
            _next = null;
            _prev = null;
            _point = p;
        }
        public double dX
        {
            get { return (double)_point.X; }
        }
        public double dY
        {
            get { return (double)_point.Y; }
        }
        public LLNode Next
        {
            get { return _next; }
            set { _next = value; }
        }
        public LLNode Prev
        {
            get { return _prev; }
            set { _prev = value; }
        }
        public PointF Point
        {
            get { return _point; }
        }
    }
}
