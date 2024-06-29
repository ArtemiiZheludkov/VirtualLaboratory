using TMPro;
using UnityEngine;

namespace VirtualLaboratory
{
    public class DigitalMeasurer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valueText;
        
        public void Init()
        {
            _valueText.text = "0.0";
        }

        public void SetValue(float value)
        {
            _valueText.text = value.ToString("#0.00");
        }
    }
}