using App.Logic_Components.Boards;
using App.Logic_Components;
using Prolog;
using System.Collections.Generic;
using System;
using App.Queries.Formatting;
using UnityEngine;

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
            const int m = 3, n = 3;
            _swipl.Consult(PL_FILENAME);

            //SetBoardValues();
            //SetPrologDimensions(m, n);


            BoardValues[] lst = new BoardValues[m * n]
            {
                BoardValues.NAUGHT,
                BoardValues.NAUGHT,
                BoardValues.NAUGHT,
                
                BoardValues.EMPTY,
                BoardValues.NAUGHT,
                BoardValues.EMPTY,

                BoardValues.EMPTY,
                BoardValues.EMPTY,
                BoardValues.NAUGHT,
            };
            IBoard curr = new StandardBoard(m, n, lst);
            VictorySequences(curr);
            NextBoard(curr, BoardValues.NAUGHT);
        }

        #region IBoard Methods
        public IBoard NextBoard(IBoard currBoard, BoardValues currentPlayer)
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
                Debug.Log(solution.ToString());
                foreach (var f in solution.VarValuesIterator)
                {
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
            var values = Enum.GetValues(typeof(BoardValues));
            foreach(BoardValues value in values)
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

        private string ToPrologPredicateName(BoardValues val) => val switch
        {
            BoardValues.EMPTY  => "empty",
            BoardValues.CROSS  => "cross",
            BoardValues.NAUGHT => "naught",
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
            Debug.Log("Query: " + res);
            return res;
        }
        #endregion
    }
}
