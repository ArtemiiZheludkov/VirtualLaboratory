using TMPro;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class VariantChoiser : MonoBehaviour
    {
        [Header("MATERIAL SETTINGS")]
        [SerializeField] private Variant[] _variants;
        [SerializeField] private VariantButton _buttonPrefab;
        [SerializeField] private Transform _buttonsParent;
        [SerializeField] private TMP_Text _variantText;

        private Variant _variant;
        private VariantButton[] _buttons;
        
        public void Init()
        {
            CreateButtons();
            SetVariant(_buttons[0]);
            Enable();
        }

        public void Enable()
        {
            _variantText.gameObject.SetActive(false);
        }

        public void Disable()
        {
            _variantText.gameObject.SetActive(true);
            _variantText.text = _variant.FullName;
        }

        public void SetVariant(VariantButton buttonCall)
        {
            foreach (VariantButton button in _buttons)
                button.Disable();
            
            buttonCall.Enable();
            _variant = buttonCall.Variant;
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