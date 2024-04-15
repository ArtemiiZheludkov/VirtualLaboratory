using System;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class PlasmaTaskThree : ProcessModule
    {
        private const float e = 1.602176487e-19f;
        private const float k = 1.380649e-23f;
        private const float M = 3.329794e-25f;
        private const float m = 9.1093856e-31f;
        private const float S = 6.453793e-09f;
        
        private float T1, T2;
        private float voltage_zero, voltage_plasma;
        private float current_zero, n10;
        
        private LeastSquares _processor1;
        private LeastSquares _processor2;

        public override void Init(Graph graph, ResultView resultView)
        {
            base.Init(graph, resultView);
            _processor1 = new LeastSquares();
            _processor2 = new LeastSquares();
        }

        public override void Enable(float[] x_array, float[] y_array, float currentIp)
        {
            base.Enable(x_array, y_array, currentIp);
            string yText = "LnI<sub>z</sub> (mkA)";
            _graph.SetAxisName(null, yText);
        }

        protected override (float[], float[]) PrepareData(float[] x_array, float[] y_array)
        {
            int newStart = 17; // -6
            int newLength = x_array.Length - newStart;

            float[] newX = new float[newLength];
            float[] newY = new float[newLength];
            Array.Copy(x_array, newStart, newX, 0, newLength);
            Array.Copy(y_array, newStart, newY, 0, newLength);

            for (int i = 0; i < newY.Length; i++)
                newY[i] = Mathf.Log(newY[i]);

            return (newX, newY);
        }

        protected override void ProcessData(float[] x_array, float[] y_array)
        {
            var (start1, end1) = CreateLineOne(x_array, y_array);
            var (start2, end2) = CreateLineTwo(x_array, y_array);
            
            Vector2 start3 = IntersectionPoint(in start1, in end1, in start2, in end2);
            Vector2 end3 = new Vector2(start3.x, y_array[0] - (y_array[0] * 0.2f));
            _graph.AddLine(in start3, in end3);
            
            voltage_zero = start3.x;
            voltage_plasma = x_array[0];
            FindClosestNumbers(x_array, voltage_zero, out int less, out int more);
            float lnCurrent = MapValue(voltage_zero, x_array[less], x_array[more], y_array[less], y_array[more]);
            current_zero = Mathf.Exp(lnCurrent);
            
            CalculateValues();
        }

        protected override void ShowProcessResult()
        {
            string text1 = "V<sub>0</sub> = " + voltage_zero.ToString("#0.00") + " +- 1 (V)" 
                           +";  " +"I<sub>10</sub> = " + current_zero.ToString("#0.00") + " (mkA)";
            
            string text2 = "T<sub>2</sub> = (MT<sub>1</sub> / m) * exp((2e / kT<sub>1</sub>) * (V<sub>пл</sub> - V<sub>0</sub>))";
            string text3 = "=> T<sub>2</sub> = " + T2.ToString("#0.00") +" (K)";
            
            string text4 = "n<sub>10</sub> = I<sub>10</sub> / (e * (kT1 / 2πm)<sup>1/2</sup> * S)";
            string text5 = "=> n<sub>10</sub> = " + n10.ToString("0.00e+0") + " (m<sup>-3</sup>)";

            _resultView.AddTextLine(text1);
            _resultView.AddTextLine(text2);
            _resultView.AddTextLine(text3);
            _resultView.AddTextLine(text4);
            _resultView.AddTextLine(text5);
        }

        private void CalculateValues()
        {
            T1 = (e / k) / _processor1.A;
            float exp = ((2 * e) * (voltage_plasma - voltage_zero)) / (k * T1);
            T2 = ((M * T1) / m) * Mathf.Exp(exp);
            n10 = current_zero / (e * Mathf.Pow((k * T1) / (2 * Mathf.PI * m), 0.5f) * S);
        }
        
        private float MapValue(float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            float normalizedValue = (value - oldMin) / (oldMax - oldMin);
            float newValue = newMin + (normalizedValue * (newMax - newMin));

            return newValue;
        }

        private (Vector2, Vector2) CreateLineOne(float[] x_array, float[] y_array)
        {
            int newLength = 23;
            float[] newX = new float[newLength];
            float[] newY = new float[newLength];
            Array.Copy(x_array, 0, newX, 0, newLength);
            Array.Copy(y_array, 0, newY, 0, newLength);
            
            _processor1.Fit(newX, newY);
            
            float x1 = newX[0];
            float x2 = x_array[35];
            float y1 = _processor1.A * x1 + _processor1.B;
            float y2 = _processor1.A * x2 + _processor1.B;
            Vector2 start = new Vector2(x1, y1);
            Vector2 end = new Vector2(x2, y2);
            
            _graph.AddLine(in start, in end);
            
            return (start, end);
        }
        
        private (Vector2, Vector2) CreateLineTwo(float[] x_array, float[] y_array)
        {
            int newStart = 30;
            int newLength = x_array.Length - newStart;
            float[] newX = new float[newLength];
            float[] newY = new float[newLength];
            Array.Copy(x_array, newStart, newX, 0, newLength);
            Array.Copy(y_array, newStart, newY, 0, newLength);
            
            _processor2.Fit(newX, newY);
            
            float x1 = x_array[15];
            float x2 = newX[^1];
            float y1 = _processor2.A * x1 + _processor2.B;
            float y2 = _processor2.A * x2 + _processor2.B;
            Vector2 start = new Vector2(x1, y1);
            Vector2 end = new Vector2(x2, y2);
            
            _graph.AddLine(in start, in end);
            
            return (start, end);
        }
        
        private Vector2 IntersectionPoint(in Vector2 start1, in Vector2 end1, in Vector2 start2, in Vector2 end2)
        {
            Vector2 intersection = Vector2.zero;
            
            float A1 = end1.y - start1.y;
            float B1 = start1.x - end1.x;
            float C1 = A1 * start1.x + B1 * start1.y;

            float A2 = end2.y - start2.y;
            float B2 = start2.x - end2.x;
            float C2 = A2 * start2.x + B2 * start2.y;
            
            float determinant = A1 * B2 - A2 * B1;

            if (determinant != 0)
            {
                float x = (B2 * C1 - B1 * C2) / determinant;
                float y = (A1 * C2 - A2 * C1) / determinant;
                intersection = new Vector2(x, y);
            }

            return intersection;
        }
        
        private void FindClosestNumbers(float[] array, float target, out int indexLess, out int indexMore)
        {
            indexLess = -1;
            indexMore = -1;

            float minDiffLess = float.MaxValue;
            float minDiffMore = float.MaxValue;

            for (int i = 0; i < array.Length; i++)
            {
                float diff = array[i] - target;
                
                if (diff < 0 && Math.Abs(diff) < minDiffLess)
                {
                    minDiffLess = Math.Abs(diff);
                    indexLess = i;
                }
                
                if (diff > 0 && Math.Abs(diff) < minDiffMore)
                {
                    minDiffMore = Math.Abs(diff);
                    indexMore = i;
                }
            }
        }
    }
}