using UnityEngine;

namespace VirtualLaboratory.Lab3
{
    public class Lab3Manager : LabManager
    {
        [Header("Lab 3")]
        [SerializeField] private ExperimentController _experimentController;
        
        private DataContainer _dataContainer;
        
        protected override CurrentInput GetCurrentInput() => _experimentController;
        
        protected override void OnInit()
        {
            _dataContainer = new DataContainer();
        }

        protected override void OnStart()
        {
            _dataContainer.Init(_variantChoiser.VariantNumber);
            _experimentController.Init(_dataContainer, _currentSource, _variantChoiser.VariantNumber);
        } 
        
        protected override void OnStop()
        {
        }
    }
}