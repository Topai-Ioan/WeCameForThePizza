namespace Chess.Models
{
    public class Move
    {
        public int Id { get; set; }
        public string Piece { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public Move(int _Id, string _Piece, int _StartX, int _StartY, int _EndX, int _EndY)
        {
            Id = _Id;
            Piece = _Piece;
            StartX = _StartX;
            StartY = _StartY;
            EndX = _EndX;
            EndY = _EndY;
        }
        public Object Clone()
        {
            return MemberwiseClone();
        }
    }
}
