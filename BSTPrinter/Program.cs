using BSTPrinter.BST;
using System;

namespace BSTPrinter
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new BinarySearchTree();

            tree.Insert(100);
            tree.Insert(10);
            tree.Insert(120);
            tree.Insert(2000);
            tree.Insert(2225);
            tree.Insert(2233);

            var preOrder = tree.GetPreOrderString();
            Console.WriteLine(preOrder);

            BSTPrinter.Print(preOrder);


        }

    }
}
