using System.Collections.Generic;

namespace App.Logic_Components.Boards
{
    public interface IBoard
    {
        public BoardValues this[int index] { get; }
        public bool IsFinal();
    }
}
