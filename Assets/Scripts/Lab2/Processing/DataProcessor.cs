using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class DataProcessor : MonoBehaviour
    {
        [SerializeField] private ModuleButton _moduleButtonPrefab;
        [SerializeField] private Transform _buttonsParent;
        [SerializeField] private GameObject _blockPanel;

        [Header("MODULES")] 
        [SerializeField] private ProcessModule[] _modules;
        
        // Result viewer (text)

        private DataContainer _data;
        private Graph _graph;
        
        private ModuleButton[] _buttons;
        private ProcessModule _currentModule;

        private float _currentIp;

        public void Init(DataContainer data, Graph graph)
        {
            _data = data;
            _graph = graph;
            
            if (_buttons != null)
            {
                foreach (ModuleButton obj in _buttons)
                    Destroy(obj.gameObject);
                
                _buttons = null;
            }
            
            CreateButtons();
            
            _buttons[0].Enable();
            _currentModule = _modules[0];

            foreach (ProcessModule module in _modules)
                module.Init(_graph);
            
            _blockPanel.SetActive(true);
        }

        public void EnableProcessing(float currentIp)
        {
            _currentIp = currentIp;
            SetModule(_buttons[0]);
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

        public void SetModule(ModuleButton buttonCall)
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                if (ReferenceEquals(_buttons[i], buttonCall) == true)
                    _currentModule = _modules[i];
                else
                    _buttons[i].Disable();
            }
            
            buttonCall.Enable();
            EnableModule(_currentModule);
        }

        private void EnableModule(ProcessModule module)
        {
            module.Enable(_data.GetUzData(), _data.GetIzData(_currentIp), _currentIp);
        }

        private void CreateButtons()
        {
            _buttons = new ModuleButton[_modules.Length];
            
            for (int i = 0; i < _buttons.Length; i++)
            {
                ModuleButton button = Instantiate(_moduleButtonPrefab, _buttonsParent);
                button.Init(_modules[i], this);
                _buttons[i] = button;
            }
        }
    }
}