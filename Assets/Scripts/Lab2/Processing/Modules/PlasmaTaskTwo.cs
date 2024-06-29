using System;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class PlasmaTaskTwo : ProcessModule
    {
        private LeastSquares _processor;
        
        private const float e = 1.602176487e-19f;
        private const float k = 1.380649e-23f;
        private float T, errorT;
        
        public override void Init(Graph graph, ResultView resultView)
        {
            base.Init(graph, resultView);
            _processor = new LeastSquares();
        }

        public override void Enable(float[] x_array, float[] y_array)
        {
            base.Enable(x_array, y_array);
            _graph.SetAxisName(null, "LnIz (mkA)");
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
            int newLength = 23;
            float[] newX = new float[newLength];
            float[] newY = new float[newLength];
            Array.Copy(x_array, 0, newX, 0, newLength);
            Array.Copy(y_array, 0, newY, 0, newLength);
            
            _processor.Fit(newX, newY);
            
            float x1 = newX[0];
            float x2 = newX[^1];
            float y1 = _processor.A * x1 + _processor.B;
            float y2 = _processor.A * x2 + _processor.B;
            Vector2 start = new Vector2(x1, y1);
            Vector2 end = new Vector2(x2, y2);
            
            _graph.AddLine(in start, in end);
            
            T = (e / k) / _processor.A;
            errorT = (_processor.ErrorA / _processor.A) * T;
        }

        protected override void ShowProcessResult()
        {
            string text1 = "a = " +_processor.A.ToString("#0.00") 
                                  +" +- " +_processor.ErrorA.ToString("#0.000");
            
            string text2 = "b = " +_processor.B.ToString("#0.00") 
                                  +" +- " +_processor.ErrorB.ToString("#0.000");
            
            string text3 = "T<sub>1</sub> = (e / k) / a = " +T.ToString("#0.00") 
                                  +" +- " +errorT.ToString("#0.000") +" (K)";
            
            _resultView.AddTextLine(text1);
            _resultView.AddTextLine(text2);
            _resultView.AddTextLine(text3);
        }
    }
}