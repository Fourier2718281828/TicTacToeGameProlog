using UnityEngine;
using App.Queries;
using App.Queries.Formatting;

namespace App
{
    public class App : MonoBehaviour
    {
        public IQueryHandler QueryHandler { get; private set; }
        public IBoardPrologFormatter Formatter { get; private set; }

        private void Start() 
        {
            Formatter = new ListFormatter();
            QueryHandler = new QueryHandler(Formatter);
        }
    }
}
