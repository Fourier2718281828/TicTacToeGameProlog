using App.Logic_Components;
using App.Logic_Components.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App.Queries
{
    public class CmdQueryHandler : IQueryHandler
    {
        private const string PL_FILENAME = "Assets/Prolog/Minimax.pl";
        private const string PL_SET_BOARD_VALUE_NAME = "set_board_value";
        private const string PL_SET_DIMENSIONS_NAME = "set_dimensions";
        private const string PL_VICTORY_SEQUENCES_NAME = "all_victory_sequences";
        private const string PL_NEXT_TURN_NAME = "next_turn";
        private CmdPrologEngine _swipl;

        public CmdQueryHandler()
        {
            _swipl = new CmdPrologEngine();
        }

        public async Task<int?> NextBoard(IBoard currBoard, CellValue currentPlayer)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IEnumerable<CellValue>>> VictorySequences(IBoard currBoard)
        {
            throw new NotImplementedException();
        }


        ///////////////////
        private void SetPrologDimensions(int m, int n)
            => _swipl.Query
               (
                  false,
                  PL_SET_DIMENSIONS_NAME,
                  m.ToString(),
                  n.ToString()
               );

        private void SetBoardValues()
        {
            var values = Enum.GetValues(typeof(CellValue));
            foreach (CellValue value in values)
            {
                SetPrologBoardValue
                (
                    ToPrologPredicateName(value),
                    (int)value
                );
            }
        }

        private void SetPrologBoardValue(string name, int value)
            => _swipl.Query
               (
                   false,
                   PL_SET_BOARD_VALUE_NAME,
                   name,
                   value.ToString()
               );

        private string ToPrologPredicateName(CellValue val) => val switch
        {
            CellValue.EMPTY => "empty",
            CellValue.CROSS => "cross",
            CellValue.NAUGHT => "naught",
            _ => ""
        };
    }
}
