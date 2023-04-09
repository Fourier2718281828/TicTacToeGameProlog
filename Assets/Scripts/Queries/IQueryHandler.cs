using App.Logic_Components;
using App.Logic_Components.Boards;
using System.Collections.Generic;

namespace App.Queries
{
    public interface IQueryHandler
    {
        public IBoard NextBoard(IBoard currBoard, BoardValues currentPlayer);
        public IEnumerable<IEnumerable<int>> VictorySequences(IBoard currBoard);
    }
}
