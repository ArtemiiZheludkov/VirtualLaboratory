using System;

namespace VirtualLaboratory.Lab3
{
    public class DataContainer
    {
        public static DataContainer Instance { get; private set; }
        
        public float a, c, d;
        public int currentIz;
        
        public int MaxIndex => (Ip.Length - 1);

        private DataProvider _dataProvider = new();

        public float[] Ip;
        public float[] Uplus;
        public float[] Uminus;
        private MagneticData[] _magneticData;

        public void Init(int variant)
        {
            Instance = this;
            
            _dataProvider.LoadData(variant);
            LoadData(variant);
            
            if (variant == 1)
            {
                a = 0.05f;
                c = 0.26f;
                d = 0.3f;
            }
            else
            {
                a = 0.019f;
                c = 0.1f;
                d = 0.2f;
            }
        }
        
        public float[] GetIp() => Ip;
        public float[] GetUplus() => Uplus;
        public float[] GetUminus() => Uminus;
        
        public float[] GetIpToIndex(int index) => TrimArray(Ip, index);
        public float[] GetUplusToIndex(int index) => TrimArray(Uplus, index);
        public float[] GetUminusToIndex(int index) => TrimArray(Uminus, index);

        public ref MagneticData GetMagneticData(int value) => ref _magneticData[(value/10) - 1];
        
        private void LoadData(int variant)
        {
            Ip = _dataProvider.GetDataTaskOne(variant, "Ізр");
            Uplus = _dataProvider.GetDataTaskOne(variant, "Uр+");
            Uminus = _dataProvider.GetDataTaskOne(variant, "Uр-");
            
            int[] I_values = { 10, 20, 30, 40, 50 };
            _magneticData = new MagneticData[I_values.Length];

            for (int i = 0; i < I_values.Length; i++)
            {
                string requestString = $"Ізр={I_values[i]}мА";
                _magneticData[i] = _dataProvider.GetDataTaskTwo(variant, requestString);
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