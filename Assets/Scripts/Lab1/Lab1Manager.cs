using UnityEngine;

namespace VirtualLaboratory.Lab1
{
    public class Lab1Manager : LabManager
    {
        [Header("Lab 1")]
        [SerializeField] private ExperimentController _experimentController;
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
        }

        protected override void StartClicked()
        {
            base.StartClicked();

            MaterialLab material = _materials[_variantChoiser.VariantNumber - 1];
            material.Init(_starT);
            _dataContainer.LoadData(material, _starT, _stepT, _stopT);
            _experimentController.Init(_dataContainer, material, _starT, _stepT, _stopT);
        }

        protected override void StopClicked()
        {
            base.StopClicked();
            _experimentController.Stop();
        }
    }
}