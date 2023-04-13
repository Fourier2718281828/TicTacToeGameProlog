using System;
using App.Logic_Components;
using App.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace World.UI.Board.CellKinds
{
    public class StandardCell : MonoBehaviour, IRenderableCell
    {
        private CellValue _value;
        private CellState _state;

        [SerializeField] private MouseInteractable _shapeMouseInteractable;

        [SerializeField] private Image _backImage;
        [SerializeField] private Image _shapeImage;

        [SerializeField] private Sprite EMPTY_SHAPE;
        [SerializeField] private Sprite CROSS_SHAPE;
        [SerializeField] private Sprite NAUGHT_SHAPE;

        [SerializeField] private Sprite DEFAULT_BACKGROUND;
        [SerializeField] private Sprite LOSS_BACKGROUND;
        [SerializeField] private Sprite WIN_BACKGROUND;

        #region IRenderableCell
        public CellValue Value 
        {
            get => _value;
            set
            {
                _value = value;
                SetCellValueShape(value);
            }
        }

        public CellState RenderedState 
        {
            get => _state; 
            set
            {
                _state = value;
                SetBackground(value);
            }
        }

        public GameObject GameObject => gameObject;

        private void SetBackground(CellState state)
        {
            _backImage.sprite = state switch
            {
                CellState.DEFAULT => DEFAULT_BACKGROUND,
                CellState.WIN_STATE => WIN_BACKGROUND,
                CellState.LOSS_STATE => LOSS_BACKGROUND,
                _ => throw new InvalidProgramException
                     ("The switch statement does not process all states."),
            };
        }

        private void SetCellValueShape(CellValue value)
        {
            _shapeImage.sprite = value switch
            {
                CellValue.EMPTY => EMPTY_SHAPE,
                CellValue.CROSS => CROSS_SHAPE,
                CellValue.NAUGHT => NAUGHT_SHAPE,
                _ => throw new InvalidProgramException
                     ("The switch statement does not process all states."),
            };
        }
        #endregion

        #region Mono Behaviour
        public void Awake()
        {
            _value = CellValue.EMPTY;
            _state = CellState.DEFAULT;
            var shapeGameObject = _shapeImage.gameObject;
            _shapeMouseInteractable = shapeGameObject.GetComponent<MouseInteractable>();
        }

        public void OnEnable()
        {
            _shapeMouseInteractable.OnMouseDownEvent += OnInteracted;
        }

        public void OnDisable()
        {
            _shapeMouseInteractable.OnMouseDownEvent -= OnInteracted;
        }
        #endregion

        private void OnInteracted()
        {
            Debug.Log("Interacted");
        }
    }
}
