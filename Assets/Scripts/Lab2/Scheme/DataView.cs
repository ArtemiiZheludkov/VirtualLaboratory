using System;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    [Serializable]
    public class DataView : IDataView
    {
        [SerializeField] private DigitalMeasurer _UzMeasurer;
        [SerializeField] private DigitalMeasurer _IzMeasurer;

        public void Init()
        {
            _UzMeasurer.Init();
            _IzMeasurer.Init();
        }

        public void UpdateScheme(params float[] values)
        {
            UpdateView(values[0], values[1]);
        }
        
        private void UpdateView(float Uz, float Iz)
        {
            _UzMeasurer.SetValue(Uz);
            _IzMeasurer.SetValue(Iz);
        }
    }
}