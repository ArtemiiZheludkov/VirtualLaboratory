using UnityEngine;

namespace VirtualLaboratory.Lab1
{
    public class Lab1Manager : LabManager
    {
        [Header("Lab 1")]
        [SerializeField] private ExperimentController _experimentController;
        [SerializeField] private Heater _heater;
        [SerializeField] private MaterialLab[] _materials;
        
        [Header("Temperature settings")]
        [SerializeField] private float _starT;
        [SerializeField] private float _stepT;
        [SerializeField] private float _stopT;
        
        private DataContainer _dataContainer;

        protected override CurrentInput GetCurrentInput() => _experimentController;
        
        protected override void Init()
        {
            base.Init();
            
            _variantChoiser.Init(_materials);
            _dataContainer = new DataContainer();
            
            _heater.Init();
        }

        protected override void StartClicked()
        {
            base.StartClicked();
            
            _materials[_variantChoiser.VariantNumber].Init(_starT);
            _dataContainer.LoadData(_materials[_variantChoiser.VariantNumber], _starT, _stepT, _stopT);
            _experimentController.Init(_dataContainer);
            
            _heater.StartGauge();
        }

        protected override void StopClicked()
        {
            base.StopClicked();
            _heater.Stop();
        }
    }
}