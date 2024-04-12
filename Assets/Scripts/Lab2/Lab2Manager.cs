using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class Lab2Manager : MonoBehaviour
    {
        [SerializeField] private DefaultVariantСhooser _variantChoiser;
        [SerializeField] private ExperimentController _experimentController;
        [SerializeField] private CurrentSource _currentSource;
        
        [Header("MENU")]
        [SerializeField] private GameObject _blockPanel;
        [SerializeField] private ControlButton _controlButton;
        
        private DataContainer _dataContainer;
        
        private void Start()
        {
            _variantChoiser.Init();
            _dataContainer = new DataContainer();
            _blockPanel.gameObject.SetActive(true);
            _controlButton.Init(StartClicked, StopClicked);
            _currentSource.Init(_experimentController);
        }

        private void StartClicked()
        {
            _variantChoiser.Disable();
            _dataContainer.Init(_variantChoiser.Variant);
            _experimentController.Init(_dataContainer);
            _blockPanel.gameObject.SetActive(false);
        }
        
        private void StopClicked()
        {
            _variantChoiser.Enable();
            _blockPanel.gameObject.SetActive(true);
        }
    }
}