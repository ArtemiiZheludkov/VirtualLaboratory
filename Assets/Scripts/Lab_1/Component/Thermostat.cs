using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLaboratory
{
    public class Thermostat : CurrentInput
    {
        [SerializeField] private DigitalMeasurer _voltmeter;

        [Header("MATERIALS")]
        [SerializeField] private TMP_Text _tempText;
        [SerializeField] private Image _metalIcon;
        [SerializeField] private Image _semiconductorIcon;

        [SerializeField] private Window_Graph _windowGraph;
        
        private float _currentI;
        private MaterialLab _currentMaterial;
        
        public void Init(float startTemp)
        {
            _voltmeter.Init();
            _tempText.text = startTemp.ToString("F1");
        }

        public void StartGauge(float maxT)
        {
            float lastR = _currentMaterial.ResistanceAtMaxTemperature(maxT);

            if (_currentMaterial.Type == MaterialType.Metal) 
                _windowGraph.Init(_currentMaterial.Resistance, lastR, _currentMaterial.Temperature);
            else if (_currentMaterial.Type == MaterialType.Semiconductor) 
                _windowGraph.Init(lastR, _currentMaterial.Resistance, _currentMaterial.Temperature);
            
            UpdateGraph();
        }

        public void SetMaterial(MaterialLab currentMaterial)
        {
            _currentMaterial = currentMaterial;
            
            if (_currentMaterial.Type == MaterialType.Metal)
            {
                _metalIcon.gameObject.SetActive(true);
                _semiconductorIcon.gameObject.SetActive(false);
            }
            else if (_currentMaterial.Type == MaterialType.Semiconductor)
            {
                _metalIcon.gameObject.SetActive(false);
                _semiconductorIcon.gameObject.SetActive(true);
            }
        }

        public override void SetCurrent(float current)
        {
            _currentI = current;
            UpdateResistance();
        }

        public void SetTemperature(float temp)
        {
            _currentMaterial.SetTemperature(temp);
            UpdateResistance();
            _tempText.text = temp.ToString("F1");
        }

        public void UpdateGraph()
        {
            if (_currentI > 0.1f)
                _windowGraph.AddPoint(_currentMaterial.Resistance, _currentMaterial.Temperature);
            else
                Debug.Log("0");//_windowGraph.AddPoint(0f, _currentMaterial.Temperature);
        }

        private void UpdateResistance()
        {
            _currentMaterial.SetResistance();

            if (_currentI > 0.1f)
                _voltmeter.SetValue(_currentMaterial.Resistance);
            else
                _voltmeter.SetValue(0f);
        }
    }
}
