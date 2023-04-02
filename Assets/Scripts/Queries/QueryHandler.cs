using App.Logic_Components.Boards;
using Prolog;

namespace App.Queries
{
    public class QueryHandler : IQueryHandler
    {
        private PrologEngine _swipl;

        public QueryHandler() => _swipl = new PrologEngine();

        public IBoard NextBoard()
        {
            throw new System.NotImplementedException();
        }
    }
}
