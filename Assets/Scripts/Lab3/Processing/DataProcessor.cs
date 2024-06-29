using TMPro;
using UnityEngine;

namespace VirtualLaboratory.Lab3
{
    public class DataProcessor : DefaultDataProcessor
    {
        [Header("Lab 3")] 
        [SerializeField] private TMP_Text _addText;
        [SerializeField] private ProcessModule[] _noMagneticModules;
        [SerializeField] private ProcessModule[] _magneticModules;
        
        private DataContainer _data;
        private int _currentIz;
        private bool _magneticActivated;

        public void Init(DataContainer data, Graph graph, int variant)
        {
            _data = data;
            _modules = _noMagneticModules;
            
            Init(graph);
            
            _addText.text = $"a={data.a} m; c={data.c} m; l=d={data.d} m;";
        }

        public void EnableProcessing(int currentIz)
        {
            _currentIz = currentIz;
            DataContainer.Instance.currentIz = _currentIz;
            EnableProcessing();
        }

        public void SetModules(bool magneticActivated)
        {
            _magneticActivated = magneticActivated;
            
            if (_magneticActivated == true)
                _modules = _magneticModules;
            else
                _modules = _noMagneticModules;
            
            CreateButtons(_modules.Length);
            _buttons[0].Enable();
            _currentModule = _modules[0];
            _blockPanel.SetActive(true);

            foreach (ProcessModule module in _modules)
                module.Init(_graph, _resultView);
        }

        protected override void EnableCurrentModule()
        {
            if (_magneticActivated == true)
            {
                ref MagneticData data = ref _data.GetMagneticData(_currentIz);
                float[] U = new float[data.I.Length];

                for (int i = 0; i < U.Length; i++)
                    U[i] = (data.UPlus[i] - data.UMinus[i]) / 2f;
                
                _currentModule.Enable(data.B, U);
            }
            else
            {
                float[] U = new float[_data.Ip.Length];

                for (int i = 0; i < U.Length; i++)
                    U[i] = ( _data.Uplus[i] - _data.Uminus[i]) / 2f;
                
                _currentModule.Enable(U, _data.Ip);
            }
        }
    }
}