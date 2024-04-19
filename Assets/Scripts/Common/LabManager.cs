using UnityEngine;

namespace VirtualLaboratory
{
    public abstract class LabManager : MonoBehaviour
    {
        [Header("Default")]
        [SerializeField] protected DefaultVariantСhooser _variantChoiser;
        [SerializeField] protected CurrentSource _currentSource;
        [SerializeField] protected ControlButton _controlButton;
        [SerializeField] protected GameObject _blockPanel;
        
        private void Start()
        {
            Init();
        }

        protected abstract CurrentInput GetCurrentInput();

        protected virtual void Init()
        {
            _variantChoiser.Init();
            _blockPanel.gameObject.SetActive(true);
            _controlButton.Init(StartClicked, StopClicked);
            _currentSource.Init(GetCurrentInput());
        }

        protected virtual void StartClicked()
        {
            _variantChoiser.Disable();
            _currentSource.StartGauge();
            _blockPanel.gameObject.SetActive(false);
        }
        
        protected virtual void StopClicked()
        {
            _variantChoiser.Enable();
            _currentSource.Stop();
            _blockPanel.gameObject.SetActive(true);
        }
    }
}