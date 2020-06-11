using BSTPrinter.BST;
using System;

namespace BSTPrinter
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new BinarySearchTree();

            tree.Insert(10);
            tree.Insert(15);
            tree.Insert(5);
            tree.Insert(6);
            tree.Insert(2000);
            tree.Insert(1000);

            var preOrder = tree.GetPreOrderString();
            Console.WriteLine(preOrder);

            BSTPrinter.Print(preOrder);
        }
    }
}
