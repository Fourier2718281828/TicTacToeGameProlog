﻿using App.Logic_Components;
using App.Logic_Components.Boards;
using System.Collections.Generic;

namespace App.Queries
{
    public interface IQueryHandler
    {
        public int? NextBoard(IBoard currBoard, CellValue currentPlayer);
        public IEnumerable<IEnumerable<CellValue>> VictorySequences(IBoard currBoard);
    }
}
