using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi
{
    class DecisionTree
    {
        Tree tree { get; set; }
        int searchDepth { get; set; }
        List<Node> leafs { get; set; }
        Node currentNode { get; set; }
        int[,] transition { get; set; }
        int levelBuilt { get; set; }

        public DecisionTree(int searchDepth)
        {
            this.searchDepth = searchDepth;
            leafs = new List<Node>();
            levelBuilt = 0;
        }

        void Initialize()
        {
            //create tree for all the fields, with no transition as it's the root.
            Field[] board;
            for(int i = 0; i < 8; i++)
            tree = new Tree(new Node(board));
            leafs.Add(tree.root);
            currentNode = tree.root;
            InitialTreeBuild();
        }

    }
}
