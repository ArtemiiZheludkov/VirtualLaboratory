using System;
using UnityEngine;

namespace VirtualLaboratory.Lab3
{
    [Serializable]
    public class DataView : IDataView
    {
        [SerializeField] private DigitalMeasurer _V1Measurer;
        [SerializeField] private DigitalMeasurer _V2Measurer;
        [SerializeField] private DigitalMeasurer _ImagMeasurer;

        public void Init()
        {
            _V1Measurer.Init();
            _V2Measurer.Init();
        }

        public void UpdateScheme(params float[] values)
        {
            UpdateView(values[0], values[1], values[3]);
        }
        
        private void UpdateView(float V1, float V2, float Imag)
        {
            _V1Measurer.SetValue(V1);
            _V2Measurer.SetValue(V2);
            _ImagMeasurer.SetValue(Imag);
        }
    }
}