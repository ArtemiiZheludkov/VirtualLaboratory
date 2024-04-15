using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLaboratory.Lab1
{
    public class MaterialButton : MonoBehaviour
    {
        public MaterialLab Material { get; private set; }

        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image _enabled;
        
        private MaterialSwitcher _switcher;

        public void Init(MaterialLab material, MaterialSwitcher switcher)
        {
            Material = material;
            _name.text = Material.Name;
            _switcher = switcher;
            
            _enabled.gameObject.SetActive(false);
            _button.onClick.AddListener(OnClicked);
        }

        public void Enable() => _enabled.gameObject.SetActive(true);

        public void Disable() => _enabled.gameObject.SetActive(false);

        private void OnClicked() => _switcher.SetMaterial(this);
    }
}