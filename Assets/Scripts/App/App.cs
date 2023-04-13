using UnityEngine;
using App.Queries;
using App.Queries.Formatting;
using World;

namespace App
{
    public class App : MonoBehaviour
    {
        #region Inner Components
        public IQueryHandler QueryHandler { get; private set; }
        public IBoardPrologFormatter Formatter { get; private set; }
        #endregion

        #region External Components
        [SerializeField] private ObjectsContainer _objectsContainer;
        #endregion

        private void Start() 
        {
            Formatter = new ListFormatter();
            QueryHandler = new QueryHandler(Formatter);
        }
    }
}
