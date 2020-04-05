using System;
using System.Collections.Generic;
using System.Text;

namespace BSTPrinter.BST
{
    class BinarySearchTree
    {
        Node root;

        public void Insert(int n)
        {
            var nodeToInsert = new Node(n);

            if (root == null)
                root = nodeToInsert;
            else
                root.Insert(nodeToInsert);
        }
        public void Remove(int n)
        {
            //expect that it is in tree

            //locate

            var current = root;
            Node parent = null;
            while (current.Data != n)
            {
                parent = current;
                if (n < current.Data)
                    current = current.LeftChild;
                else if (n > current.Data)
                    current = current.RightChild;
            }

            //if leaf
            if (current.RightChild == null && current.LeftChild == null)
            {
                //if root
                if (parent == null)
                    root = null;
                else if (current == parent.RightChild)
                    parent.RightChild = null;
                else
                    parent.LeftChild = null;
            }

            //if has single child
            else if (current.RightChild != null && current.LeftChild == null ||
                current.RightChild == null && current.LeftChild != null)
            {
                //if root
                if (parent == null)
                {
                    if (current.RightChild != null)
                        root = current.RightChild;
                    else
                        root = current.LeftChild;
                }

                //parent pointer set to single child
                if (current == parent.RightChild)
                {
                    if (current.RightChild != null)
                        parent.RightChild = current.RightChild;
                    else
                        parent.RightChild = current.LeftChild;
                }
                else if (current == parent.LeftChild)
                {
                    if (current.RightChild != null)
                        parent.LeftChild = current.RightChild;
                    else
                        parent.LeftChild = current.LeftChild;
                }
            }

            //has two children
            else
            {
                //locate biggest value in left subtree
                var leftBiggest = current.LeftChild;
                while (leftBiggest.RightChild != null)
                    leftBiggest = leftBiggest.RightChild;

                //delete that node
                Remove(leftBiggest.Data);

                //copy data into current
                current.Data = leftBiggest.Data;
            }
        }

        public void printInOrder()
        {
            root.TraverseInOrder();
        }

        public string GetPreOrderString()
        {
            var preOrder = "";
            root.TraversePreOrder(ref preOrder);

            return preOrder;
        }
    }
}
