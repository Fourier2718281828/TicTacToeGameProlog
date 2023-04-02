using System.Collections.Generic;

namespace App.Logic_Components.Boards
{
    public interface IBoard
    {
        public BoardValues this[int m, int n] { get; }
        public int Rows { get; }
        public int Cols { get; }
        public bool IsFinal();
    }
}
