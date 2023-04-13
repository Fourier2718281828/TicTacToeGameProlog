using App.Logic_Components;
using App.Logic_Components.Boards;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Queries
{
    public interface IQueryHandler
    {
        public Task<int?> NextBoard(IBoard currBoard, CellValue currentPlayer);
        public Task<IEnumerable<IEnumerable<CellValue>>> VictorySequences(IBoard currBoard);
    }
}
