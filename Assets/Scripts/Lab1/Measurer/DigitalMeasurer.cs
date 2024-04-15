using TMPro;
using UnityEngine;

namespace VirtualLaboratory
{
    public class DigitalMeasurer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valueText;
        
        private float _minValue, _maxValue;
        
        public void Init()
        {
            _valueText.text = "0.0";
        }

        public void SetValue(float value)
        {
            _valueText.text = value.ToString("F2");
        }
    }
}