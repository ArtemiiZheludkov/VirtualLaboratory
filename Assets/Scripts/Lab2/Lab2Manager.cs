using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class Lab2Manager : LabManager
    {
        [Header("Lab 2")]
        [SerializeField] private ExperimentController _experimentController;
        [SerializeField] private ControlButton _controlButton;
        
        private DataContainer _dataContainer;
        
        protected override void Init()
        {
            base.Init();
            
            _dataContainer = new DataContainer();
            _controlButton.Init(StartClicked, StopClicked);
            _currentSource.Init(_experimentController);
        }

        protected override void StartClicked()
        {
            base.StartClicked();
            
            _dataContainer.Init(_variantChoiser.VariantNumber);
            _experimentController.Init(_dataContainer);
        }
    }
}