using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLaboratory.Lab2
{
    public class ModuleButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image _enabled;
        
        private DataProcessor _processor;
        
        public void Init(ProcessModule module, DataProcessor processor)
        {
            _processor = processor;
            _name.text = module.Name;
            
            Disable();
            
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(OnClicked);
        }

        public void Enable() => _enabled.gameObject.SetActive(true);

        public void Disable() => _enabled.gameObject.SetActive(false);

        private void OnClicked() =>  _processor.SetModule(this);
    }
}