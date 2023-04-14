using App.Queries;
using UnityEngine;
using App.Logic_Components.Boards;
using App.Logic_Components;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        private void OnDisable()
        {
            DetachEvents(_board);
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

        private async Task<IEnumerable<IEnumerable<CellValue>>> VictorySequences()
        {
            return await _queryHandler.VictorySequences(_board);
        }

        private CellValue? GetWinner(IEnumerable<IEnumerable<CellValue>> seqs)
        {
            var list = seqs.ToList();
            return list.Count == 0
                ? null
                : list[0].Take(1).ToList()[0];
        }

        private async void ComputersTurn()
        {
            int? index = await _queryHandler.NextBoard(_board, _currentPlayer);
            if (index == null)
            {
                var nxtCell = _board.Cells.FirstOrDefault(el => el.Value == CellValue.EMPTY);
                if (nxtCell == null) return;
                nxtCell.Value = CellValue.NAUGHT;
            }
            else
            {
                _board[(int)index].Value = CellValue.NAUGHT;
            }
            if (await CheckVictory()) return;
            _currentPlayer = CellValue.CROSS;
        }

        private async Task<bool> CheckVictory()
        {
            var vicSeqs = await VictorySequences();
            var winner = GetWinner(vicSeqs);

            if (winner != null)
            {
                _currentPlayer = CellValue.EMPTY;
                OnVictory?.Invoke((CellValue)winner);
                return true;
            }

            return false;
        }

        private async void OnCellInteraction(ICell cell)
        {
            if(cell.Value == CellValue.EMPTY && _currentPlayer == CellValue.CROSS) //Non-computer's player cell value
            {
                cell.Value = CellValue.CROSS;
                if (await CheckVictory()) return;
                _currentPlayer = CellValue.NAUGHT;
                ComputersTurn();
            }
        }
    }
}
