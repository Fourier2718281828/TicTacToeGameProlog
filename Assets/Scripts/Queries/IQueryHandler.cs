using App.Logic_Components.Boards;

namespace App.Queries
{
    public interface IQueryHandler
    {
        public IBoard NextBoard(IBoard currBoard);
        public bool IsFinalBoard();
    }
}
