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
        private IBoardPrologFormatter _formatter;
        private PrologEngine _swipl;

        public QueryHandler(IBoardPrologFormatter formatter)
        {
            _formatter = formatter;
            _swipl = new PrologEngine(persistentCommandHistory : false);
            InitSwipl();
        }

        private void InitSwipl()
        {
            _swipl.Consult(PL_FILENAME);

            //const int m = 3, n = 3;
            //SetBoardValues();
            //SetPrologDimensions(m, n);


            //CellValue[] lst = new CellValue[m * n]
            //{
            //    CellValue.NAUGHT,
            //    CellValue.NAUGHT,
            //    CellValue.NAUGHT,
            //    CellValue.EMPTY,
            //    CellValue.NAUGHT,
            //    CellValue.EMPTY,
            //    CellValue.EMPTY,
            //    CellValue.EMPTY,
            //    CellValue.NAUGHT,
            //};
            //IBoard curr = new StandardBoard(m, n, lst);
            //VictorySequences(curr);
            //NextBoard(curr, CellValue.NAUGHT);
        }

        #region IBoard Methods
        public IBoard NextBoard(IBoard currBoard, CellValue currentPlayer)
        {
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
                    Debug.Log(val);
                }
            );
            return null;
        }

        public IEnumerable<IEnumerable<int>> VictorySequences(IBoard currBoard) 
        {
            ExecuteQuery(QueryFormat(false, PL_VICTORY_SEQUENCES_NAME, _formatter.ToPrologFormat(currBoard), "X"));
            ProcessSolutions
            (
                val => 
                {
                    Debug.Log(val);
                }
            );
            return null;
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
