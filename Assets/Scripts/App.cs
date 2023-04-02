using UnityEngine;
using App.Queries;

namespace App
{
    public class App : MonoBehaviour
    {
        public IQueryHandler QueryHandler { get; private set; }

        private void Start() 
        {
            QueryHandler = new QueryHandler();
        }
    }
}
