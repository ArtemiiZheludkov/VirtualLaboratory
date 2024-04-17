using UnityEngine;

namespace VirtualLaboratory
{
    public abstract class LabManager : MonoBehaviour
    {
        [Header("Default")]
        [SerializeField] protected DefaultVariantСhooser _variantChoiser;
        [SerializeField] protected CurrentSource _currentSource;
        [SerializeField] protected GameObject _blockPanel;
        
        private void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
            _variantChoiser.Init();
            _blockPanel.gameObject.SetActive(true);
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