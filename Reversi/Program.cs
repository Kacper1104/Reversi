using System;

namespace Reversi
{
    class Program
    {
        static void Main(string[] args)
        {
            DecisionTree ABTree = new DecisionTree(3);
            ABTree.Initialize();
        }
    }
}
