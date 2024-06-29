using System;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLaboratory
{
    public class ControlButton : MonoBehaviour
    {
        [SerializeField] private Button _start;
        [SerializeField] private Button _stop;

        private Action OnStartClicked; 
        private Action OnStopClicked;

        public virtual void Init(Action onStartClicked, Action onStopClicked)
        {
            OnStartClicked = onStartClicked;
            OnStopClicked = onStopClicked;
            
            _start.onClick.RemoveAllListeners();
            _stop.onClick.RemoveAllListeners();
            
            _start.onClick.AddListener(StartClicked);
            _stop.onClick.AddListener(StopClicked);
        }
        
        public void SetStart() => StopClicked();
        public void SetStop() => StartClicked();

        public virtual void EnableInteractivity()
        {
            _start.interactable = true;
            _stop.interactable = true;
        }
        
        public virtual void DisableInteractivity()
        {
            _start.interactable = false;
            _stop.interactable = false;
        }
        
        protected virtual void StartClicked()
        {
            _start.gameObject.SetActive(false);
            _stop.gameObject.SetActive(true);
            
            OnStartClicked?.Invoke();
        }
        
        protected virtual void StopClicked()
        {
            _start.gameObject.SetActive(true);
            _stop.gameObject.SetActive(false);
            
            OnStopClicked?.Invoke();
        }
    }
}