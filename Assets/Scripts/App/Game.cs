using App.Queries;
using UnityEngine;
using App.Logic_Components.Boards;
using App.Logic_Components;
using System.Linq;
using System.Collections.Generic;

namespace App
{
    public class Game : MonoBehaviour
    {
        public delegate void GameAction(CellValue winner);
        public event GameAction OnVictory;

        private IBoard _board;
        private IQueryHandler _queryHandler;

        private CellValue _currentPlayer;

        private void Awake()
        {
            _currentPlayer = CellValue.CROSS;
        }

        public void Init(IBoard board, IQueryHandler queryHandler)
        {
            SwitchToBoard(board);
            _queryHandler = queryHandler;
        }

        private void SwitchToBoard(IBoard board)
        {
            DetachEvents(_board);
            AttachEvents(board);
            _board = board;
        }

        private void AttachEvents(IBoard board)
        {
            if (board == null) return;
            foreach (var cell in board.Cells)
            {
                cell.OnInteraction += OnCellInteraction;
            }
        }

        private void DetachEvents(IBoard board)
        {
            if (board == null) return;
            foreach (var cell in board.Cells)
            {
                cell.OnInteraction -= OnCellInteraction;
            }
        }

        private IEnumerable<IEnumerable<CellValue>> VictorySequences()
        {
            return _queryHandler.VictorySequences(_board);
        }

        private CellValue? GetWinner(IEnumerable<IEnumerable<CellValue>> seqs)
        {
            var list = seqs.ToList();
            return list.Count == 0
                ? null
                : list[0].Take(1).ToList()[0];
        }

        private void ComputersTurn()
        {
            int? index = _queryHandler.NextBoard(_board, _currentPlayer);
            if (index == null) return;
            _board[(int)index].Value = CellValue.NAUGHT;
            if (CheckVictory()) return;
        }

        private bool CheckVictory()
        {
            var vicSeqs = VictorySequences();
            var winner = GetWinner(vicSeqs);

            if (winner != null)
            {
                OnVictory?.Invoke((CellValue)winner);
                return true;
            }

            return false;
        }

        private void OnCellInteraction(ICell cell)
        {
            if(cell.Value == CellValue.EMPTY && _currentPlayer == CellValue.CROSS) //Non-computer's player cell value
            {
                cell.Value = CellValue.CROSS;
                if (CheckVictory()) return;
                //_currentPlayer = CellValue.NAUGHT;
                //ComputersTurn();
            }
        }
    }
}
