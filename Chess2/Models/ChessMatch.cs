using System.Diagnostics;

namespace Chess.Models
{
    public class ChessMatch
    {
        public List<List<string>> Board { get; set; }
        public int Id { get; set; }
        public bool WhiteTurn = true;
        public string Victory = "";
        public ChessMatch(int _id)
        {
            Board = new();
            Board.Add(new List<string>() { "BR", "BKn", "BB", "BQ", "BK", "BB", "BKn", "BR" });
            Board.Add(new List<string>() { "BP", "BP", "BP", "BP", "BP", "BP", "BP", "BP" });
            Board.Add(new List<string>() { "", "", "", "", "", "", "", "" });
            Board.Add(new List<string>() { "", "", "", "", "", "", "", "" });
            Board.Add(new List<string>() { "", "", "", "", "", "", "", "" });
            Board.Add(new List<string>() { "", "", "", "", "", "", "", "" });
            Board.Add(new List<string>() { "WP", "WP", "WP", "WP", "WP", "WP", "WP", "WP" });
            Board.Add(new List<string>() { "WR", "WKn", "WB", "WQ", "WK", "WB", "WKn", "WR" });
            Id = _id;
        }
        public bool Move(Move move)
        {
            if (move.Piece == "") { Trace.WriteLine("no piece selected"); return false; }
            if (Victory != "") { Trace.WriteLine("match is over, go home"); return false; }
            if (WhiteTurn == true && move.Piece[0] != "W"[0]) { Trace.WriteLine("invalid turn"); return false; }
            if (WhiteTurn == false && move.Piece[0] != "B"[0]) { Trace.WriteLine("invalid turn"); return false; }
            Trace.WriteLine("turn valid");
            if (!MoveIsValid(move)) { Trace.WriteLine("invalid movement"); return false; }
            Trace.WriteLine("movement valid");
            if (IsCastling(move)) { DoCastling(move); }
            else
            {
                Board[move.StartY][move.StartX] = "";
                Board[move.EndY][move.EndX] = move.Piece;
            }
            if (WhiteTurn) { WhiteTurn = false; }
            else { WhiteTurn = true; }
            return true;
        }
        private bool MoveIsValid(Move move)
        {
            Trace.WriteLine("------ Validating ------");
            Trace.WriteLine($"Match id:{Id} | Piece: {move.Piece}");
            Trace.WriteLine($"Start position: {move.StartX}:{move.StartY}");
            Trace.WriteLine($"End position: {move.EndX}:{move.EndY}");
            int Xdif = move.StartX - move.EndX;
            int Ydif = move.StartY - move.EndY;
            Trace.WriteLine($"Diffs: X:{Xdif}|Y:{Ydif}");
            if (Target(move) != "")
            {
                if (Target(move)[0] == move.Piece[0])
                {
                    Trace.WriteLine("ally on the way");
                    if (move.Piece[1] != "K"[0] || Target(move)[1] != "R"[0])
                    {
                        return false;
                    }
                }
            }
            switch (move.Piece)
            {
                case ("BP"):
                    Trace.WriteLine("BP detected");
                    if (Math.Abs(Xdif) == 0)
                    {
                        if (Ydif == -2 && move.StartY == 1 || Ydif == -1)
                        {
                            if (Board[move.EndY][move.EndX] == "") { if (KingAttacked(move.EndX,move.EndY)) Victory = "Black"; return true; }
                            return false;
                        }
                    }
                    if (Math.Abs(Xdif) == Math.Abs(Ydif))
                    {
                        if (Ydif == -2 && move.StartY == 1 || Ydif == -1)
                        {
                            if (Board[move.EndY][move.EndX] != "") { if (KingAttacked(move.EndX, move.EndY)) Victory = "Black"; return true; }
                            return false;
                        }
                    }
                    break;
                case ("BR"):
                    Trace.WriteLine("BR detected");
                    if (Xdif != 0 && Ydif == 0 || Xdif == 0 && Ydif != 0)
                    { // if it moves in X no move in Y or move in Y, no move in X
                        if (StraightCollitionCheck(move))
                        {
                            Trace.WriteLine("piece in the middle of movement");
                            return false;
                        }
                        if (KingAttacked(move.EndX, move.EndY)) Victory = "Black";
                        return true;
                    }
                    break;
                case ("BKn"):
                    Trace.WriteLine("BKn detected");
                    if (Math.Abs(Xdif) == 2 && Math.Abs(Ydif) == 1 || Math.Abs(Ydif) == 2 && Math.Abs(Xdif) == 1)
                    { // if difference in X = 2, difference in Y = 1 or difference in X = 1 and difference in Y = 2
                        if (KingAttacked(move.EndX, move.EndY)) Victory = "Black";
                        return true;
                    }
                    break;
                case ("BB"):
                    Trace.WriteLine("BB detected");
                    if (Math.Abs(Xdif) == Math.Abs(Ydif))
                    { // Y movement = X movement
                        if (DiagonalCollitionCheck(move))
                        {
                            Trace.WriteLine("piece in the middle of movement");
                            return false;
                        }
                        if (KingAttacked(move.EndX, move.EndY)) Victory = "Black";
                        return true;
                    }
                    break;
                case ("BQ"):
                    Trace.WriteLine("BQ detected");
                    if (Math.Abs(Xdif) == Math.Abs(Ydif) || Xdif != 0 && Ydif == 0 || Xdif == 0 && Ydif != 0)
                    { // Y movement = X movement or if it moves in X no move in Y or move in Y, no move in X
                        if (Math.Abs(Xdif) == Math.Abs(Ydif))
                        {
                            if (DiagonalCollitionCheck(move))
                            {
                                Trace.WriteLine("piece in the middle of movement");
                                return false;
                            }
                            if (KingAttacked(move.EndX, move.EndY)) Victory = "Black";
                            return true;
                        }
                        if (Xdif != 0 && Ydif == 0 || Xdif == 0 && Ydif != 0)
                        {
                            if (StraightCollitionCheck(move))
                            {
                                Trace.WriteLine("piece in the middle of movement");
                                return false;
                            }
                            if (KingAttacked(move.EndX, move.EndY)) Victory = "Black";
                            return true;
                        }
                    }
                    break;
                case ("BK"):
                    Trace.WriteLine("BK detected");
                    if (IsCastling(move))
                    {
                        Trace.WriteLine("Castling detected");
                        return true;
                    }
                    if (Math.Abs(Xdif) == Math.Abs(Ydif) || Xdif == 0 || Ydif == 0)
                    { // Y movement = X movement
                        if (Math.Abs(Ydif) == 1 || Math.Abs(Xdif) == 1)
                        { // movement difference in X or Y = 1
                            if (KingAttacked(move.EndX, move.EndY)) Victory = "Black";
                            return true;
                        }
                    }
                    break;
                case ("WP"):
                    Trace.WriteLine("WP detected");
                    if (Math.Abs(Xdif) == 0)
                    {
                        if (Ydif == 2 && move.StartY == 6 || Ydif == 1)
                        {
                            if (Board[move.EndY][move.EndX] == "") { if (KingAttacked(move.EndX, move.EndY)) Victory = "White"; return true; }
                            return false;
                        }
                    }
                    if (Math.Abs(Xdif) == Math.Abs(Ydif))
                    {
                        if (Ydif == 1)
                        {
                            if (Board[move.EndY][move.EndX] != "") { if (KingAttacked(move.EndX, move.EndY)) Victory = "White"; return true; }
                            return false;
                        }
                    }
                    break;
                case ("WR"):
                    Trace.WriteLine("WR detected");
                    if (Xdif != 0 && Ydif == 0 || Xdif == 0 && Ydif != 0)
                    { // if it moves in X no move in Y or move in Y, no move in X
                        if (StraightCollitionCheck(move))
                        {
                            Trace.WriteLine("piece in the middle of movement");
                            return false;
                        }
                        if (KingAttacked(move.EndX, move.EndY)) Victory = "White";
                        return true;
                    }
                    break;
                case ("WKn"):
                    Trace.WriteLine("WKn detected");
                    if (Math.Abs(Xdif) == 2 && Math.Abs(Ydif) == 1 || Math.Abs(Ydif) == 2 && Math.Abs(Xdif) == 1)
                    { // if difference in X = 2, difference in Y = 1 or difference in X = 1 and difference in Y = 2
                        if (KingAttacked(move.EndX, move.EndY)) Victory = "White";
                        return true;
                    }
                    break;
                case ("WB"):
                    Trace.WriteLine("WB detected");
                    if (Math.Abs(Xdif) == Math.Abs(Ydif))
                    { // Y movement = X movement
                        if (DiagonalCollitionCheck(move))
                        {
                            Trace.WriteLine("piece in the middle of movement");
                            return false;
                        }
                        if (KingAttacked(move.EndX, move.EndY)) Victory = "White";
                        return true;
                    }
                    break;
                case ("WQ"):
                    Trace.WriteLine("WQ detected");
                    if (Math.Abs(Xdif) == Math.Abs(Ydif) || Xdif != 0 && Ydif == 0 || Xdif == 0 && Ydif != 0)
                    { // Y movement = X movement or if it moves in X no move in Y or move in Y, no move in X
                        if (Math.Abs(Xdif) == Math.Abs(Ydif))
                        {
                            if (DiagonalCollitionCheck(move))
                            {
                                Trace.WriteLine("piece in the middle of movement");
                                return false;
                            }
                            if (KingAttacked(move.EndX, move.EndY)) Victory = "White";
                            return true;
                        }
                        if (Xdif != 0 && Ydif == 0 || Xdif == 0 && Ydif != 0)
                        {
                            if (StraightCollitionCheck(move))
                            {
                                Trace.WriteLine("piece in the middle of movement");
                                return false;
                            }
                            if (KingAttacked(move.EndX, move.EndY)) Victory = "White";
                            return true;
                        }
                    }
                    break;
                case ("WK"):
                    Trace.WriteLine("WK detected");
                    if (IsCastling(move))
                    {
                        Trace.WriteLine("Castling detected");
                        return true;
                    }
                    if (Math.Abs(Xdif) == Math.Abs(Ydif))
                    { // Y movement = X movement
                        if (Math.Abs(Ydif) == 1 || Math.Abs(Xdif) == 1)
                        { // movement difference in X or Y = 1
                            if (KingAttacked(move.EndX, move.EndY)) Victory = "White";
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }
        private bool KingAttacked(int X, int Y)
        {
            if (Board[Y][X] != "" && Board[Y][X][1] == "K"[0]) return true;
            else return false;
        }
        private bool StraightCollitionCheck(Move move)
            //Returns true if there's a collition
        {
            Move _move = (Move)move.Clone();
            bool collitionFound = false;
            if (_move.StartX == _move.EndX)
            {
                if (_move.EndY > _move.StartY) { _move.EndY--; }
                else _move.EndY++;
                while (_move.EndY != _move.StartY)
                {
                    if (Board[_move.EndY][_move.EndX] != "") { collitionFound = true; }
                    if (_move.EndY > _move.StartY) { _move.EndY--; }
                    else _move.EndY++;
                }
            }
            else
            {
                if (_move.EndX > _move.StartX) { _move.EndX--; }
                else _move.EndX++;
                while (_move.EndX != _move.StartX)
                {
                    if (Board[_move.EndY][_move.EndX] != "") { collitionFound = true; }
                    if (_move.EndX > _move.StartX) { _move.EndX--; }
                    else _move.EndX++;
                }
            }
            return collitionFound;
        }
        private bool DiagonalCollitionCheck(Move move)
            //Returns true if there's a collition
        {
            Move _move = (Move)move.Clone();
            bool collitionFound = false;
            if (_move.EndX < _move.StartX) _move.EndX++;
            else _move.EndX--;
            if (_move.EndY < _move.StartY) _move.EndY++;
            else _move.EndY--;
            while (_move.EndX != _move.StartX)
            {
                if (Board[_move.EndY][_move.EndX] != "") { collitionFound = true; }
                if (_move.EndX < _move.StartX) _move.EndX++;
                else _move.EndX--;
                if (_move.EndY < _move.StartY) _move.EndY++;
                else _move.EndY--;
            }
            return collitionFound;
        }
        private string Target(Move move)
        {
            return Board[move.EndY][move.EndX];
        }
        private bool IsCastling(Move move)
        {
            if (move.Piece[1] != "K"[0] || move.Piece.Length != 2) { return false; }
            Trace.WriteLine("piece 2nd char is k and piece len is 2");
            Trace.WriteLine($"checking: {move.StartX} == 4 | {Target(move)[1]} == {"R"[0]} | {move.Piece[0]} == {Target(move)[0]}");
            if (move.StartX == 4 && Target(move)[1] == "R"[0] && move.Piece[0] == Target(move)[0])
            {
                if (move.EndX != 0 && move.EndX != 7) return false;
                Trace.WriteLine("target position is in X= 0 or 7");
                if (move.Piece[0] == "W"[0]) { if (move.StartY != 7 || move.EndY != 7) return false; }
                Trace.WriteLine("if piece is white is on the last row");
                if (move.Piece[0] == "B"[0]) { if (move.StartY != 0 || move.EndY != 0) return false; }
                Trace.WriteLine("if piece is black is on the first row");
                if (StraightCollitionCheck(move)) return false;
                Trace.WriteLine("no collition");
                Trace.WriteLine("-!-!-!- CASTLING FOUND -!-!-!-");
                return true;
            }
            return false;
        }
        private void DoCastling(Move move)
        {
            Trace.WriteLine("-!-!-!- DOING CASTLING -!-!-!-");
            if (move.EndX == 0)
            {
                Board[move.StartY][move.StartX - 2] = move.Piece;
                Board[move.StartY][move.StartX - 1] = Board[move.EndY][move.EndX];
            }
            if (move.EndX == 7)
            {
                Board[move.StartY][move.StartX + 2] = move.Piece;
                Board[move.StartY][move.StartX + 1] = Board[move.EndY][move.EndX];
            }
            Board[move.StartY][move.StartX] = "";
            Board[move.EndY][move.EndX] = "";
        }
    }
}
