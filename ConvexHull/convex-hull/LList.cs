using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace _2_convex_hull
{
    class LList
    {
        private LLNode _leftMost;
        private LLNode _rightMost;
        private int _count;
        
        public LList()
        {
            _leftMost = null;
            _rightMost = null;
            _count = 0;
        }
        public LLNode LeftMost
        {
            get { return _leftMost; }
            set { _leftMost = value; }
        }
        public LLNode RightMost
        {
            get { return _rightMost; }
            set { _rightMost = value; }
        }
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        public void UpdateCount()
        {
            int tCount = 0;
            LLNode temp = _leftMost;
            do
            {
                tCount++;
                temp = temp.Next;
            }
            while ((temp.Point != _leftMost.Point) && (tCount < 5));
            //any count above 3 is not actually impotant
            _count = tCount;
        }

    }
}
