using UnityEngine;

namespace VirtualLaboratory.Lab1
{
    public class DataProcessor : DefaultDataProcessor
    {
        [Header("Lab 1")]
        [SerializeField] private ProcessModule[] _metalModules;
        [SerializeField] private ProcessModule[] _semiconductorModules;
        
        private DataContainer _data;

        public void Init(MaterialType type, DataContainer data, Graph graph)
        {
            if (type == MaterialType.Metal)
                _modules = _metalModules;
            else if (type == MaterialType.Semiconductor)
                _modules = _semiconductorModules;
            
            _data = data;
            Init(graph);
        }

        protected override void EnableCurrentModule()
        {
            _currentModule.Enable(_data.GetXData(), _data.GetYData());
        }
    }
}