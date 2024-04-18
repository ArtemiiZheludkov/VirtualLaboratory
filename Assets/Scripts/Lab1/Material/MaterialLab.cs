using System;
using UnityEngine;

namespace VirtualLaboratory.Lab1
{
    [Serializable]
    public class MaterialLab : DefaultVariant
    {
        public MaterialType Type => _material;

        [SerializeField] private MaterialType _material;
        [SerializeField] private float _koefTemperature;
        [SerializeField] private float _resistivity;
        [SerializeField] private float _l;
        [SerializeField] private float _s;
        
        private float _startT;
        private const float K = 1.38e-23f;
        
        public void Init(float startTemperature)
        {
            _startT = startTemperature;
        }

        public float CalculateResistance(float T)
        {
            float R = 0f;

            if (_material == MaterialType.Metal)
            {
                float r = _resistivity + _resistivity * _koefTemperature * (T - _startT);
                R = r * (_l / _s);
            }
            else if (_material == MaterialType.Semiconductor)
            {
                R = _resistivity * Mathf.Exp(_koefTemperature / (2 * K * T));
            }

            return R;
        }
    }
}