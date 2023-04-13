using UnityEngine;

namespace World
{
    public class ObjectsContainer : MonoBehaviour
    {
        #region Objects
        [SerializeField] private GameObject _cellsArea;
        [SerializeField] private GameObject _background;
        #endregion

        #region Selectors
        public GameObject CellsArea => _cellsArea;
        public GameObject Background => _background;
        #endregion
    }
}
