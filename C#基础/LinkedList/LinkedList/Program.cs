using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYLinkedList
{
    class Program
    {
        static void Main(string[] args)
        {
            Node a = new Node(10);
            Node b = new Node(20);
            Node c = new Node(30);
            a.nextNode = b;
            b.nextNode = c;
            c.nextNode = null;

        }
        static void DelNextNode(Node n)
        {
            if (n.nextNode==null)
            {
                return ;
            }
            n.nextNode = n.nextNode.nextNode;
        }
        static void AddNode(Node n,int value)
        {
            Node new01 = new Node(value);
            new01.nextNode = n.nextNode;
            n.nextNode = new01;
        }
        static Node FindNode(Node n, int numble)
        {
            for (int i=1;i<=numble; i++)
            {
                n = n.nextNode;
            }
            return n;
        }
    }
    class Node
    {
        public int value;
        //public Node previousNode;
        public Node nextNode;
        public Node(int _value)
        {
            value = _value;
            nextNode = null;
        }
    }
}
