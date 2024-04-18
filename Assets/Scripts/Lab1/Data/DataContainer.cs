using System;

namespace VirtualLaboratory.Lab1
{
    public class DataContainer
    {
        public int MaxIndex => (X.Length - 1);
        
        private float[] X;
        private float[] Y;
        
        public void LoadData(MaterialLab material, float startT, float stepT, float stopT)
        {
            int dataLength = (int)(Math.Abs(startT - stopT) / stepT);
            X = new float[dataLength];
            Y = new float[dataLength];

            float currentT = startT;

            for (int i = 0; i < dataLength; i++)
            {
                X[i] = currentT;
                Y[i] = material.CalculateResistance(currentT);

                currentT += stepT;
            }
        }

        public float[] GetUzData() => X;
        public float[] GetIzData() => Y;
        
        public float GetXByIndex(int index) => X[index];

        public float GetYByIndex(int index) => Y[index];

        public float[] GetXToIndex(int index) => TrimArray(X, index);
        
        public float[] GetYToIndex(int index) => TrimArray(Y, index);

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