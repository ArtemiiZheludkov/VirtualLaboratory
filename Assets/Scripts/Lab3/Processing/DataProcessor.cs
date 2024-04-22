using UnityEngine;

namespace VirtualLaboratory.Lab3
{
    public class DataProcessor : DefaultDataProcessor
    {
        [Header("Lab 3")]
        [SerializeField] private ProcessModule[] _noMagneticModules;
        [SerializeField] private ProcessModule[] _magneticModules;
        
        private float _currentIp;
        private DataContainer _data;

        public void Init(DataContainer data, Graph graph)
        {
            _data = data;
            _modules = _noMagneticModules;
            
            Init(graph);
        }

        public void EnableProcessing(float currentIp)
        {
            _currentIp = currentIp;
            EnableProcessing();
        }

        public void SetModules(bool isMagnetic)
        {
            if (isMagnetic == true)
                _modules = _magneticModules;
            else
                _modules = _noMagneticModules;
            
            CreateButtons(_modules.Length);
        }

        protected override void EnableCurrentModule()
        {
            string addName = "Ip = " + _currentIp.ToString("0.##") + " (mA)";
            _graph.SetAddNameY(addName);
            //_currentModule.Enable(_data.GetUzData(), _data.GetIzData(_currentIp));
        }
    }
}