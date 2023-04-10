using App.Logic_Components.Boards;
using UnityEngine;

namespace World.UI.Board
{
    public class CellsArea : MonoBehaviour
    {
        private Transform _transform;
        private IBoard _board;

        private void Awake()
        {
            _transform = transform;
        }

        public void Init(IBoard board)
        {
            _board = board;
            Refresh();
        }

        public void Refresh()
        {

        }
    }
}

