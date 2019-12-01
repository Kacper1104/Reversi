using System;
using System.Collections.Generic;

namespace Reversi
{
    enum Colour { black = 'b', white = 'w', none =' ' }
    enum Heuristic { stupid, smart, smartest }
    internal class Node
    {

        public Node parent { get; set; }
        public List<Node> children { get; set; }
        Colour[,] tokens { get; set; }
        public int[] move { get; set; }
        int moveValue { get; set; }
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
            moveValue = 0;
        }//End of Node()
        public Node(Node parent, Colour[,]tokens, int[]move, Colour turn, int moveValue)
        {
            this.parent = parent;
            this.tokens = tokens;
            this.move[0] = move[0];
            this.move[1] = move[1];
            this.turn = turn;
            this.children = new List<Node>();
            this.moveValue = moveValue;
        }//End of Node(Node, Colour[,], int[], Colour, int)
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
            int fieldCheck;
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (tokens[i, j] == Colour.none)
                    {
                        fieldCheck = CheckField(i, j, playerTurn);
                        if (fieldCheck != 0)
                        {
                            children.Add(new Node(this, tempBoard, new int[2] { i, j }, playerTurn, fieldCheck));
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
        public int CheckField(int x, int y, Colour player)
        {
            int points = 0;
            int sectionPoints = 0;

            bool playerTokenFound = false;
            bool enemyTokensFound = false;

            //check field
            if (tokens[x, y] != Colour.none)
                return 0;
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
                        sectionPoints = 0;
                        continue;
                    }
                    else
                    {
                        enemyTokensFound = true;
                        sectionPoints++;
                    }
                }
            }
            if (playerTokenFound && enemyTokensFound)
            {
                points += sectionPoints;
            }
            //down to up
            playerTokenFound = false;
            enemyTokensFound = false;
            sectionPoints = 0;
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
                        sectionPoints = 0;
                        continue;
                    }
                    else
                    {
                        enemyTokensFound = true;
                        sectionPoints++;
                    }
                }
            }
            if (playerTokenFound && enemyTokensFound)
            {
                points += sectionPoints;
            }

            //Check columns
            //left to right
            playerTokenFound = false;
            enemyTokensFound = false;
            sectionPoints = 0;
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
                        sectionPoints = 0;
                        continue;
                    }
                    else
                    {
                        enemyTokensFound = true;
                        sectionPoints++;
                    }
                }
            }
            if (playerTokenFound && enemyTokensFound)
            {
                points += sectionPoints;
            }
            //right to left
            playerTokenFound = false;
            enemyTokensFound = false;
            sectionPoints = 0;
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
                        sectionPoints = 0;
                        continue;
                    }
                    else
                    {
                        enemyTokensFound = true;
                        sectionPoints++;
                    }
                }
            }
            if (playerTokenFound && enemyTokensFound)
            {
                points += sectionPoints;
            }
            //Check diagonally
            int temp_x;
            int temp_y;
            int[] xy; 
            //left up to right down
            playerTokenFound = false;
            enemyTokensFound = false;
            sectionPoints = 0;
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
                        sectionPoints = 0;
                        continue;
                    }
                    else
                    {
                        enemyTokensFound = true;
                        sectionPoints++;
                    }
                }
                temp_x++;
                temp_y++;
            }
            if (playerTokenFound && enemyTokensFound)
            {
                points += sectionPoints;
            }
            //right up to left down
            playerTokenFound = false;
            enemyTokensFound = false;
            sectionPoints = 0;
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
                        sectionPoints = 0;
                        continue;
                    }
                    else
                    {
                        enemyTokensFound = true;
                        sectionPoints++;
                    }
                }
                temp_x++;
                temp_y--;
            }
            if (playerTokenFound && enemyTokensFound)
            {
                points += sectionPoints;
            }
            //left up to right up
            playerTokenFound = false;
            enemyTokensFound = false;
            sectionPoints = 0;
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
                        sectionPoints = 0;
                        continue;
                    }
                    else
                    {
                        enemyTokensFound = true;
                        sectionPoints++;
                    }
                }
                temp_x--;
                temp_y++;
            }
            if (playerTokenFound && enemyTokensFound)
            {
                points += sectionPoints;
            }
            //right down to left up
            playerTokenFound = false;
            enemyTokensFound = false;
            sectionPoints = 0;
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
                        sectionPoints = 0;
                        continue;
                    }
                    else
                    {
                        enemyTokensFound = true;
                        sectionPoints++;
                    }
                }
                temp_x--;
                temp_y--;
            }
            if (playerTokenFound && enemyTokensFound)
            {
                points += sectionPoints;
            }
            return points;

        }//End of CheckField()
        public int Evaluate(Heuristic heuristic)
        {
            if (heuristic == Heuristic.stupid)
            {
                return (new Random(100)).Next();
            }
            if (heuristic == Heuristic.smart)
            {
                return moveValue;
            }
            if (heuristic == Heuristic.smartest)
            {
                return TableBasedEvaluation(move[0], move[1]);
            }
            return 0;
        }//End of Evaluate()
        private int TableBasedEvaluation(int x, int y)
        {
            if ((x == 0 || x == 7) && 
                (y == 0 || y == 7))
            {
                return 100;
            }
            if((x == 0 || x == 7) && (y == 1 || y == 6) ||
               (y == 0 || y == 7) && (x == 1 || x == 6))
            {
                return -20;
            }
            if((x == 1 || x == 6) && 
               (y == 1 || y == 6))
            {
                return -50;
            }
            if((x == 0 || x == 7) && (y == 2 || y == 5) ||
               (y == 0 || y == 7) && (x == 2 || x == 5))
            {
                return 10;
            }
            if((x == 3 || x == 4) && (y == 0 || y == 7) ||
               (y == 3 || y == 4) && (x == 0 || x == 7))
            {
                return 5;
            }
            if((x == 1 || x == 6) && (y > 1 && x < 6) ||
               (y == 1 || y == 6) && (x > 1 && x < 6))
            {
                return -2;
            }
            if ((x > 1 && x < 6) && (y > 1 && y < 6))
                return -1;
            return 0;
        }//End of TableBasedEvaluation()
        private int[] TopRightXY(int x, int y)
        {
            var temp_x = x;
            var temp_y = y;
            while (temp_x > 0 && temp_y < 7)
            {
                temp_x--;
                temp_y++;
            }
            return new int[2] { temp_x, temp_y };
        }//End of TopRightXY()
        private int[] TopLeftXY(int x, int y)
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
        private int[] BotLeftXY(int x, int y)
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
        private int[] BotRightXY(int x, int y)
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
    }
}