using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLaboratory
{
    public class CurrentSource : MonoBehaviour
    {
        [SerializeField] private float _minValue = 0f;
        [SerializeField] private float _maxValue;
        [Min(0.01f)][SerializeField] private float _denominator;
        [SerializeField] private float _offset = 0f;
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _maxI;
        [SerializeField] private ScaleMeasurer _ammeter;
        [SerializeField] private CurrentInput _output;

        private float _currentI;

        public void Init(CurrentInput output)
        {
            _output = output;
            Init();
        }
        
        public void Init()
        {
            _currentI = _minValue / _denominator;
            _ammeter.Init(_minValue / _denominator, (_maxValue / _denominator) + _offset);
            _output.SetCurrent(_currentI);
            
            _maxI.text = ((_maxValue / _denominator) + _offset).ToString("0.##");
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

        public void EnableInteractivity()
        {
            _slider.interactable = true;
        }
        
        public void DisableInteractivity()
        {
            _slider.interactable = false;
        }

        public void SetValue(float value)
        {
            _slider.value = (value * _denominator) + _offset;
        }

        public void OnSliderMove() => SetI();

        private void SetI()
        {
            _currentI = (_slider.value / _denominator) + _offset;

            if (_currentI < 0.1f)
                _currentI = 0f;
            
            _ammeter.SetValue(_currentI);
            _output.SetCurrent(_currentI);
        }
    }
}