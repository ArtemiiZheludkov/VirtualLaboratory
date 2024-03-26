using TMPro;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class VariantChoiser : MonoBehaviour
    {
        public int Variant { get; private set; }

        [Header("MATERIAL SETTINGS")]
        [SerializeField] private Variant[] _variants;
        [SerializeField] private VariantButton _buttonPrefab;
        [SerializeField] private Transform _buttonsParent;
        [SerializeField] private TMP_Text _variantText;
        
        private VariantButton[] _buttons;

        public void Init()
        {
            CreateButtons();
            SetVariant(_buttons[0]);
            Enable();
        }

        public void Enable()
        {
            foreach (VariantButton variant in _buttons)
                variant.gameObject.SetActive(true);
            
            _variantText.gameObject.SetActive(false);
        }

        public void Disable()
        {
            foreach (VariantButton variant in _buttons)
                variant.gameObject.SetActive(false);
            
            _variantText.gameObject.SetActive(true);
            _variantText.text = _variants[Variant - 1].FullName;
        }

        public void SetVariant(VariantButton buttonCall)
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                if (ReferenceEquals(_buttons[i], buttonCall) == true)
                    Variant = i + 1;
                else
                    _buttons[i].Disable();
            }

            buttonCall.Enable();
        }

        private void CreateButtons()
        {
            _buttons = new VariantButton[_variants.Length];

            for (int i = 0; i < _variants.Length; i++)
            {
                _buttons[i] = Instantiate(_buttonPrefab, _buttonsParent, false);
                _buttons[i].gameObject.SetActive(true);
                _buttons[i].Init(_variants[i], this);
            }
        }
    }
}