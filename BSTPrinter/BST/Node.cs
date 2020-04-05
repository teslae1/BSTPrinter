using System;
using System.Collections.Generic;
using System.Text;

namespace BSTPrinter.BST
{
    class Node
    {
        public Node LeftChild;
        public Node RightChild;

        public int Data;
        public Node(int data)
        {
            Data = data;
        }

        public void Insert(Node nodeToInsert)
        {
            if (nodeToInsert.Data < Data)
            {
                if (LeftChild == null)
                    LeftChild = nodeToInsert;
                else
                    LeftChild.Insert(nodeToInsert);
            }
            else if (nodeToInsert.Data > Data)
            {
                if (RightChild == null)
                    RightChild = nodeToInsert;
                else
                    RightChild.Insert(nodeToInsert);
            }
        }


        public void TraversePreOrder(ref string preOrder)
        {
            preOrder += (Data + " ");

            if (LeftChild != null)
                LeftChild.TraversePreOrder(ref preOrder);

            if (RightChild != null)
                RightChild.TraversePreOrder(ref preOrder);
        }
        public void TraverseInOrder()
        {
            if (LeftChild != null)
                LeftChild.TraverseInOrder();

            Console.Write(Data + " ");

            if (RightChild != null)
                RightChild.TraverseInOrder();
        }
    }
}
