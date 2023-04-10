using App.Logic_Components.Boards;
using App.Logic_Components;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using World.UI.Board.CellKinds;
using System.Linq;

namespace World.UI.Board
{
    public class CellsArea : MonoBehaviour
    {
        private RectTransform _transform;
        private IBoard _board;

        [SerializeField] private GameObject _cellPrefab;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
        }

        public void Init(int rows, int cols)
        {
            var cells = GenerateCells(rows, cols);
            _board = new StandardBoard(rows, cols, cells);
            Refresh();
        }

        private IEnumerable<ICell> GenerateCells(int rows, int cols)
            => Enumerable
                .Repeat(Instantiate(_cellPrefab), rows * cols)
                .Select(go => go.GetComponent<ICell>())
                .ToList();

        public void Refresh()
        {
            var width = _transform.rect.width;
            var height = _transform.rect.height;
            var colWidth = width / _board.Cols;
            var rowHeight = height / _board.Rows;

            for(int i = 0; i < _board.Rows; ++i)
            {
                for(int j = 0; j < _board.Cols; ++j)
                {
                    _board[i, j]
                }
            }
        }
    }
}

