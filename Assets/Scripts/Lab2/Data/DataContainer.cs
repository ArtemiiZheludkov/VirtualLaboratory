﻿using System.Collections.Generic;
using System.Globalization;

namespace VirtualLaboratory.Lab2
{
    public class DataContainer
    {
        private DataProvider _dataProvider;
        
        private List<float> Uz;
        private Dictionary<int, List<float>> Iz;

        public DataContainer()
        {
            _dataProvider = new DataProvider();
        }

        public void Init(int variant)
        {
            LoadData(variant);
        }

        public float GetDataUz(int index) => Uz[index];

        public float GetDataIz(float Ip, int index) => Iz[(int)(Ip*100)][index];

        private void LoadData(int variant)
        {
            Uz = _dataProvider.GetData(variant, "Uz(V)");
            Iz = new Dictionary<int, List<float>>();
            
            float[] Ip_values = { 0.1f, 0.15f, 0.2f, 0.25f, 0.3f, 0.35f, 0.4f };
            foreach (float Ip in Ip_values)
            {
                string requestString = $"Ip={Ip.ToString(CultureInfo.InvariantCulture)}(mA)";
                Iz.Add((int)(Ip*100), _dataProvider.GetData(variant, requestString));
            }
        }
    }
}