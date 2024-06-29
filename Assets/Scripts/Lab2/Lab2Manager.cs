using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class Lab2Manager : LabManager
    {
        [Header("Lab 2")]
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
            _experimentController.Init(_dataContainer);
        }

        protected override void OnStop()
        {
        }
    }
}