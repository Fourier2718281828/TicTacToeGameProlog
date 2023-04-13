using System;
using System.Collections.Generic;
using System.Linq;
using World.UI.Board.CellKinds;

namespace App.Logic_Components.Boards
{
    public class StandardBoard : IRenderableBoard
    {
        private readonly List<IRenderableCell> _Cells;
        private readonly int _Rows;
        private readonly int _Cols;

        public int Rows => _Rows;
        public int Cols => _Cols;

        public IEnumerable<ICell> Cells => _Cells;

        public IEnumerable<IRenderableCell> RenderableCells => _Cells;

        //public ICell this[int m, int n] => RenderableAt(m, n);
        public ICell this[int index] => _Cells[index];

        public StandardBoard(int rows, int cols, IEnumerable<IRenderableCell> values)
        {
            _Cells = values.ToList();
            _Rows = rows;
            _Cols = cols;

            if (_Cells.Count != _Rows * _Cols)
                throw new ArgumentException("Invalid enumerable input.");
        }

        public IRenderableCell RenderableAt(int i, int j)
        {
            return _Cells[i * Cols + j];
        }
    }
}
