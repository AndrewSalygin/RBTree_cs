using System;

namespace RBTree {
    class Program
    {
        static void Main(string[] args)
        {
            RBTree tree = new RBTree();
            tree.Insert(457);
            tree.Insert(176);
            tree.Insert(706);
            tree.Insert(16);
            tree.Insert(372);
            tree.Insert(642);
            tree.Insert(768);
            tree.Insert(216);
            tree.Insert(455);
            tree.Insert(675);
            tree.Insert(680);
            tree.Insert(681);
            tree.Delete(706);
            tree.Delete(681);
            Console.ReadLine();
        }
    }
}