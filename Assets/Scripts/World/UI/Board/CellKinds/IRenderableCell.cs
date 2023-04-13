using App.Logic_Components;
using UnityEngine;

namespace World.UI.Board.CellKinds
{
    public interface IRenderableCell : ICell
    {
        public CellState RenderedState { get; set; }
        public GameObject GameObject { get; }
    }
}
