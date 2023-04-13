using System.Collections.Generic;

namespace App.Logic_Components.Boards
{
    public interface IBoard
    {
        public IEnumerable<ICell> Cells { get; }
        public ICell this[int index] { get; }
        public int Rows { get; }
        public int Cols { get; }
    }
}
    