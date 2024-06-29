using UnityEngine;

namespace VirtualLaboratory
{
    public class LeastSquares
    {
        public float A { get; private set; }
        public float B { get; private set; }
        public float ErrorA { get; private set; }
        public float ErrorB { get; private set; }

        public void Fit(float[] xData, float[] yData)
        {
            if (xData.Length != yData.Length)
                return;

            int n = xData.Length;
            float sumX = 0, sumY = 0, sumX2 = 0, sumXY = 0;
            
            for (int i = 0; i < n; i++)
            {
                sumX += xData[i];
                sumY += yData[i];
                sumX2 += xData[i] * xData[i];
                sumXY += xData[i] * yData[i];
            }
            
            float denominator = n * sumX2 - sumX * sumX;
            A = (n * sumXY - sumX * sumY) / denominator;
            B = (sumY - A * sumX) / n;
            
            float ssResidual = 0;
            for (int i = 0; i < n; i++)
            {
                float fitVal = B + A * xData[i];
                ssResidual += (yData[i] - fitVal) * (yData[i] - fitVal);
            }

            float s2 = ssResidual / (n - 2);
            ErrorA = Mathf.Sqrt(n * s2 / denominator);
            ErrorB = Mathf.Sqrt(s2 * sumX2 / denominator);
        }
    }
}