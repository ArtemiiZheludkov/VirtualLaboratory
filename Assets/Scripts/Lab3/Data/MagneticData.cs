using System;

namespace VirtualLaboratory.Lab3
{
    public struct MagneticData
    {
        public float[] B;
        public float[] I;
        public float[] UPlus;
        public float[] UMinus;
        
        public float[] GetBToIndex(int index) => TrimArray(B, index);
        public float[] GetIToIndex(int index) => TrimArray(I, index);
        public float[] GetUPlusToIndex(int index) => TrimArray(UPlus, index);
        public float[] GetUMinusToIndex(int index) => TrimArray(UMinus, index);
        
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