using DG.Tweening;
using TMPro;
using UnityEngine;

namespace VirtualLaboratory
{
    public class ScaleMeasurer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private TMP_Text[] _scaleTexts;

        [Space(10), Header("POINTER")]
        [SerializeField] private RectTransform _pointer;
        [SerializeField] private float _maxAngle;
        
        private float _minValue, _maxValue;
        
        public void Init(float minValue, float maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            
            float value = minValue;
            _valueText.text = value.ToString("0.##");
            
            for (int i = 0; i < _scaleTexts.Length; i++)
            {
                _scaleTexts[i].text = value.ToString("0.##");
                value += (maxValue - minValue) / (_scaleTexts.Length - 1);
            }
            
            _pointer.Rotate(0f,0f,0f);
        }

        public void SetValue(float value)
        {            
            _valueText.text = value.ToString("0.##");
            
            float z = value / _maxValue;

            if (z < 0f)
                z = 0f;
            else if (z > 1f)
                z = 1f;

            z *= _maxAngle;
            
            _pointer.DOLocalRotate(new Vector3(0f, 0f, z), 0.25f);
        }
    }
}