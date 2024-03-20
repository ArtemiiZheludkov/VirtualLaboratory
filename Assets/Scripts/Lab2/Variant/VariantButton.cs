using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLaboratory.Lab2
{
    public class VariantButton : MonoBehaviour
    {
        public Variant Variant { get; private set; }
        
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image _enabled;
        
        private VariantChoiser _choiser;

        public void Init(Variant variant, VariantChoiser choiser)
        {
            Variant = variant;
            _choiser = choiser;

            _name.text = Variant.ButtonName;
            _enabled.gameObject.SetActive(false);
            _button.onClick.AddListener(OnClicked);
        }

        public void Enable() => _enabled.gameObject.SetActive(true);

        public void Disable() => _enabled.gameObject.SetActive(false);

        private void OnClicked() =>  _choiser.SetVariant(this);
    }
}