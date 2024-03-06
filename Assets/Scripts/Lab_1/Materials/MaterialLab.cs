using System;
using UnityEngine;

namespace VirtualLaboratory
{
    [Serializable]
    public class MaterialLab
    {
        public MaterialType Type => _material;
        public float Temperature { get; private set; }
        public float Resistance { get; private set; }
        public string Name => _buttonName;

        [SerializeField] private MaterialType _material;
        [SerializeField] private string _buttonName;
        [SerializeField] private float _koefTemperature;
        [SerializeField] private float _resistivity;
        [SerializeField] private float _l;
        [SerializeField] private float _s;
        
        private float _startT;
        private const float K = 1.38e-23f;
        
        public void Init(float startTemperature)
        {
            Temperature = startTemperature + 273f;
            _startT = startTemperature + 273f;
            SetResistance();
        }

        public void SetResistance()
        {
            if (_material == MaterialType.Metal)
            {
                float r = _resistivity * (1 + _koefTemperature * (Temperature - _startT));
                Resistance = r * (_l / _s);
            }
            else if (_material == MaterialType.Semiconductor)
            {
                Resistance = _resistivity * Mathf.Exp(_koefTemperature / (2 * K * Temperature));
                //Resistance = r * (_l / _s);
                //Resistance = _resistivity * Mathf.Exp(_koefTemperature / (2 * K * Tkelv));
            }
        }

        public void SetTemperature(float T)
        {
            Temperature = T + 273f;
        }

        public float ResistanceAtMaxTemperature(float maxT)
        {
            float R = 0f;
            
            if (_material == MaterialType.Metal)
            {
                float r = _resistivity * (1 + _koefTemperature * (Temperature - _startT));
                R = r * (_l / _s);
            }
            else if (_material == MaterialType.Semiconductor)
            {
                float Tkelv = 273 + maxT;
                Resistance = _resistivity * Mathf.Exp(_koefTemperature / (2 * K * Tkelv));
                //R = _resistivity * Mathf.Exp(_koefTemperature * ((1f / (maxT + 273)) - (1f / (_startT + 273))));
                //float r = _resistivity * (1 - _koefTemperature * (Temperature - _startT));
                //R = r * (_l / _s);
            }

            return R;
        }
    }
}