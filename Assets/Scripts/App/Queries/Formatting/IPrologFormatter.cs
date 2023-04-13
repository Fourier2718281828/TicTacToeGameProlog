using System.Collections.Generic;
using App.Logic_Components;
using App.Logic_Components.Boards;

namespace App.Queries.Formatting
{
    public interface IPrologFormatter
    {
        public string ToPrologFormat(IBoard board);
        public IEnumerable<CellValue> ListToEnumerable(string plrep);
        public IEnumerable<IEnumerable<CellValue>> DoubleListToEnumerable(string plrep);
    }
}
