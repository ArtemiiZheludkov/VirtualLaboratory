using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace VirtualLaboratory
{
    public class Noiser : MonoBehaviour
    {
        [Header("VALUES IN %")]
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;
        
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _minText;
        [SerializeField] private TMP_Text _maxText;
        [SerializeField] private TMP_Text _currentText;

        private float _currentNoise;
        private float _noise;

        private void OnEnable() => _slider.onValueChanged.AddListener(SetNoise);
        private void OnDisable() => _slider.onValueChanged.RemoveListener(SetNoise);

        public void Init()
        {
            _slider.minValue = _minValue;
            _slider.maxValue = _maxValue;
            _slider.value = _minValue;

            _minText.text = _minValue.ToString();
            _maxText.text = _maxValue.ToString();
            _currentText.text = $"{_minValue} %";
            
            _currentNoise = _minValue / 100f;
        }

        public float GetNoise(float data)
        {
            if (_currentNoise <= 0.001f)
                return 0f;
            
            float u1 = 1f - Random.value;
            float u2 = 1f - Random.value;
            float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
            float stdDev = data * _currentNoise;
            return stdDev * randStdNormal;
        }

        private void SetNoise(float value)
        {
            _currentNoise = value / 100f;
            _currentText.text = $"{value} %";
        }
    }
}