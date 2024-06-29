using UnityEngine;

namespace VirtualLaboratory
{
    public class MenuManager : VariantСhooser
    {
        [SerializeField] private DefaultVariant[] _variants;
        [SerializeField] private LabManager[] _labManagers;
        [SerializeField] private GameObject[] _labCanvas;

        private int _variantNumber;

        private void Start()
        {
            Init();
        }

        public override void Init()
        {
            CreateButtons(_variants.Length);
            OnClickedVariant(_buttons[0]);

            _variantNumber = 0;

            foreach (LabManager manager in _labManagers)
                manager.Init(this);

            foreach (GameObject canvas in _labCanvas)
                canvas.SetActive(false);
            
            gameObject.SetActive(true);
        }
        
        protected override void OnClickedVariant(VariantButton buttonCall)
        {
            base.OnClickedVariant(buttonCall);
            
            Disable();
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            _labManagers[_variantNumber].SetStart();
            _labCanvas[_variantNumber].SetActive(false);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            _labCanvas[_variantNumber].SetActive(true);
        }

        protected override void SetVariant(int index) => _variantNumber = index;

        protected override void OnCreateButton(int index) => _buttons[index].Init(_variants[index], OnClickedVariant);
    }
}