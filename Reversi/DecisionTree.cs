using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reversi
{
    class DecisionTree
    {
        Tree tree { get; set; }
        int searchDepth { get; set; }
        List<Node> leafs { get; set; }
        Node currentNode { get; set; }
        int[] move { get; set; }
        int levelBuilt { get; set; }

        public DecisionTree(int searchDepth)
        {
            this.searchDepth = searchDepth;
            leafs = new List<Node>();
            levelBuilt = 0;
        }

        public void Initialize()
        {
            //create tree for all the fields, with no transition as it's the root
            tree = new Tree(new Node());
            leafs.Add(tree.root);
            currentNode = tree.root;
            InitialTreeBuild();
        }//End of Initialize()
        private void InitialTreeBuild()
        {
            for (int i = 0; i < searchDepth - 1; i++)
            {
                BuildLevel();
            }
        }//End of InitialTreeBuild()
        public void BuildLevel()
        {
            if (leafs.Count == 0)
                Console.WriteLine("0 leafs");
            for (int i = 0; i < leafs.Count; i++)
            {
                leafs[i].PopulateChildren();
            }
            List<Node> previousLeafs = new List<Node>(leafs);
            leafs.Clear();
            for (int i = 0; i < previousLeafs.Count; i++)
            {
                for (int j = 0; j < previousLeafs[i].children.Count; j++)
                    leafs.Add(previousLeafs[i].children[j]);
            }
            levelBuilt++;
        }//End of BuildTree()
        public Node getCurrentNode()
        {
            return currentNode;
        }
        public void SetSearchDepth(int depht)
        {
            searchDepth = depht;
        }
        public int GetSearchDepth()
        {
            return searchDepth;
        }
        public void MakePlayerMove()
        {
            //zaktualizowac drzewo na podstawie wykonanego ruchu
            UpdateTree(currentNode.move);
        }
        public int[] ReturnMoveToMake(Heuristic heuristic, bool alfaBeta)
        {

            if (alfaBeta)
                currentNode = MinMaxAlphaBeta(currentNode, heuristic);
            else
                currentNode = MinMax(currentNode, heuristic);

            UpdateLeafs();
            CleanMemory();

            BuildLevel();
            move = new int[2] { currentNode.move[0], currentNode.move[1] };
            return move;
        }
        private Node MinMax(Node currentNode, Heuristic heuristic)
        {
            return MinMax(currentNode, true, heuristic);
        }

        private Node MinMax(Node currentNode, bool isMaxNode, Heuristic heuristic)
        {
            if (currentNode.children.Count == 0)
            {
                return currentNode;
            }
            else if (isMaxNode)
            {
                return currentNode.children.OrderByDescending(i => MinMax(i, false, heuristic).Evaluate(heuristic)).First();
            }
            else
            {
                return currentNode.children.OrderBy(i => MinMax(i, true, heuristic).Evaluate(heuristic)).First();
            }
        }

        //wywolanie alfabeta
        private Node MinMaxAlphaBeta(Node currentNode, Heuristic heuristic)
        {
            return MinMaxAlphaBeta(currentNode, Int32.MinValue, Int32.MaxValue, true, heuristic, searchDepth);
        }

        //alfabeta
        private Node MinMaxAlphaBeta(Node currentNode, int alpha, int beta, bool isMaxNode, Heuristic heuristic, int depth)
        {
            if (currentNode.children.Count == 0 || depth == 0)
            {
                return currentNode;
            }
            else if (isMaxNode)
            {
                Node maxNode = currentNode.children[0];
                alpha = maxNode.Evaluate(heuristic);
                for (int i = 0; i < currentNode.children.Count; i++)
                {
                    int alphaChild = MinMaxAlphaBeta(currentNode.children[i], alpha, beta, false, heuristic, depth - 1).Evaluate(heuristic);
                    if (alpha < alphaChild)
                    {
                        alpha = alphaChild;
                        maxNode = currentNode.children[i];
                    }
                    if (beta <= alpha)
                        return maxNode;
                }
                return maxNode;
            }
            else
            {
                Node minNode = currentNode.children[0];
                beta = minNode.Evaluate(heuristic);
                for (int i = 0; i < currentNode.children.Count; i++)
                {
                    int betaChild = MinMaxAlphaBeta(currentNode.children[i], alpha, beta, true, heuristic, depth - 1).Evaluate(heuristic);
                    if (beta > betaChild)
                    {
                        beta = betaChild;
                        minNode = currentNode.children[i];
                    }
                    if (beta <= alpha)
                        return minNode;
                }
                return minNode;
            }
        }
        //find player move and execute on tree
        public void UpdateTree(int[] move)
        {
            if (tree != null)
            {
                currentNode = currentNode.children.Where(item => item.move[0] == move[0] && item.move[1] == move[1] && item.move[2] == move[2]).First();
                UpdateLeafs();
                CleanMemory();
                BuildLevel();
            }
        }

        public void UpdateLeafs()
        {
            leafs.Clear();
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(currentNode);

            while (queue.Count != 0)
            {
                Node node = queue.Dequeue();
                if (node.children.Count == 0)
                {
                    leafs.Add(node);
                }
                else
                {
                    for (int i = 0; i < node.children.Count; i++)
                    {
                        queue.Enqueue(node.children[i]);

                    }
                }
            }
        }

        public void CleanMemory()
        {
            if (currentNode.parent != null)
            {
                currentNode.parent.children.Clear();
                currentNode.parent.children.Add(currentNode);

                if (currentNode.parent.parent != null)
                {
                    currentNode.parent.parent.children.Clear();
                }
            }
        }
    }
}
