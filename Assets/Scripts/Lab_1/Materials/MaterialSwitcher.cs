using UnityEngine;
using UnityEngine.UI;

namespace VirtualLaboratory
{
    public class MaterialSwitcher : MonoBehaviour
    {
        [Header("MATERIAL SETTINGS")]
        [SerializeField] private MaterialLab[] _materials;
        [SerializeField] private MaterialButton _buttonPrefab;
        [SerializeField] private Transform _buttonsParent;
        
        [Header("Icons")]
        [SerializeField] private Image _metalIcon;
        [SerializeField] private Image _semiconductorIcon;
        
        [Header("Main")]
        [SerializeField] private Thermostat _thermostat;

        private MaterialLab _material;
        private MaterialButton[] _buttons;
        
        public void Init()
        {
            CreateButtons();
            SetMaterial(_buttons[0]);
            _thermostat.SetMaterial(_material);
            Enable();
        }

        public void Enable()
        {
            _metalIcon.gameObject.SetActive(false);
            _semiconductorIcon.gameObject.SetActive(false);
        }

        public void Disable()
        {
            if (_material.Type == MaterialType.Metal)
                _metalIcon.gameObject.SetActive(true);
            else if (_material.Type == MaterialType.Semiconductor)
                _semiconductorIcon.gameObject.SetActive(true);
        }

        public void SetMaterial(MaterialButton buttonCall)
        {
            foreach (MaterialButton button in _buttons)
                button.Disable();
            
            buttonCall.Enable();
            
            _material = buttonCall.Material;
            _thermostat.SetMaterial(_material);
        }

        public void SetStartTemperature(float _startT)
        {
            foreach (MaterialLab material in _materials)
                material.Init(_startT);
        }

        private void CreateButtons()
        {
            _buttons = new MaterialButton[_materials.Length];

            for (int i = 0; i < _materials.Length; i++)
            {
                _buttons[i] = Instantiate(_buttonPrefab, _buttonsParent, false);
                _buttons[i].gameObject.SetActive(true);
                _buttons[i].Init(_materials[i], this);
            }
        }
    }
}