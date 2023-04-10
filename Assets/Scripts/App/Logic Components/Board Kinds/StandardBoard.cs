using System.Collections.Generic;
using System.Linq;

namespace App.Logic_Components.Boards
{
    public class StandardBoard : IBoard
    {
        private readonly List<ICell> _Cells;
        private readonly int _Rows;
        private readonly int _Cols;

        public int Rows => _Rows;
        public int Cols => _Cols;

        public IEnumerable<ICell> Cells => _Cells;
        public CellValue this[int m, int n] => _Cells[m * Cols + n].Value;

        //public StandardBoard(int rows, int cols) : 
        //    this
        //    (
        //        rows, 
        //        cols, 
        //        Enumerable.Repeat(BoardValues.EMPTY, rows * cols).ToList()
        //    )
        //{ }

        public StandardBoard(int rows, int cols, IEnumerable<ICell> values)
        {
            _Cells = values.ToList();
            _Rows = rows;
            _Cols = cols;
        }
    }
}
