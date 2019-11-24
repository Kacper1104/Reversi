using System;
using System.Collections.Generic;

namespace Reversi
{
    enum Colour { black = 'b', white = 'w', none =' ' }
    internal class Node
    {

        Node parent { get; set; }

        List<Node> children { get; set; }
        Colour[,] tokens { get; set; }
        Tuple<int, int>[] transition { get; set; }
        int phase { get; set; }
        int tokensPlaced { get; set; }
        Colour turn { get; set; }

        public Node()
        {
            tokens = new Colour[8,8];
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                    tokens[i, j] = Colour.none;
            }
            tokens[3, 3] = Colour.black;
            tokens[3, 4] = Colour.white;
            tokens[4, 3] = Colour.black;
            tokens[4, 4] = Colour.white;

            children = new List<Node>();
            phase = 0;
            tokensPlaced = 0;
            turn = Colour.black;
            transition = new Tuple<int, int>[2];
        }
        public Node(Node parent, Colour[,]tokens, Tuple<int,int>[] transition, int phase, int tokensPlaced, int turn)
        {
            this.parent = parent;
            this.tokens = tokens;
            this.transition = transition;
            this.phase = phase;
            this.tokensPlaced = tokensPlaced;

            if (turn == -1)
                this.turn = Colour.black;
            else
                this.turn = Colour.white;
            this.children = new List<Node>();
        }
        public void PopulateChildren()
        {
            Colour playerTurn;
            if (turn == Colour.black)
                playerTurn = Colour.white;
            else
                playerTurn = Colour.black;
            Colour[,] tempBoard = CloneTokens(tokens);
            List<Tuple<int,int>> tokensToKill = TokensToKill(playerTurn);

            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (tokens[i, j] == Colour.none)
                    {

                    }
                }
            }
        }//End of PopulateChildren()
        public Colour[,] CloneTokens(Colour[,] other)
        {
            Colour[,] newTokens = new Colour[8, 8];
            for(int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    newTokens[i, j] = other[i, j];
            }
            return newTokens;
        }//End of CloneTokens()
        public List< Tuple<int, int> > TokensToKill(Colour turn)
        {
            List < Tuple<int, int> > tokensToKill = new List<Tuple<int, int> >();
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if ((turn == Colour.black && tokens[i, j] == Colour.black) || (turn == Colour.white && tokens[i, j] == Colour.white))
                        tokensToKill.Add(new Tuple<int, int>(i, j));
                }
            }
            return tokensToKill;
        }//End of TokensToKill()
        public bool CheckBounds(int x, int y)
        {
            if (x >= 0 && x < 8 && y >= 0 && y < 8)
                return true;
            return false;
        }//Enc of CheckBounds()
        
        //  Sprawdz czy:
        //  - W rzędzie, po skosie, w kolumnie jest pion gracza
        //  - Pomiędzy pionem gracza a polem x,y znajdują się tylko piony przeciwnika
    }
}