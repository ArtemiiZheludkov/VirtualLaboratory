using System;
using UnityEngine;

namespace VirtualLaboratory.Lab1
{
    [Serializable]
    public class DataView : IDataView
    {
        [SerializeField] private DigitalMeasurer _voltmeter;
        [SerializeField] private DigitalMeasurer _thermometer;

        public void Init()
        {
            _voltmeter.Init();
            _thermometer.Init();
        }

        public void UpdateScheme(params float[] values)
        {
            UpdateView(values[0], values[1]);
        }

        private void UpdateView(float U, float T)
        {
            _voltmeter.SetValue(U);
            _thermometer.SetValue(T);
        }
    }
}