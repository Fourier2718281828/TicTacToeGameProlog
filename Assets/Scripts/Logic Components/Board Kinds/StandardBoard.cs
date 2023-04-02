using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Logic_Components.Boards
{
    public class StandardBoard : IBoard
    {
        private readonly List<BoardValues> _Values;
        private readonly int _Rows;
        private readonly int _Cols;

        public int Rows => _Rows;

        public int Cols => _Cols;

        public BoardValues this[int m, int n] => _Values[m * Cols + n];

        public StandardBoard(int rows, int cols) : 
            this(rows, cols, Enumerable.Repeat(BoardValues.EMPTY, rows * cols).ToList())
        { }

        public StandardBoard(int rows, int cols, IEnumerable<BoardValues> values)
        {
            _Values = values.ToList();
            _Rows = rows;
            _Cols = cols;
        }

        public override string ToString()
        {
            return "[" + string.Join(", ", _Values.Select(n => (int)n)) + "]";
        }
    }
}
