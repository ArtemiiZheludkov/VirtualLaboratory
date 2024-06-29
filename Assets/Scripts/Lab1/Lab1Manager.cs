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
        
        protected override void OnInit()
        {
            _variantChoiser.Init(_materials);
            _dataContainer = new DataContainer();
            Debug.Log("Lab1 init");
        }

        protected override void OnStart()
        {
            MaterialLab material = _materials[_variantChoiser.VariantNumber - 1];
            material.Init(_starT);
            _dataContainer.LoadData(material, _starT, _stepT, _stopT);
            _experimentController.Init(_dataContainer, material, _starT, _stepT, _stopT);
        }

        protected override void OnStop()
        {
            _experimentController.Stop();
        }
    }
}