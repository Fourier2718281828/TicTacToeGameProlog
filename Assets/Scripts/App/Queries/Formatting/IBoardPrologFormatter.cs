using System.Collections.Generic;
using App.Logic_Components;
using App.Logic_Components.Boards;

namespace App.Queries.Formatting
{
    public interface IBoardPrologFormatter
    {
        public string ToPrologFormat(IBoard board);
        public IEnumerable<CellValue> ToBoardFormat(string plrep);
    }
}
