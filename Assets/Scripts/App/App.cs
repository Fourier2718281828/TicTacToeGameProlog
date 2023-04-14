using UnityEngine;
using App.Queries;
using App.Queries.Formatting;
using World;
using World.UI.Board;

namespace App
{
    public class App : MonoBehaviour
    {
        #region Inner Components
        public IQueryHandler QueryHandler { get; private set; }
        public IPrologFormatter Formatter { get; private set; }
        #endregion

        #region External Components
        [SerializeField] private Game _game;
        [SerializeField] private ObjectsContainer _objectsContainer;
        #endregion

        private void Start() 
        {
            Formatter = new ListFormatter();
            QueryHandler = new CmdQueryHandler(Formatter);//new QueryHandler(Formatter);
            var cellArea = _objectsContainer.CellsArea.GetComponent<CellsArea>();
            _game.Init(cellArea.Board, QueryHandler);
        }
    }
}
