using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLaboratory
{
    public class Heater : MonoBehaviour
    {
        [Header("Heat"), Space(5)] 
        [SerializeField] private float _startT;
        [SerializeField] private float _maxT;
        [SerializeField] private float _stepT;
        [SerializeField] private float _stepTUpdate;
        [SerializeField] private float _timeUpdate;
        
        [Header("UI")]
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _maxTedc;
        [SerializeField] private TMP_Text _TedcText;
        [SerializeField] private TMP_Text _curText;
        [SerializeField] private Thermostat _output;
        [SerializeField] private MaterialSwitcher _switcher;

        private float _currentT, _targetT;
        private bool _isHeat = false;

        public void Init()
        {
            _isHeat = false; 
            _currentT = _startT;
            _targetT = _currentT;
            _switcher.SetStartTemperature(_startT);
            _output.Init(_startT);
            
            _slider.minValue = 0f;
            _slider.maxValue = (_maxT - _startT) / _stepT;
            _slider.value = 0f;

            _maxTedc.text = (_slider.maxValue / 10f).ToString("F1");
            _curText.text = (_slider.value / 10f).ToString("F1");
            
            SetTedcText();
        }

        public void StartGauge()
        {
            _isHeat = true;
            _slider.value = 0f;
            _currentT = _startT;
            _targetT = _currentT;
            
            SetTedcText();
            _output.SetTemperature(_currentT);
            _output.StartGauge(_maxT);
            
            StartCoroutine(Heat());
        }

        public void Stop()
        {
            _isHeat = false;
            _slider.value = 0f;
        }

        public void OnSliderMove()
        {
            _targetT = _startT + (_stepT * _slider.value);
            _curText.text = ((_slider.value / 10f) - 0.01f).ToString("F1");
        }

        private void SetTedcText()
        {
            _TedcText.text = (((_currentT - _startT) / _stepT) / 10f).ToString("F2");
        }

        private IEnumerator Heat()
        {
            while (_isHeat == true)
            {
                yield return new WaitForSeconds(_timeUpdate);
                
                if (_isHeat == false)
                    yield break;

                if (_currentT < _targetT && _currentT <= _maxT)
                {
                    SetTedcText();
                    _currentT += _stepTUpdate;
                    _output.SetTemperature(_currentT);
                    _output.UpdateGraph();
                }
            }
        }
    }
}