using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;

namespace _2_convex_hull
{
    class ConvexHullSolver
    {
        private DateTime timeUp;

        public class XComparer : IComparer<PointF>
        {
            int IComparer<PointF>.Compare(PointF a, PointF b)
            {
                if (b.X > a.X)
                    return 1;
                if (b.X < a.X)
                    return -1;

                return 0;
            }
        }

        public void Solve(System.Drawing.Graphics g, List<System.Drawing.PointF> pointList)
        {
            // Insert your code here.
            
            //Add all the points to a SortedList sorted by x-value
    //        List<PointF> slist = new List<PointF>();
    //        for (int i = 0; i < pointList.Count; i++)
    //        {
    //            if (!slist.Contains(pointList[i]))
    //            {
    //                slist.Add(pointList[i]);
    //            }
    //        }

            System.Collections.Generic.
            HashSet<PointF> hashPoints = new HashSet<PointF>();
            for (int i = 0; i < pointList.Count; i++)
            {
                hashPoints.Add(pointList[i]);
            }
            List<PointF> slist = ((IEnumerable<PointF>)hashPoints).ToList<PointF>();
            slist.Sort(new XComparer());

            //testing sorting visually
     //       drawList(g, slist);
            //Start timer
        //    timeUp = DateTime.Now.AddSeconds(5);

            //Recursively divide and conqueror
            LList hull = divAndCon(g, slist);
            drawLList(g, hull, new Pen(Color.Blue));
        }

        private LList divAndCon(Graphics g, List<PointF> slist)
        {
            if (slist.Count > 1)
            {
                //Find middle
                int mid = slist.Count / 2;
                float midX = slist[mid].X;
                PointF minY = slist[0];
                PointF maxY = slist[0];

                //Separate into left and right lists
                List<PointF> leftList = new List<PointF>();
                List<PointF> rightList = new List<PointF>();
                for (int i = 0; i < slist.Count; i++)
                {
                    if (slist[i].X <= midX)
                    {
                        leftList.Add(slist[i]);
                        if (slist[i].Y < minY.Y)
                            minY = slist[i];
                        if (slist[i].Y > maxY.Y)
                            maxY = slist[i];
                    }
                    else
                        rightList.Add(slist[i]);
                }
                
                //if all points were added to the left list
                if (rightList.Count <= 0)
                {
                    LList skipper = new LList();
                    skipper.LeftMost = new LLNode(minY);
                    skipper.RightMost = new LLNode(maxY);
                    skipper.LeftMost.Next = skipper.RightMost;
                    skipper.LeftMost.Prev = skipper.RightMost;
                    skipper.RightMost.Next = skipper.LeftMost;
                    skipper.RightMost.Prev = skipper.LeftMost;
                    if (skipper.LeftMost.Point == skipper.RightMost.Point)
                        skipper.Count = 1;
                    else
                        skipper.Count = 2;

                    return skipper;
                }

                LList leftLL = divAndCon(g, leftList);
                LList rightLL = divAndCon(g, rightList);

                return combine(g, leftLL, rightLL);
            }
            else
            {
                LList llist = new LList();
                LLNode one = new LLNode(slist[0]);
                one.Next = one;
                one.Prev = one;
                llist.LeftMost = one;
                llist.RightMost = one;
                llist.Count = 1;

                return llist;
            }
        }

        private LList combine(Graphics g, LList leftLL, LList rightLL)
        {
            //place holders for solution points
            //(to manipulate next/prev pointers after completed testing)
            LLNode left = leftLL.RightMost;
            LLNode right = rightLL.LeftMost;
            LLNode topLSol = left;
            LLNode topRSol = right;
            LLNode botLSol = left;
            LLNode botRSol = right;

            //Test top
            Boolean found = false;
            Boolean acceptRight;
            Boolean acceptLeft;
            while (!found)
            {
                acceptRight = false;
                acceptLeft = false;
                double m = (left.dY - right.dY) / (left.dX - right.dX);
                double b = (-m * left.dX) + left.dY;

                acceptRight = true;
                if (rightLL.Count > 1)
                {
                    LLNode testRight = right.Next;
                    if (m * testRight.dX + b - testRight.dY < 0)
                    {
                        right = testRight;
                        acceptRight = false;
                    }
                    else
                    {
                        if (rightLL.Count > 2)
                        {
                            testRight = right.Prev;
                            if (m * testRight.dX + b - testRight.dY < 0)
                            {
                                right = testRight;
                                acceptRight = false;
                            }

                        }
                    }
                }
                if (acceptRight)
                {
                    acceptLeft = true;
                    if (leftLL.Count > 1)
                    {
                        LLNode testLeft = left.Prev;
                        if (m * testLeft.dX + b - testLeft.dY < 0)
                        {
                            left = testLeft;
                            acceptLeft = false;
                        }
                        else
                        {
                            if (leftLL.Count > 2)
                            {
                                testLeft = left.Next;
                                if (m * testLeft.dX + b - testLeft.dY < 0)
                                {
                                    left = testLeft;
                                    acceptLeft = false;
                                }
                            }
                        }
                    }
                    if (acceptLeft)
                    {
                        topLSol = left;
                        topRSol = right;
                        found = true;
                    }
                }
            }

            //Test bottom
            found = false;
            left = leftLL.RightMost;
            right = rightLL.LeftMost;
            while (!found)
            {
                acceptRight = false;
                acceptLeft = false;
                double m = (left.dY - right.dY) / (left.dX - right.dX);
                double b = (-m * left.dX) + left.dY;

                acceptRight = true;
                if (rightLL.Count > 1)
                {
                    LLNode testRight = right.Prev;
                    if (m * testRight.dX + b - testRight.dY > 0)
                    {
                        right = testRight;
                        acceptRight = false;
                    }
                    else
                    {
                        if (rightLL.Count > 2)
                        {
                            testRight = right.Next;
                            if (m * testRight.dX + b - testRight.dY > 0)
                            {
                                right = testRight;
                                acceptRight = false;
                            }
                        }
                    }
                }

                if (acceptRight)
                {
                    acceptLeft = true;
                    if (leftLL.Count > 1)
                    {
                        LLNode testLeft = left.Next;
                        if (m * testLeft.dX + b - testLeft.dY > 0)
                        {
                            left = testLeft;
                            acceptLeft = false;
                        }
                        else
                        {
                            if (leftLL.Count > 2)
                            {
                                testLeft = left.Prev;
                                if (m * testLeft.dX + b - testLeft.dY > 0)
                                {
                                    left = testLeft;
                                    acceptLeft = false;
                                }
                            }
                        }
                    }
                    if (acceptLeft)
                    {
                        botLSol = left;
                        botRSol = right;
                        found = true;
                    }
                }
            }

            //Change pointers
            topLSol.Prev = topRSol;
            topRSol.Next = topLSol;
            botLSol.Next = botRSol;
            botRSol.Prev = botLSol;

            //update leftmost and rightmost pointers in a new LList
            LList nList = new LList();
            nList.LeftMost = topLSol;
            while (nList.LeftMost.Prev.dX < nList.LeftMost.dX)
                nList.LeftMost = nList.LeftMost.Prev;
            nList.RightMost = botRSol;
            while (nList.RightMost.Next.dX > nList.RightMost.dX)
                nList.RightMost = nList.RightMost.Next;
            nList.UpdateCount();

            return nList;
        }

        private void drawList(Graphics g, List<PointF> pointList)
        {
            Pen pen = new Pen(Color.Blue);
            if (pointList.Count > 1)
            {
                for (int i = 0; i < pointList.Count - 1; i++)
                {
                    g.DrawLine(pen, pointList[i], pointList[i + 1]);
                }
            }
        }
        private void drawLList(Graphics g, LList hull, Pen pen)
        {
            //Font font = new Font("Arial", 10);
            //Brush brush = new SolidBrush(Color.SteelBlue);
            //g.DrawString("" + hull.Count, font, brush, new PointF(0F, 0F));
            LLNode temp = hull.LeftMost;

            if (hull.Count > 1)
            {
                do
                {
                    //g.DrawEllipse(pen, temp.Point.X, temp.Point.Y, 5, 5);
                    g.DrawLine(pen, temp.Point, temp.Next.Point);
                    temp = temp.Next;
                }
                while (temp.Point != hull.LeftMost.Point);
            }
        }
    }
}
