using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SimpleCheckers
{
    /// <summary>
    /// Implementeaza algoritmul de cautare a mutarii optime
    /// </summary>
    public partial class Minimax
    {
        private static Random _rand = new Random();

        public static int _depth = 4;



        // ------------------

        /// <summary>
        /// Primeste o configuratie ca parametru, cauta mutarea optima si returneaza configuratia
        /// care rezulta prin aplicarea acestei mutari optime
        /// </summary>
        public static (Board, Move) FindNextBoard(Board currentBoard)
        {
            //throw new Exception("Aceasta metoda trebuie implementata");
            Board bestVariant = new Board(currentBoard);
            List<Board> bestVariants = new List<Board>();
            List<Move> bestMoves = new List<Move>();
            double max = 0;
            bool first = true;
            foreach (Piece piece in currentBoard.Pieces)
            {
                // Selectez piesele ce apartin calculatorului
                if (piece.Player == PlayerType.Computer)
                {
                    List<Move> moves = piece.ValidMoves(currentBoard);
                    foreach (Move move in moves)
                    {
                        Board boardVariant = currentBoard.MakeMove(move);
                        double score = Evaluate(boardVariant, 0);
                        if (first)
                        {
                            max = score;
                            first = false;
                            //bestMove = new Board(boardVariance);
                            bestVariants.Add(boardVariant);
                            bestMoves.Add(move);
                        }
                        else if (max < score)
                        {
                            max = score;
                            //bestMove = new Board(boardVariance);
                            bestVariants = new List<Board>();
                            bestVariants.Add(boardVariant);
                            bestMoves = new List<Move>();
                            bestMoves.Add(move);
                        }
                        else if (max == score)
                        {
                            bestVariants.Add(boardVariant);
                            bestMoves.Add(move);
                        }
                    }
                }
            }
            int chosen = _rand.Next(bestVariants.Count);
            //bestVariant = bestVariants[chosen];

            return (bestVariants[chosen], bestMoves[chosen]);
        }

        // functia recursiva prin care voi calcula o mutare

        public static double Evaluate(Board currentBoard, int depth)
        {
            // trebuie sa clonez currentBoard
            // daca adancimea are valoare para e calculatorul(maximizez)
            // altfel e jucatorul daci minimizez
            if (depth >= Minimax._depth)
            {
                return currentBoard.EvaluationFunction2();
            }
            //Maximizare
            if (depth % 2 == 0)
            {
                double max = 0;
                bool first = true;
                foreach (Piece piece in currentBoard.Pieces)
                {
                    if (piece.Player == PlayerType.Computer)
                    {
                        List<Move> moves = piece.ValidMoves(currentBoard);
                        foreach (Move move in moves)
                        {
                            Board boardVariant = currentBoard.MakeMove(move);
                            double score = Evaluate(boardVariant, depth + 1);
                            if (first)
                            {
                                max = score;
                                first = false;
                            }
                            else if (max < score)
                            {
                                max = score;
                            }
                        }
                    }
                }
                return max;
            }
            else
            {
                //Minimizez
                double min = 0;
                bool first = true;
                foreach (Piece piece in currentBoard.Pieces)
                {
                    if (piece.Player == PlayerType.Human)
                    {
                        List<Move> moves = piece.ValidMoves(currentBoard);
                        foreach (Move move in moves)
                        {
                            Board boardVariant = currentBoard.MakeMove(move);
                            double score = Evaluate(boardVariant, depth + 1);
                            if (first)
                            {
                                min = score;
                                first = false;
                            }
                            else if (min > score)
                            {
                                min = score;
                            }
                        }
                    }
                }
                return min;
            }
        }





    }
}