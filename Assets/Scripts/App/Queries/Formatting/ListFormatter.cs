using App.Logic_Components;
using App.Logic_Components.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Queries.Formatting
{
    public class ListFormatter : IBoardPrologFormatter
    {
        public IEnumerable<CellValue> ToBoardFormat(string plrep)
        {
            try
            {
                return plrep
                    .Trim('[', ']')
                    .Split(", ")
                    .Select
                    (
                        elem => (CellValue)Enum.Parse(typeof(CellValue), elem)
                    );

            }
            catch(ArgumentException)
            {
                throw new ArgumentException("Invalid format of board's Prolog representation.");
            }
            
        }

        public string ToPrologFormat(IBoard board)
        {
            return "[" + string.Join(", ", board.Cells.Select(c => (int)(c.Value))) + "]";
        }
    }
}
