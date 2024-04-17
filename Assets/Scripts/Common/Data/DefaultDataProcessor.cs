using UnityEngine;

namespace VirtualLaboratory
{
    public abstract class DefaultDataProcessor : VariantСhooser
    {
        [SerializeField] protected GameObject _blockPanel;

        [Header("MODULES")] 
        [SerializeField] protected ProcessModule[] _modules;
        [SerializeField] protected ResultView _resultView;
        
        protected Graph _graph;
        protected ProcessModule _currentModule;

        public void Init(Graph graph)
        {
            _graph = graph;
            Init();
        }

        public override void Init()
        {
            _resultView.Init();
            CreateButtons(_modules.Length);
            
            _buttons[0].Enable();
            _currentModule = _modules[0];
            _blockPanel.SetActive(true);

            foreach (ProcessModule module in _modules)
                module.Init(_graph, _resultView);
        }

        public void EnableProcessing()
        {
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

        protected abstract void EnableCurrentModule();

        protected override void OnClickedVariant(VariantButton buttonCall)
        {
            base.OnClickedVariant(buttonCall);
            EnableCurrentModule();
        }

        protected override void SetVariant(int index) => _currentModule = _modules[index];
        
        protected override void OnCreateButton(int index) => _buttons[index].Init(_modules[index], OnClickedVariant);
    }
}