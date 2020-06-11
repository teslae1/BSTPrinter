using BSTPrinter.BST;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;

namespace BSTPrinter
{
    static class BSTPrinter
    {
        private static int distanceBetweenNodes;

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
        /// <param name="distanceBetweenNodes">Sets the distance between each node </param>
        static public void Print(string binaryTreePreOrder, int distanceBetweenNodes)
        {
            Console.WriteLine(GetPrintableString(binaryTreePreOrder, distanceBetweenNodes));
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

        /// <summary>
        /// Returns a printable string of BST structure
        /// </summary>
        /// <param name="binaryTreePreOrder">String should be pre order tree values as single spaced integers</param>
        /// <param name="distanceBetweenNodes">Sets the distance between each node </param>
        /// <returns></returns>
        public static string GetPrintableString(string binaryTreePreOrder, int distanceBetweenNodes)
        {
            if (InputValidated(binaryTreePreOrder))
            {
                SetDistanceBetweenNodes(distanceBetweenNodes);
                var graphicalPoints = GetGraphicalPoints(binaryTreePreOrder);
                var gridGenerator = new GraphicalGridGenerator(graphicalPoints);
                return gridGenerator.GetPrintable();
            }

            return null;
        }

        static bool InputValidated(string binaryTreePreOrder)
        {
            if (distanceBetweenNodes < 1)
                throw new FormatException("Invalid distance, must be atleast 1");
            if (binaryTreePreOrder.Length == 0 ||
                binaryTreePreOrder == " " ||
                ToNumbers(binaryTreePreOrder.Split(' ')).Count < 1)
                throw new FormatException("Invalid format of pre-order values");

            return true;
        }
        
        private static void SetDistanceBetweenNodes(int distance)
        {
            distanceBetweenNodes = distance;
        }

        static List<GraphicalPoint> GetGraphicalPoints(string binaryTreePreOrder)
        {
            string[] arr = binaryTreePreOrder.Split(' ');
            var nodeValues = ToNumbers(arr);
            var pointGetter = new GraphicalPointGetter();

            return pointGetter.GetPoints(nodeValues);
        }

        static List<int> ToNumbers(string[] arr)
        {
            var numbers = new List<int>();
            int numb;
            foreach (string s in arr)
                if (int.TryParse(s, out numb))
                    numbers.Add(numb);

            return numbers;
        }

        private class GraphicalPointGetter
        {

            static List<GraphicalPoint> points = new List<GraphicalPoint>();

            Node root;

            public List<GraphicalPoint> GetPoints(List<int> nodeValues)
            {
                points.Clear();
                foreach (int value in nodeValues)
                    Insert(value);

                root = null;
                return points;
            }

            public void Insert(int value)
            {
                var nodeToInsert = new Node(value);
                if (root == null)
                    SetupRoot(nodeToInsert);
                else
                {
                    var parentCoordinate = new Coordinate(
                        nodeToInsert.Data < root.Data ? root.Point.SmallestX : root.Point.BiggestX,
                        0);
                    root.Insert(nodeToInsert, parentCoordinate);
                }
            }

            void SetupRoot(Node root)
            {
                this.root = root;
                var point = new GraphicalPoint(new Coordinate(0, 0), root.Data.ToString());
                points.Add(point);
                root.Point = point;
            }

            private class Node
            {
                public Node LeftChild;
                public Node RightChild;

                public GraphicalPoint Point;

                public int Data;

                public Node(int data)
                {
                    this.Data = data;
                }

                public void Insert(Node nodeToInsert, Coordinate parentNodeCoordinate)
                {

                    if (nodeToInsert.Data < Data)
                    {
                        //go left
                        if (LeftChild == null)
                            InsertAsLeftChild(nodeToInsert, parentNodeCoordinate);
                        else
                            TraverseLeft(nodeToInsert, parentNodeCoordinate);
                    }
                    else if (nodeToInsert.Data > Data)
                    {
                        //go right
                        if (RightChild == null)
                            InsertAsRightChild(nodeToInsert, parentNodeCoordinate);
                        else
                            TraverseRight(nodeToInsert, parentNodeCoordinate);
                    }
                }

                void TraverseLeft(Node nodeToInsert, Coordinate parentNodeCoordinate)
                {
                    var leftSideCoordinate = new Coordinate(
                                LeftChild.Data > nodeToInsert.Data ? LeftChild.Point.SmallestX : LeftChild.Point.BiggestX,
                                parentNodeCoordinate.Y + (distanceBetweenNodes + 1));
                    LeftChild.Insert(nodeToInsert, leftSideCoordinate);
                }

                void TraverseRight(Node nodeToInsert, Coordinate parentNodeCoordinate)
                {
                    var rightSideCoordinate = new Coordinate(
                               RightChild.Data > nodeToInsert.Data ? RightChild.Point.SmallestX : RightChild.Point.BiggestX,
                               parentNodeCoordinate.Y + (distanceBetweenNodes + 1));
                    RightChild.Insert(nodeToInsert, rightSideCoordinate);
                }

                void InsertAsRightChild(Node node, Coordinate parentNodeCoordinate)
                {
                    RightChild = node;
                    AddSlashesToRight(parentNodeCoordinate);
                    AddNodePointRight(node, parentNodeCoordinate);
                }
                void AddSlashesToRight(Coordinate parentNodeCoordinate)
                {
                    for (int i = 1; i <= distanceBetweenNodes; i++)
                    {
                        var coordinate = new Coordinate(parentNodeCoordinate.X + i, parentNodeCoordinate.Y + i);
                        points.Add(new GraphicalPoint(coordinate, "\\"));
                    }
                }
                void AddNodePointRight(Node node, Coordinate parentNodeCoordinate)
                {
                    var coordinate = new Coordinate(
                        parentNodeCoordinate.X + (distanceBetweenNodes + 1),
                        parentNodeCoordinate.Y + (distanceBetweenNodes + 1));
                    var point = new GraphicalPoint(coordinate, node.Data.ToString());
                    points.Add(point);
                    RightChild.Point = point;
                }

                void InsertAsLeftChild(Node node, Coordinate parentNodeCoordinate)
                {
                    LeftChild = node;
                    AddSlashesToLeft(parentNodeCoordinate);
                    AddNodePointLeft(node, parentNodeCoordinate);
                }
                void AddNodePointLeft(Node node, Coordinate parentNodeCoordinate)
                {
                    var nodePoint = new GraphicalPoint(
                        new Coordinate((parentNodeCoordinate.X - distanceBetweenNodes) - node.Data.ToString().Length,
                        parentNodeCoordinate.Y + (distanceBetweenNodes + 1)),
                        node.Data.ToString());

                    points.Add(nodePoint);
                    LeftChild.Point = nodePoint;
                }
                void AddSlashesToLeft(Coordinate parentNodeCoordinate)
                {
                    for (int i = 1; i <= distanceBetweenNodes; i++)
                        points.Add(new GraphicalPoint(
                            new Coordinate(parentNodeCoordinate.X - i, parentNodeCoordinate.Y + i),
                             "/"));
                }
            }

        }

        private class Coordinate
        {
            public int X;
            public int Y;
            public Coordinate(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        private class GraphicalPoint
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
            public List<GraphicalPoint> Points = new List<GraphicalPoint>();

            /// <summary>
            /// constructor auto creates composite if needed
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="data"></param>
            public GraphicalPoint(Coordinate coordinate, string data)
            {

                Y = coordinate.Y;
                if (data.Length > 1)
                    for (int i = 0; i < data.Length; i++)
                        Points.Add(new GraphicalPoint(
                            new Coordinate(coordinate.X + i, coordinate.Y)
                            , data[i].ToString()));
                else
                {
                    X = coordinate.X;
                    Data = data;
                }
            }
        }

        private class GraphicalGridGenerator
        {
            List<GraphicalPoint> points;
            int smallestX;
            int biggestX;

            public GraphicalGridGenerator(List<GraphicalPoint> points)
            {
                this.points = points;
                smallestX = points.Min(p => p.SmallestX);
                biggestX = points.Max(p => p.BiggestX);
            }

            int xSpacer = 50;
            public string GetPrintable()
            {
                var grid = GetEmptyGrid();

                foreach (GraphicalPoint p in points)
                {

                    if (p.IsComposite)
                        AddCompositeGraphicsToGrid(p, grid);
                    else
                        AddPointGraphicsToGrid(p, grid);
                }

                return ArrayToString(grid);
            }

            void AddCompositeGraphicsToGrid(GraphicalPoint point, string[,] grid)
            {
                int offset = 0;
                bool didOffset = false;
                foreach (var subPoint in point.Points)
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

            void AddPointGraphicsToGrid(GraphicalPoint point, string[,] grid)
            {
                int realX = point.X + (smallestX * -1);

                if (grid[point.Y, realX] != " ")
                {
                    grid[point.Y, realX + 1] = "x";
                    grid[point.Y, realX + 2] = point.Data;
                }
                else
                    grid[point.Y, realX] = point.Data;
            }


            string ArrayToString(string[,] grid)
            {
                string printable = "";
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                        printable += grid[i, j];
                    printable += "\n";
                }

                return printable;
            }

            string[,] GetEmptyGrid()
            {
                int height = points.Max(p => p.Y) + 1;
                int width = ((smallestX * -1) + biggestX) + xSpacer;

                var grid = new string[height, width];
                for (int i = 0; i < height; i++)
                    for (int j = 0; j < width; j++)
                        grid[i, j] = " ";

                return grid;
            }
        }
    }
}
