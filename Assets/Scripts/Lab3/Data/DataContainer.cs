using System;
using System.Collections.Generic;

namespace VirtualLaboratory.Lab3
{
    public class DataContainer
    {
        public int MaxIndex => (Ip.Length - 1);

        private DataProvider _dataProvider = new();
        
        private float[] Ip;
        private float[] Uplus;
        private float[] Uminus;
        private Dictionary<int, MagneticData> _magneticData;

        public void Init(int variant)
        {
            _dataProvider.LoadData(variant);
            LoadData(variant);
        }
        
        public float[] GetIp() => Ip;
        public float[] GetUplus() => Uplus;
        public float[] GetUminus() => Uminus;
        
        public float GetIpByIndex(int index) => Ip[index];
        public float GetUplusByIndex(int index) => Uplus[index];
        public float GetUminusByIndex(int index) => Uminus[index];
        
        public float[] GetIpToIndex(int index) => TrimArray(Ip, index);
        public float[] GetUplusToIndex(int index) => TrimArray(Uplus, index);
        public float[] GetUminusToIndex(int index) => TrimArray(Uminus, index);

        public MagneticData GetMagneticData(int value) => _magneticData[value];
        
        private void LoadData(int variant)
        {
            Ip = _dataProvider.GetDataTaskOne(variant, "Ізр");
            Uplus = _dataProvider.GetDataTaskOne(variant, "Uр+");
            Uminus = _dataProvider.GetDataTaskOne(variant, "Uр-");
            
            _magneticData = new Dictionary<int, MagneticData>();
            
            int[] I_values = { 10, 20, 30, 40, 50 };
            foreach (int I in I_values)
            {
                string requestString = $"Ізр={I}мА";
                _magneticData.Add(I, _dataProvider.GetDataTaskTwo(variant, requestString));
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