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
        {
            List<IRenderableCell> list = new();
            for(int i = 0; i < rows*cols; ++i)
            {
                var go = Instantiate(_cellPrefab);
                Debug.Log(go);
                list.Add(go.GetComponent<IRenderableCell>());
            }

            return list;
        }

        private void FillBoard()
        {
            var width = _transform.rect.width;
            var height = _transform.rect.height;
            var colWidth = width / _board.Cols;
            var rowHeight = height / _board.Rows;
            var colHalfWidth  = 0.5f * colWidth;
            var rowHalfHeight = 0.5f * rowHeight;

            //Debug.Log($"Width {width}");
            //Debug.Log($"Height {height}");
            //Debug.Log($"colHalfWidth  {colHalfWidth }");
            //Debug.Log($"rowHalfHeight {rowHalfHeight}");
            var initPos = new Vector3(width, height, CELLS_APLICATE);
            for(int i = 0; i < _board.Rows; ++i)
            {
                for(int j = 0; j < _board.Cols; ++j)
                {
                    
                    PositionCellAt(i, j, new Vector3
                        (
                            colHalfWidth * (j + 1),
                            rowHalfHeight * (i + 1),
                            CELLS_APLICATE
                        ));
                    ScaleCellToSize(i, j, new(colWidth, rowHeight));
                }
            }
        }

        private void PositionCellAt(int i, int j, Vector3 pos)
        {
            var cell = _board.RenderableAt(i, j).GameObject;
            var cellTransform = cell.transform;
            cellTransform.SetParent(gameObject.transform);
            var rectTransform = cellTransform.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = pos;
            rectTransform.localPosition = new(rectTransform.localPosition.x, rectTransform.localPosition.y, pos.z);
        }

        private void ScaleCellToSize(int i, int j, Vector2 size)
        {
            var cell = _board.RenderableAt(i, j).GameObject;
            var rectTransform = cell.transform.GetComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            rectTransform.localScale = new Vector3(1, 1, 1);
        }
    }
}

