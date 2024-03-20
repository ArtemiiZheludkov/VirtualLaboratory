using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLaboratory
{
    public class CurrentSource : MonoBehaviour
    {
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;
        [Min(1)][SerializeField] private float _denominator;
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _maxI;
        [SerializeField] private ScaleMeasurer _ammeter;
        [SerializeField] private CurrentInput _output;

        private float _currentI;
        
        public void Init()
        {
            _currentI = _minValue / _denominator;
            _ammeter.Init(_minValue / _denominator, _maxValue / _denominator);
            _output.SetCurrent(_currentI);
            
            _maxI.text = (_maxValue / _denominator).ToString("F1");
            _slider.minValue = _minValue;
            _slider.maxValue = _maxValue;
            _slider.value = _minValue;
        }
        
        public void StartGauge()
        {
            _slider.value = _minValue;
            SetI();
        }

        public void Stop()
        {
            _slider.value = _minValue;
            SetI();
        }
        
        public void OnSliderMove() => SetI();

        private void SetI()
        {
            _currentI = _slider.value / _denominator;

            if (_currentI < 0.1f)
                _currentI = 0f;
            
            _ammeter.SetValue(_currentI);
            _output.SetCurrent(_currentI);
        }
    }
}