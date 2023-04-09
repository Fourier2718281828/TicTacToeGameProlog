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
        public IEnumerable<BoardValues> ToBoardFormat(string plrep)
        {
            try
            {
                return plrep
                    .Trim('[', ']')
                    .Split(", ")
                    .Select
                    (
                        elem => (BoardValues)Enum.Parse(typeof(BoardValues), elem)
                    );

            }
            catch(ArgumentException)
            {
                throw new ArgumentException("Invalid format of board's Prolog representation.");
            }
            
        }

        public string ToPrologFormat(IBoard board)
        {
            return "[" + string.Join(", ", board.Values.Select(n => (int)n)) + "]";
        }
    }
}
