using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLaboratory.Lab1
{
    public class Heater : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _currentText;
        [SerializeField] private TMP_Text _minText;
        [SerializeField] private TMP_Text _maxText;
        [SerializeField] private TMP_Text _TedcText;
        
        private Action<float> OnSliderMoved;
        private float _startT, _stepT, _maxT, _targetT;

        public void Init(Action<float> onSliderMoved, float startT, float stepT, float maxT)
        {
            OnSliderMoved = onSliderMoved;
            _startT = startT - 1;
            _stepT = stepT;
            _maxT = maxT;
            
            _targetT = _startT;
            
            _slider.minValue = _startT;
            _slider.maxValue = _maxT;
            _slider.value = _startT;

            _currentText.text = _startT.ToString("F0");
            _minText.text = _startT.ToString("F0");
            _maxText.text = _maxT.ToString("F0");
            _TedcText.text = "0.0";
        }

        public void EnableInteractivity() => _slider.interactable = true;
        
        public void DisableInteractivity() => _slider.interactable = false;

        public float GetTargetT() => _targetT;
        
        public void SetValue(float T)
        {
            _TedcText.text = (((T - _startT) / _stepT) / 10f).ToString("#0.00");
        }

        public void OnSliderMove()
        {
            _targetT = _slider.value;
            _currentText.text = _targetT.ToString("F0");
            OnSliderMoved?.Invoke(_targetT);
        }
    }
}