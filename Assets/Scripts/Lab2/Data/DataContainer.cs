using System;
using System.Collections.Generic;
using System.Globalization;

namespace VirtualLaboratory.Lab2
{
    public class DataContainer
    {
        public int MaxIndex => (Uz.Length - 1);

        private DataProvider _dataProvider;
        
        private float[] Uz;
        private Dictionary<int, float[]> Iz;

        public DataContainer()
        {
            _dataProvider = new DataProvider();
        }

        public void Init(int variant)
        {
            LoadData(variant);
        }

        public float[] GetUzData() => Uz;
        public float[] GetIzData(float Ip) => Iz[(int)(Ip*100)];
        
        public float GetUzByIndex(int index) => Uz[index];

        public float GetIzByIndex(float Ip, int index) => Iz[(int)(Ip*100)][index];

        public float[] GetUzToIndex(int index) => TrimArray(Uz, index);
        
        public float[] GetIzToIndex(float Ip, int index) => TrimArray(Iz[(int)(Ip * 100)], index);

        private void LoadData(int variant)
        {
            Uz = _dataProvider.GetData(variant, "Uz(V)");
            Iz = new Dictionary<int, float[]>();
            
            float[] Ip_values = { 0.1f, 0.15f, 0.2f, 0.25f, 0.3f, 0.35f, 0.4f };
            foreach (float Ip in Ip_values)
            {
                string requestString = $"Ip={Ip.ToString(CultureInfo.InvariantCulture)}(mA)";
                Iz.Add((int)(Ip*100), _dataProvider.GetData(variant, requestString));
            }
        }
        
        private float[] TrimArray(float[] original, int desiredCount)
        {
            desiredCount += 1;
            
            if (desiredCount >= original.Length)
                return (float[])original.Clone();
    
            float[] result = new float[desiredCount];
            Array.Copy(original, 0, result, 0, desiredCount);
            return result;
        }
    }
}