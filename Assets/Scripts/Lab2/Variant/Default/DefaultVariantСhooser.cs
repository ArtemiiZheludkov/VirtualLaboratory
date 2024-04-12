using TMPro;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class DefaultVariantСhooser : VariantСhooser
    {
        public int Variant { get; private set; }

        [Header("MATERIAL SETTINGS")]
        [SerializeField] private DefaultVariant[] _variants;
        [SerializeField] private TMP_Text _variantText;

        public void Init()
        {
            CreateButtons(_variants.Length);
            OnClickedVariant(_buttons[0]);
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
            _variantText.text = _variants[Variant - 1].FullName();
        }

        protected override void SetVariant(int index) => Variant = index + 1;

        protected override void OnCreateButton(int index) => _buttons[index].Init(_variants[index], OnClickedVariant);
    }
}