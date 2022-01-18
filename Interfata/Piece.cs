/**************************************************************************
 *                                                                        *
 *  Copyright:   (c) 2016-2020, Florin Leon                               *
 *  E-mail:      florin.leon@academic.tuiasi.ro                           *
 *  Website:     http://florinleon.byethost24.com/lab_ia.html             *
 *  Description: Game playing. Minimax algorithm                          *
 *               (Artificial Intelligence lab 7)                          *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/


using System;
using System.Collections.Generic;

namespace SimpleCheckers
{
    public enum PlayerType { None, Computer, Human };
    // Piesa normala se misca pe diagolele fata/stanga fata/dreapta
    // Piesa de tip king se misca pe toate cele 4 diagonale
    public enum PieceType { Normal, King};

    /// <summary>
    /// Reprezinta o piesa de joc
    /// </summary>
    public partial class Piece
    {
        public int Id { get; set; } // identificatorul piesei
        public int X { get; set; } // pozitia X pe tabla de joc
        public int Y { get; set; } // pozitia Y pe tabla de joc
        public PlayerType Player { get; set; } // carui tip de jucator apartine piesa (om sau calculator)
        public PieceType PT { get; set; }

        public Piece(int x, int y, int id, PlayerType player)
        {
            X = x;
            Y = y;
            Id = id;
            Player = player;
            PT = PieceType.Normal;
        }


        // -------------------------------

        /// <summary>
        /// Genereaza atacuri valide pentru o piesa
        /// Atacurile trebuie sa fie verificate, deoarece este posibil
        /// sa ajung intr-o locatie in care se afla o alta piesa
        /// </summary>
        /// <param name="currentBoard"></param>
        /// <returns></returns>
        public List<Move> ValidAttacks(Board currentBoard)
        {
            List<Move> validAttacks = new List<Move>();

            // Verific daca am o piesa inamica in directia in care merg
            foreach (Piece piece in currentBoard.Pieces)
            {
                // Mai intai generez pt calculator
                // Am gasit o piesa inamica
                // verific daca se afla in calea mea
                if (this.Player == PlayerType.Computer && piece.Player == PlayerType.Human
                    && (this.PT == PieceType.King && Math.Abs(this.X - piece.X) == 1 && Math.Abs(this.Y - piece.Y) == 1
                        || this.PT == PieceType.Normal && Math.Abs(this.X - piece.X) == 1 && this.Y - piece.Y == 1))
                {
                    Move move = new Move(this.Id, this.X + 2 * (piece.X - this.X), this.Y + 2 * (piece.Y - this.Y));
                    move.Type = MoveType.Attack;
                    move.AttackedId = piece.Id;
                    if (IsValidAttack(currentBoard, move)) validAttacks.Add(move);
                }

                if (this.Player == PlayerType.Human && piece.Player == PlayerType.Computer
                    && (this.PT == PieceType.King && Math.Abs(this.X - piece.X) == 1 && Math.Abs(this.Y - piece.Y) == 1
                        || this.PT == PieceType.Normal && Math.Abs(this.X - piece.X) == 1 && this.Y - piece.Y == -1))
                {
                    Move move = new Move(this.Id, this.X + 2 * (piece.X - this.X), this.Y + 2 * (piece.Y - this.Y));
                    move.Type = MoveType.Attack;
                    move.AttackedId = piece.Id;
                    if (IsValidAttack(currentBoard, move)) validAttacks.Add(move);
                }
            }

            return validAttacks;
        }
        /// <summary>
        /// Aceasta metoda verifica daca o mutare poate fi considerata atac
        /// daca returneaza false nu inseamna ca e gresita mutarea ci doar ca nu e atac
        /// </summary>
        /// <param name="currentBoard"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        public bool IsValidAttack(Board currentBoard, Move move)
        {
            /*
             * Verific daca nu iese de pe tabla
             * Verific daca se afla o alta piesa in locul in care ajung
             * 
             */
            if (move.Type == MoveType.Attack && move.NewX >= 0 && move.NewX < currentBoard.Size && move.NewY >= 0 && move.NewY < currentBoard.Size)
                foreach (Piece piece in currentBoard.Pieces)
                {
                    if (move.NewX == piece.X && move.NewY == piece.Y)
                        return false;
                }
            else return false;

            return true;
        }
        /// <summary>
        /// Returneaza lista tuturor mutarilor permise pentru piesa curenta (this)
        /// in configuratia (tabla de joc) primita ca parametru
        /// </summary>
        public List<Move> ValidMoves(Board currentBoard)
        {
            // pot muta o pisa intr-o locatie in functie de tipul acesteia 
            // King - orice directie pe diagonala
            // Normal Computer diagonala stanga dreapta in sensul pozitiv al axei OY
            // Normal Human diagonala stanga dreapta in sensul negativ al axei OY
            // O piesa nu poate fi mutata in acelasi loc in care se afla

            List<Move> validMoves = new List<Move>();
            /*
            for(int i=-1;i<=1;i++)
                for(int j=-1;j<=1;j++)
                {
                    
                    if (i!=0 && j != 0 )
                    {
                        Move nextMove = new Move(this.Id, this.X + i, this.Y + j);
                        if (IsValidMove(currentBoard, nextMove))
                        {
                            validMoves.Add(nextMove);
                        }
                    }
                }
            */
            /* c1, c2, c3, c4 sunt considerate cadrane, mutarile pe diagonala in acele cadrane
             * imi asum ca mutarea nu este una de atac
             * 
             *                  l+
             *           c2     l      c1
             *      -___________I___________+
             *                  I
             *           c3     l       c4
             *                  l-
             */
            if (PT == PieceType.King)
            {
                Move c1 = new Move(this.Id, this.X + 1, this.Y + 1);
                if (IsValidMove(currentBoard, c1)) validMoves.Add(c1);

                Move c2 = new Move(this.Id, this.X - 1, this.Y + 1);
                if (IsValidMove(currentBoard, c2)) validMoves.Add(c2);

                Move c3 = new Move(this.Id, this.X - 1, this.Y - 1);
                if (IsValidMove(currentBoard, c3)) validMoves.Add(c3);

                Move c4 = new Move(this.Id, this.X + 1, this.Y - 1);
                if (IsValidMove(currentBoard, c4)) validMoves.Add(c4);

            }
            else if (PT == PieceType.Normal && Player == PlayerType.Computer)
            {
                Move c3 = new Move(this.Id, this.X - 1, this.Y - 1);
                if (IsValidMove(currentBoard, c3)) validMoves.Add(c3);

                Move c4 = new Move(this.Id, this.X + 1, this.Y - 1);
                if (IsValidMove(currentBoard, c4)) validMoves.Add(c4);

            }
            else if (PT == PieceType.Normal && Player == PlayerType.Human)
            {
                Move c1 = new Move(this.Id, this.X + 1, this.Y + 1);
                if (IsValidMove(currentBoard, c1)) validMoves.Add(c1);

                Move c2 = new Move(this.Id, this.X - 1, this.Y + 1);
                if (IsValidMove(currentBoard, c2)) validMoves.Add(c2);
            }
            validMoves.AddRange(ValidAttacks(currentBoard));


            return validMoves;
        }

        /// <summary>
        /// Testeaza daca o mutare este valida intr-o anumita configuratie
        /// </summary>
        public bool IsValidMove(Board currentBoard, Move move)
        {


            // Verific daca mutarea nu este in afara tablei
            if (move.NewX < currentBoard.Size && move.NewX >= 0 && move.NewY < currentBoard.Size && move.NewY >= 0)
            {
                // verific daca am mutat pe diagonala care trebuie
                //momentan mutarile nu permit atacuri
                if ((PT == PieceType.King && (move.NewX == X || move.NewY == Y)) ||
                       PT == PieceType.Normal && (Player == PlayerType.Human && !(Math.Abs(move.NewX - X) == 1 && move.NewY - Y == 1) ||
                                                  Player == PlayerType.Computer && !(Math.Abs(move.NewX - X) == 1 && move.NewY - Y == -1))
                    )
                {
                    return false;
                }
                foreach (Piece piece in currentBoard.Pieces)
                {
                    // Daca o alta piesa se afla acolo mutarea nu este valida
                    if (move.NewX == piece.X && move.NewY == piece.Y)
                    {
                        return false;
                    }


                    // Verific daca mutarea nu depaseste o casuta distanta
                    // daca asta e piesa inainte de a fi mutata(ma uit la ID)
                    // vezi daca depaseste o patratica distanta

                    /*
                    if (piece.Id == move.PieceId && (Math.Abs(piece.X-move.NewX)>1 || Math.Abs(piece.Y-move.NewY)>1))
                    //if ((Math.Abs(piece.X - move.NewX) > 1 || Math.Abs(piece.Y - move.NewY) > 1))
                    {
                        
                        return false;
                    }
                    */
                }
                /*
                MainForm.richTextBox1.Text = MainForm.richTextBox1.Text +
                            "\n\nvalid " + move.PieceId + " in" +
                            "\n x=" + move.NewX + " y=" + move.NewY;
                */
                return true;
            }
            return false;
        }




    }
}