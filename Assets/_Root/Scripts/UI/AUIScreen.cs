namespace MineSweeper.UI
{
    using System;
    using UnityEngine;

    public abstract class AUIScreen : MonoBehaviour
    {
        public event Action OnShow;
        public event Action OnShown;
        public event Action OnHide;
        public event Action OnHidden;
        
        public bool IsShown { get; }
        
        public void Show()
        {
            OnShow?.Invoke();
            OnStartShow();
        }
        
        public void Close()
        {
            OnHide?.Invoke();
            OnStartClose();
        }
        
        protected virtual void OnStartShow()
        {
            
        }
        
        protected virtual void OnStartClose()
        {
            Destroy(gameObject);
        }
        
    }
}