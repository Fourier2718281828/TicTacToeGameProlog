using App.Logic_Components.Boards;
using App.Logic_Components;
using Prolog;
using System.Collections.Generic;

using UnityEngine;
using System;

namespace App.Queries
{
    public class QueryHandler : IQueryHandler
    {
        private delegate void SolutionValuesProcessor(string nxtVal);
        private const string PROLOG_FILENAME = "Assets/Prolog/Minimax.pl";
        private const string PROLOG_SET_BOARD_VALUE_NAME = "set_board_value";
        private PrologEngine _swipl;

        public QueryHandler()
        {
            _swipl = new PrologEngine(persistentCommandHistory : false);
            _swipl.Consult(PROLOG_FILENAME);
            SetBoardValues();
            var lst = new List<BoardValues>();
            lst.Add((BoardValues)0);
            lst.Add((BoardValues)1);
            lst.Add((BoardValues)2);
            IBoard curr = new StandardBoard(1, 2, lst); 
            NextBoard(curr);
        }

        public IBoard NextBoard(IBoard currBoard)
        {
            ExecuteQuery(QueryFormat(true, "next_board", currBoard.ToString(), "X"));
            ProcessSolutions
            (
                val => 
                {
                    Debug.Log(val);
                }
            );
            return null;
        }

        private IBoard parseBoard(string str)
        {
            return null;
        }

        private void ProcessSolutions(SolutionValuesProcessor processor)
        {
            foreach (var solution in _swipl.SolutionIterator)
            {
                foreach (var f in solution.VarValuesIterator)
                {
                    var strRep = f.Value.ToString();
                    processor(strRep);
                }
            }
        }

        private void ExecuteQuery(string query) => _swipl.Query = query;

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
                    PROLOG_SET_BOARD_VALUE_NAME, 
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

            return res;
        }

        public bool IsFinalBoard()
        {
            throw new NotImplementedException();
        }
    }
}
