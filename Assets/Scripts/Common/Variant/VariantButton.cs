using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLaboratory
{
    public class VariantButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image _enabled;

        private Action<VariantButton> _onClicked;

        public void Init(IVariant variant, Action<VariantButton> onClicked)
        {
            _onClicked = onClicked;
            _name.text = variant.ButtonName();
            
            Disable();
            
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(OnClicked);
        }

        public void Enable() => _enabled.gameObject.SetActive(true);

        public void Disable() => _enabled.gameObject.SetActive(false);

        private void OnClicked() =>  _onClicked?.Invoke(this);
    }
}