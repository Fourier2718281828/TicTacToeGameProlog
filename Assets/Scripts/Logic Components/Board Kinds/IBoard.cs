using System.Collections.Generic;

namespace App.Logic_Components.Boards
{
    public interface IBoard
    {
        public IEnumerable<BoardValues> Values { get; }
        public BoardValues this[int m, int n] { get; }
        public int Rows { get; }
        public int Cols { get; }
    }
}
