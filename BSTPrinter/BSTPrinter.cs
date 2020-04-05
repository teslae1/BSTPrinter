using System;
using System.Collections.Generic;
using System.Linq;


namespace BSTPrinter
{
    static class BSTPrinter
    {
        /// <summary>
        /// Prints BST 
        /// 
        /// </summary>
        /// <param name="binaryTreePreOrder">Should be pre order tree values as single spaced integers</param>
        static public void Print(string binaryTreePreOrder)
        {
            Console.WriteLine(GetPrintableString(binaryTreePreOrder));
        }
        /// <summary>
        /// Prints BST 
        /// 
        /// </summary>
        /// <param name="binaryTreePreOrder">Should be pre order tree values as single spaced integers</param>
        /// <param name="distance">Sets the distance between each node </param>
        static public void Print(string binaryTreePreOrder, int distance)
        {
            Console.WriteLine(GetPrintableString(binaryTreePreOrder, distance));
        }

        /// <summary>
        /// Returns a printable string of BST structure
        /// </summary>
        /// <param name="binaryTreePreOrder">String should be pre order tree values as single spaced integers</param>
        /// <param name="distance">Sets the distance between each node </param>
        /// <returns></returns>
        public static string GetPrintableString(string binaryTreePreOrder, int distance)
        {

            if (distance < 1)
                throw new FormatException("Invalid distance, must be atleast 1");
            else if (binaryTreePreOrder.Length == 0 || binaryTreePreOrder == " ")
                throw new FormatException("Invalid format of input string");

            var points = GetPoints(binaryTreePreOrder, distance);

            //get dimensions of grid
            int height = points.Max(p => p.Y) + 1;
            int smallestX = points.Min(p => p.SmallestX);
            int biggestX = (points.Max(p => p.BiggestX));
            int width = ((smallestX * -1) + biggestX) + 50;


            //init grid with spaces 
            var grid = new string[height, width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    grid[i, j] = " ";

            //fill with data
            foreach (BSTPrinterPoint p in points)
            {
                int realX = p.X + (smallestX * -1);
                if (p.IsComposite)
                {
                    int offset = 0;
                    bool didOffset = false;
                    foreach (var subPoint in p.Points)
                    {
                        if (grid[subPoint.Y, subPoint.X + (smallestX * -1)] != " " && !didOffset)
                        {
                            didOffset = true;
                            while (grid[subPoint.Y, subPoint.X + (smallestX * -1) + offset] != " ")
                                offset++;
                            grid[subPoint.Y, subPoint.X + (smallestX * -1) + offset] = "x";
                            offset++;
                        }
                        grid[subPoint.Y, subPoint.X + (smallestX * -1) + offset] = subPoint.Data;
                    }
                }
                else
                {
                    if (grid[p.Y, realX] != " ")
                    {
                        grid[p.Y, realX + 1] = "x";
                        grid[p.Y, realX + 2] = p.Data;
                    }
                    else
                        grid[p.Y, realX] = p.Data;
                }
            }


            string printable = "";
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                    printable += grid[i, j];
                printable += "\n";
            }

            return printable;
        }
        /// <summary>
        /// >Returns a printable string of BST structure
        /// </summary>
        /// <param name="binaryTreePreOrder">Should be pre order tree values as single spaced integers</param>
        /// <returns></returns>
        public static string GetPrintableString(string binaryTreePreOrder)
        {
            return GetPrintableString(binaryTreePreOrder, 1);
        }



        static void RemoveNotNumbers(ref string[] arr)
        {
            var filtered = new List<string>();
            int n;
            foreach (var s in arr)
                if (int.TryParse(s, out n))
                    filtered.Add(s);

            arr = filtered.ToArray();
        }
        static List<BSTPrinterPoint> GetPoints(string binaryTreePreOrder, int slashes)
        {
            string[] arr = binaryTreePreOrder.Split(' ');
            RemoveNotNumbers(ref arr);

            var tree = new BSTPrinterTree(slashes);
            foreach (var s in arr)
                tree.Insert(s);

            return tree.GetPoints();
        }
        class BSTPrinterTree
        {
            int slashes;
            public BSTPrinterTree(int slashes)
            {
                this.slashes = slashes;
            }
            BSTPrinterNode root;
            public void Insert(string value)
            {
                var nodeToInsert = new BSTPrinterNode(Convert.ToInt32(value));
                if (root == null)
                {
                    root = nodeToInsert;
                    var point = new BSTPrinterPoint(0, 0, value);
                    nodeToInsert.Point = point;
                    points = new List<BSTPrinterPoint>() { point };
                }
                else
                    root.Insert(nodeToInsert,
                        nodeToInsert.Data < root.Data ? root.Point.SmallestX : root.Point.BiggestX,
                        0, ref points, slashes);
            }

            List<BSTPrinterPoint> points;
            public List<BSTPrinterPoint> GetPoints()
            {
                root = null;
                return points;
            }
        }
        class BSTPrinterNode
        {
            public BSTPrinterNode LeftChild;
            public BSTPrinterNode RightChild;

            public BSTPrinterPoint Point;

            public int Data;

            public BSTPrinterNode(int data)
            {
                this.Data = data;
            }

            public void Insert(BSTPrinterNode nodeToInsert, int x, int y, ref List<BSTPrinterPoint> points, int slashes)
            {

                if (nodeToInsert.Data < Data)
                {
                    //go left
                    if (LeftChild == null)
                    {
                        LeftChild = nodeToInsert;

                        for (int i = 1; i <= slashes; i++)
                            points.Add(new BSTPrinterPoint(x - i, y + i, "/"));

                        var point = new BSTPrinterPoint((x - slashes) - nodeToInsert.Data.ToString().Length,
                            y + (slashes + 1),
                            nodeToInsert.Data.ToString());

                        points.Add(point);
                        LeftChild.Point = point;
                    }
                    else
                        LeftChild.Insert(nodeToInsert,
                            LeftChild.Data > nodeToInsert.Data ? LeftChild.Point.SmallestX : LeftChild.Point.BiggestX,
                            y + (slashes + 1),
                            ref points, slashes);
                }
                else if (nodeToInsert.Data > Data)
                {
                    //go right
                    if (RightChild == null)
                    {
                        RightChild = nodeToInsert;

                        for (int i = 1; i <= slashes; i++)
                            points.Add(new BSTPrinterPoint(x + i, y + i, "\\"));

                        var point = new BSTPrinterPoint(
                            x + (slashes + 1),
                            y + (slashes + 1),
                            nodeToInsert.Data.ToString());

                        points.Add(point);
                        RightChild.Point = point;
                    }
                    else
                        RightChild.Insert(nodeToInsert,
                            RightChild.Data > nodeToInsert.Data ? RightChild.Point.SmallestX : RightChild.Point.BiggestX,
                            y + (slashes + 1),
                            ref points, slashes);
                }
            }
        }
        public class BSTPrinterPoint
        {
            public int X;
            public int Y;
            public string Data;


            public int SmallestX
            {
                get
                {
                    if (IsComposite)
                        return Points.Min(p => p.X);
                    else
                        return X;
                }
            }
            public int BiggestX
            {
                get
                {
                    if (IsComposite)
                        return Points.Max(p => p.X);
                    else
                        return X;
                }
            }
            public bool IsComposite { get { return Data == null; } }
            public List<BSTPrinterPoint> Points = new List<BSTPrinterPoint>();

            /// <summary>
            /// constructor auto creates composite if needed
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="data"></param>
            public BSTPrinterPoint(int x, int y, string data)
            {

                Y = y;
                if (data.Length > 1)
                    for (int i = 0; i < data.Length; i++)
                        Points.Add(new BSTPrinterPoint(x + i, y, data[i].ToString()));
                else
                {
                    X = x;
                    Data = data;
                }
            }
        }
    }
}
