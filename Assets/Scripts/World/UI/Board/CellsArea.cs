using App.Logic_Components.Boards;
using App.Logic_Components;
using UnityEngine;
using System.Collections.Generic;
using World.UI.Board.CellKinds;
using System.Linq;

namespace World.UI.Board
{
    public class CellsArea : MonoBehaviour
    {
        private RectTransform _transform;
        private IRenderableBoard _board;
        const float CELLS_APLICATE = -10.0f;

        [SerializeField] private GameObject _cellPrefab;

        public IBoard Board => _board;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            Init(3, 3);
        }

        public void Init(int rows, int cols)
        {
            var cells = GenerateCells(rows, cols);
            _board = new StandardBoard(rows, cols, cells);
            FillBoard();
        }

        private IEnumerable<IRenderableCell> GenerateCells(int rows, int cols)
            => Enumerable
                .Repeat(Instantiate(_cellPrefab), rows * cols)
                .Select(go => go.GetComponent<IRenderableCell>())
                .ToList();

        private void FillBoard()
        {
            var width = _transform.rect.width;
            var height = _transform.rect.height;
            var colWidth = width / _board.Cols;
            var rowHeight = height / _board.Rows;
            var colHalfWidth  = 0.5f * colWidth;
            var rowHalfHeight = 0.5f * rowHeight;

            Debug.Log($"Width {width}");
            Debug.Log($"Height {height}");
            Debug.Log($"colHalfWidth  {colHalfWidth }");
            Debug.Log($"rowHalfHeight {rowHalfHeight}");

            for(int i = 0; i < _board.Rows; ++i)
            {
                for(int j = 0; j < _board.Cols; ++j)
                {
                    Debug.Log(new Vector3
                        (
                            colHalfWidth * (j + 1),
                            rowHalfHeight * (i + 1),
                            CELLS_APLICATE
                        ));
                    PositionCellAt(i, j, new Vector3
                        (
                            colHalfWidth * (j + 1),
                            rowHalfHeight * (i + 1),
                            CELLS_APLICATE
                        ));
                    //ScaleCellToSize(i, j, new(colWidth, rowHeight));
                }
            }
        }

        private void PositionCellAt(int i, int j, Vector3 pos)
        {
            var cell = _board.RenderableAt(i, j).GameObject;
            var cellTransform = cell.transform;
            cellTransform.parent = gameObject.transform;
            //cellTransform.position = pos;
        }

        //private void ScaleCellToSize(int i, int j, Vector2 size)
        //{
        //    var cell = _board.RenderableAt(i, j).GameObject;
        //    var renderer = cell.GetComponent<Renderer>();
        //    var initialSize = renderer.bounds.size;
        //    var scaleX = size.x / initialSize.x;
        //    var scaleY = size.y / initialSize.y;
        //    var scaleZ = cell.transform.localScale.z;
        //    cell.transform.localScale = new(scaleX, scaleY, scaleZ);
        //}
    }
}

