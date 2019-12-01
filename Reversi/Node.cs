using System;
using System.Collections.Generic;

namespace Reversi
{
    enum Colour { black = 'b', white = 'w', none =' ' }
    enum Heuristic { stupid, smart, smartest }
    internal class Node
    {

        Node parent { get; set; }

        List<Node> children { get; set; }
        Colour[,] tokens { get; set; }
        int[] move { get; set; }
        Colour turn { get; set; }
        //Constructors
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
            turn = Colour.black;
            move = new int[2];
        }//End of Node()
        public Node(Node parent, Colour[,]tokens, int[]move, Colour turn)
        {
            this.parent = parent;
            this.tokens = tokens;
            this.move[0] = move[0];
            this.move[1] = move[1];
            this.turn = turn;
            this.children = new List<Node>();
        }//End of Node(Node, Colour[,], int[], Colour)
        //Functions
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
                        if(CheckField(i, j, playerTurn))
                        {
                            children.Add(new Node(this, tempBoard, new int[2] { i, j }, playerTurn));
                        }
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
        public bool CheckField(int x, int y, Colour player)
        {
            bool playerTokenFound = false;
            bool enemyTokensFound = false;

            //check field
            if (tokens[x, y] != Colour.none)
                return false;
            //check rows
            //Up to down
            for(int i = 0; i < x; i++)
            {
                if (!playerTokenFound)
                {
                    if(tokens[i, y] == player)
                    {
                        playerTokenFound = true;
                        continue;
                    }
                }
                else
                {
                    if (tokens[i, y] == player || tokens[i, y] == Colour.none)
                    {
                        enemyTokensFound = false;
                        playerTokenFound = false;
                        continue;
                    }
                    else
                    {
                        enemyTokensFound = true;
                    }
                }
            }
            if (playerTokenFound && enemyTokensFound)
                return true;
            //down to up
            playerTokenFound = false;
            enemyTokensFound = false;
            for (int i = 7; i > x; i--)
            {
                if (!playerTokenFound)
                {
                    if (tokens[i, y] == player)
                    {
                        playerTokenFound = true;
                        continue;
                    }
                }
                else
                {
                    if (tokens[i, y] == player || tokens[i, y] == Colour.none)
                    {
                        enemyTokensFound = false;
                        playerTokenFound = false;
                        continue;
                    }
                    else
                    {
                        enemyTokensFound = true;
                    }
                }
            }
            if (playerTokenFound && enemyTokensFound)
                return true;

            //Check columns
            //left to right
            playerTokenFound = false;
            enemyTokensFound = false;
            for (int i = 0; i < y; i++)
            {
                if (!playerTokenFound)
                {
                    if (tokens[x, i] == player)
                    {
                        playerTokenFound = true;
                        continue;
                    }
                }
                else
                {
                    if (tokens[x, i] == player || tokens[x, i] == Colour.none)
                    {
                        enemyTokensFound = false;
                        playerTokenFound = false;
                        continue;
                    }
                    else
                    {
                        enemyTokensFound = true;
                    }
                }
            }
            if (playerTokenFound && enemyTokensFound)
                return true;
            //right to left
            playerTokenFound = false;
            enemyTokensFound = false;
            for (int i = 7; i > y; i--)
            {
                if (!playerTokenFound)
                {
                    if (tokens[x, i] == player)
                    {
                        playerTokenFound = true;
                        continue;
                    }
                }
                else
                {
                    if (tokens[x, i] == player || tokens[x, i] == Colour.none)
                    {
                        enemyTokensFound = false;
                        playerTokenFound = false;
                        continue;
                    }
                    else
                    {
                        enemyTokensFound = true;
                    }
                }
            }
            if (playerTokenFound && enemyTokensFound)
                return true;
            int temp_x;
            int temp_y;
            int[] xy;
            //Check diagonally 
            //left up to right down
            playerTokenFound = false;
            enemyTokensFound = false;
            xy = TopLeftXY(x, y);
            temp_x = xy[0];
            temp_y = xy[1];
            while (temp_x < 8 && temp_y < 8)
            {
                if (!playerTokenFound)
                {
                    if (tokens[temp_x, temp_y] == player)
                    {
                        playerTokenFound = true;
                        continue;
                    }
                }
                else
                {
                    if (tokens[temp_x, temp_y] == player || tokens[temp_x, temp_y] == Colour.none)
                    {
                        enemyTokensFound = false;
                        playerTokenFound = false;
                        continue;
                    }
                    else
                    {
                        enemyTokensFound = true;
                    }
                }
                temp_x++;
                temp_y++;
            }
            if (playerTokenFound && enemyTokensFound)
                return true;
            //right up to left down
            playerTokenFound = false;
            enemyTokensFound = false;
            xy = TopRightXY(x, y);
            temp_x = xy[0];
            temp_y = xy[1];
            while (temp_x < 8 && temp_y >= 0)
            {
                if (!playerTokenFound)
                {
                    if (tokens[temp_x, temp_y] == player)
                    {
                        playerTokenFound = true;
                        continue;
                    }
                }
                else
                {
                    if (tokens[temp_x, temp_y] == player || tokens[temp_x, temp_y] == Colour.none)
                    {
                        enemyTokensFound = false;
                        playerTokenFound = false;
                        continue;
                    }
                    else
                    {
                        enemyTokensFound = true;
                    }
                }
                temp_x++;
                temp_y--;
            }
            if (playerTokenFound && enemyTokensFound)
                return true;
            //left up to right up
            playerTokenFound = false;
            enemyTokensFound = false;
            xy = BotLeftXY(x, y);
            temp_x = xy[0];
            temp_y = xy[1];
            while (temp_x >= 0 && temp_y < 8)
            {
                if (!playerTokenFound)
                {
                    if (tokens[temp_x, temp_y] == player)
                    {
                        playerTokenFound = true;
                        continue;
                    }
                }
                else
                {
                    if (tokens[temp_x, temp_y] == player || tokens[temp_x, temp_y] == Colour.none)
                    {
                        enemyTokensFound = false;
                        playerTokenFound = false;
                        continue;
                    }
                    else
                    {
                        enemyTokensFound = true;
                    }
                }
                temp_x--;
                temp_y++;
            }
            if (playerTokenFound && enemyTokensFound)
                return true;
            //right down to left up
            playerTokenFound = false;
            enemyTokensFound = false;
            xy = BotRightXY(x, y);
            temp_x = xy[0];
            temp_y = xy[1];
            while (temp_x >= 0 && temp_y >= 0)
            {
                if (!playerTokenFound)
                {
                    if (tokens[temp_x, temp_y] == player)
                    {
                        playerTokenFound = true;
                        continue;
                    }
                }
                else
                {
                    if (tokens[temp_x, temp_y] == player || tokens[temp_x, temp_y] == Colour.none)
                    {
                        enemyTokensFound = false;
                        playerTokenFound = false;
                        continue;
                    }
                    else
                    {
                        enemyTokensFound = true;
                    }
                }
                temp_x--;
                temp_y--;
            }
            if (playerTokenFound && enemyTokensFound)
                return true;
            return false;
        }//End of CheckField()
        int[] TopRightXY(int x, int y)
        {
            var temp_x = x;
            var temp_y = y;
            while(temp_x > 0 && temp_y < 7)
            {
                temp_x--;
                temp_y++;
            }
            return new int[2] { temp_x, temp_y };
        }//End of TopRightXY()
        int[] TopLeftXY(int x, int y)
        {
            var temp_x = x;
            var temp_y = y;
            while (temp_x > 0 && temp_y > 0)
            {
                temp_x--;
                temp_y--;
            }
            return new int[2] { temp_x, temp_y };
        }//End of TopLeftXY()
        int[] BotLeftXY(int x, int y)
        {
            var temp_x = x;
            var temp_y = y;
            while (temp_x < 7 && temp_y > 0)
            {
                temp_x++;
                temp_y--;
            }
            return new int[2] { temp_x, temp_y };
        }//End of BotLeftXY()
        int[] BotRightXY(int x, int y)
        {
            var temp_x = x;
            var temp_y = y;
            while (temp_x < 7 && temp_y < 7)
            {
                temp_x++;
                temp_y++;
            }
            return new int[2] { temp_x, temp_y };
        }//End of BotRightXY()
        public int Evaluate(Heuristic heuristic)
        {

            return 0;
        }//End of Evaluate()
        int TokenValue(int x, int y, int currentValue)
        {
            if(x+1 < 8 && y + 1 < 8)
            return 0;
        }
    }
}