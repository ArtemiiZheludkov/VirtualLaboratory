using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class DataProcessor : VariantСhooser
    {
        [SerializeField] private GameObject _blockPanel;

        [Header("MODULES")] 
        [SerializeField] private ProcessModule[] _modules;
        [SerializeField] private ResultView _resultView;

        private DataContainer _data;
        private Graph _graph;
        private ProcessModule _currentModule;
        private float _currentIp;

        public void Init(DataContainer data, Graph graph)
        {
            _data = data;
            _graph = graph;
            
            _resultView.Init();
            CreateButtons(_modules.Length);
            
            _buttons[0].Enable();
            _currentModule = _modules[0];
            _blockPanel.SetActive(true);

            foreach (ProcessModule module in _modules)
                module.Init(_graph, _resultView);
        }

        public void EnableProcessing(float currentIp)
        {
            _currentIp = currentIp;
            OnClickedVariant(_buttons[0]);
            _blockPanel.SetActive(false);
        }

        public void DisableProcessing()
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].Disable();
                _modules[i].Disable();
            }
            
            _buttons[0].Enable();
            _currentModule = _modules[0];
            _blockPanel.SetActive(true);
        }

        protected override void OnClickedVariant(VariantButton buttonCall)
        {
            base.OnClickedVariant(buttonCall);
            _currentModule.Enable(_data.GetUzData(), _data.GetIzData(_currentIp), _currentIp);
        }

        protected override void SetVariant(int index) => _currentModule = _modules[index];
        
        protected override void OnCreateButton(int index) => _buttons[index].Init(_modules[index], OnClickedVariant);
    }
}