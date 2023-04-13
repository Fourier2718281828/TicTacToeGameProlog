using App.Logic_Components.Boards;
using App.Logic_Components;
using Prolog;
using System.Collections.Generic;
using System;
using App.Queries.Formatting;
using UnityEngine;

using World.UI.Board.CellKinds;

namespace App.Queries
{
    public class QueryHandler : IQueryHandler
    {
        private delegate void SolutionValuesProcessor(string nxtVal);
        private const string PL_FILENAME = "Assets/Prolog/Minimax.pl";
        private const string PL_SET_BOARD_VALUE_NAME = "set_board_value";
        private const string PL_SET_DIMENSIONS_NAME = "set_dimensions";
        private const string PL_VICTORY_SEQUENCES_NAME = "all_victory_sequences";
        private const string PL_NEXT_TURN_NAME = "next_turn";
        private IPrologFormatter _formatter;
        private PrologEngine _swipl;

        public QueryHandler(IPrologFormatter formatter)
        {
            _formatter = formatter;
            _swipl = new PrologEngine(persistentCommandHistory : false);
            InitSwipl();
        }

        private void InitSwipl()
        {
            _swipl.Consult(PL_FILENAME);
        }

        #region IBoard Methods
        public int? NextBoard(IBoard currBoard, CellValue currentPlayer)
        {
            int? result = null;
            ExecuteQuery(QueryFormat
            (
                true, 
                PL_NEXT_TURN_NAME,
                _formatter.ToPrologFormat(currBoard),
                ((int)currentPlayer).ToString(),
                "X"
            ));
            ProcessSolutions
            (
                val => 
                {
                    int res;
                    if(int.TryParse(val, out res))
                    {
                        result = res;
                    }
                    else
                    {
                        result = null;
                    }
                }
            );
            return result;
        }

        public IEnumerable<IEnumerable<CellValue>> VictorySequences(IBoard currBoard) 
        {
            IEnumerable<IEnumerable<CellValue>> res = new List<List<CellValue>>();
            ExecuteQuery(QueryFormat(false, PL_VICTORY_SEQUENCES_NAME, _formatter.ToPrologFormat(currBoard), "X"));
            ProcessSolutions
            (
                val => 
                {
                    Debug.Log($"Prolog output: {val}");
                    res = _formatter.DoubleListToEnumerable(val);
                }
            );
            return res;
        }
        #endregion

        #region Prolog Interaction
        private void ProcessSolutions(SolutionValuesProcessor processor)
        {
            foreach (var solution in _swipl.SolutionIterator)
            {
                foreach (var f in solution.VarValuesIterator)
                {
                    if (f.DataType == "namedvar") continue;
                    var strRep = f.Value.ToString();
                    processor(strRep);
                }
            }
        }

        private void ExecuteQuery(string query) => _swipl.Query = query;

        private void SetPrologDimensions(int m, int n)
            => ExecuteQuery(QueryFormat
               (
                   false,
                   PL_SET_DIMENSIONS_NAME,
                   m.ToString(), 
                   n.ToString()
               ));

        private void SetBoardValues()
        {
            var values = Enum.GetValues(typeof(CellValue));
            foreach(CellValue value in values)
            {
                SetPrologBoardValue
                (
                    ToPrologPredicateName(value),
                    (int)value
                );
            }
        }

        private void SetPrologBoardValue(string name, int value)
            => ExecuteQuery(QueryFormat
                (
                    false, 
                    PL_SET_BOARD_VALUE_NAME, 
                    name, 
                    value.ToString()
                ));

        private string ToPrologPredicateName(CellValue val) => val switch
        {
            CellValue.EMPTY  => "empty",
            CellValue.CROSS  => "cross",
            CellValue.NAUGHT => "naught",
            _                  => ""
        };

        private static string QueryFormat(bool cut, string predicateName, params string[] ps)
        {
            string res = "";

            res += predicateName + "(";
            for(var i = 0; i < ps.Length; i++)
                res += ps[i] + 
                    (
                        i == ps.Length - 1 
                        ? ""
                        : ", "
                    );
            res += cut 
                ? "),!."
                : ").";

            return res;
        }
        #endregion
    }
}
