using UnityEngine;
using UnityEngine.UI;

namespace VirtualLaboratory
{
    public abstract class LabManager : MonoBehaviour
    {
        [Header("Default")]
        [SerializeField] protected DefaultVariantСhooser _variantChoiser;
        [SerializeField] protected CurrentSource _currentSource;
        [SerializeField] protected ControlButton _controlButton;
        [SerializeField] protected GameObject _blockPanel;
        [SerializeField] private Button _homeButton;

        private MenuManager _menu;

        public void Init(MenuManager menuManager)
        {
            _menu = menuManager;
            
            _variantChoiser.Init();
            _blockPanel.gameObject.SetActive(true);
            _controlButton.Init(StartClicked, StopClicked);
            _currentSource.Init(GetCurrentInput());
            
            _homeButton.onClick.RemoveAllListeners();
            _homeButton.onClick.AddListener(OnHomeClicked);
            
            OnInit();
        }

        public void SetStart() => _controlButton.SetStart();
        
        private void StartClicked()
        {
            _variantChoiser.Disable();
            _currentSource.StartGauge();
            _blockPanel.gameObject.SetActive(false);
            OnStart();
        }
        
        private void StopClicked()
        {
            _variantChoiser.Enable();
            _currentSource.Stop();
            _blockPanel.gameObject.SetActive(true);
            OnStop();
        }

        private void OnHomeClicked() => _menu.Enable();
        
        protected abstract CurrentInput GetCurrentInput();
        protected abstract void OnInit();
        protected abstract void OnStart();
        protected abstract void OnStop();
    }
}