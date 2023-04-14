using App.Logic_Components;
using App.Logic_Components.Boards;
using App.Queries.Formatting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


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
        private IPrologFormatter _formatter;

        public CmdQueryHandler(IPrologFormatter formatter)
        {
            _swipl = new CmdPrologEngine();
            _formatter = formatter;
        }

        public async Task<int?> NextBoard(IBoard currBoard, CellValue currentPlayer)
        {
            string val = await _swipl.Query
            (
                true,
                PL_NEXT_TURN_NAME,
                "X",
                _formatter.ToPrologFormat(currBoard),
                ((int)currentPlayer).ToString(),
                "X"
            );

            int res;
            int? result = null;
            if (int.TryParse(val, out res))
            {
                Debug.Log($"Prolog index = {res}");
                result = res;
            }

            return result;
        }

        public async Task<IEnumerable<IEnumerable<CellValue>>> VictorySequences(IBoard currBoard)
        {
            IEnumerable<IEnumerable<CellValue>> res = new List<List<CellValue>>();
            string val = await _swipl.Query
            (
                false, 
                PL_VICTORY_SEQUENCES_NAME, 
                "X", 
                _formatter.ToPrologFormat(currBoard), 
                "X"
            );

            Debug.Log($"Prolog output: {val}");
            res = _formatter.DoubleListToEnumerable(val);
            return res;
        }


        ///////////////////
        public async void SetPrologDimensions(int m, int n)
            => await _swipl.Query
               (
                  false,
                  PL_SET_DIMENSIONS_NAME,
                  m.ToString(),
                  n.ToString()
               );

        public async void SetBoardValues()
        {
            var values = Enum.GetValues(typeof(CellValue));
            foreach (CellValue value in values)
            {
                await SetPrologBoardValue
                (
                    ToPrologPredicateName(value),
                    (int)value
                );
            }
        }

        private async Task SetPrologBoardValue(string name, int value)
            => await _swipl.Query
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
