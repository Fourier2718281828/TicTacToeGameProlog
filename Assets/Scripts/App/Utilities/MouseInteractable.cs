using UnityEngine;

namespace App.Utilities
{
    public class MouseInteractable : MonoBehaviour
    {
        public delegate void OnMouseInteracted();
        public event OnMouseInteracted OnMouseDownEvent;
        public event OnMouseInteracted OnMouseEnterEvent;
        public event OnMouseInteracted OnMouseExitEvent;
        public event OnMouseInteracted OnMouseOverEvent;

        public void OnMouseDown()   => OnMouseDownEvent?.Invoke();

        public void OnMouseEnter()  => OnMouseEnterEvent?.Invoke();
        
        public void OnMouseExit()   => OnMouseExitEvent?.Invoke();

        public void OnMouseOver()   => OnMouseOverEvent?.Invoke();
    }
}
