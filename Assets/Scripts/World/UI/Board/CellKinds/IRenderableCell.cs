using App.Logic_Components;

namespace World.UI.Board
{
    public interface IRenderableCell : ICell
    {
        public CellState RenderedState { get; set; }
    }
}
