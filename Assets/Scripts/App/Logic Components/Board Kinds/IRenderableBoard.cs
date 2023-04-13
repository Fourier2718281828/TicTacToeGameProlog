using App.Logic_Components.Boards;
using System.Collections.Generic;
using World.UI.Board.CellKinds;

namespace App.Logic_Components.Boards
{
    public interface IRenderableBoard : IBoard
    {
        public IEnumerable<IRenderableCell> RenderableCells { get; }
        public IRenderableCell RenderableAt(int i, int j);
    }
}
