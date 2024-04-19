using System;
using UnityEngine;

namespace VirtualLaboratory.Lab1
{
    public class SemiconductorTask : ProcessModule
    {
        private LeastSquares _processor;

        public override void Init(Graph graph, ResultView resultView)
        {
            base.Init(graph, resultView);
            _processor = new LeastSquares();
        }
        
        public override void Enable(float[] x_array, float[] y_array)
        {
            base.Enable(x_array, y_array);
            string xText = "1/T (K)*10<sup>3</sup>";
            string yText = "LnR (Om)";
            _graph.SetAxisName(xText, yText);
        }

        protected override (float[], float[]) PrepareData(float[] x_array, float[] y_array)
        {
            int newLength = x_array.Length;
            
            float[] newX = new float[newLength];
            float[] newY = new float[newLength];
            Array.Copy(x_array, 0, newX, 0, newLength);
            Array.Copy(y_array, 0, newY, 0, newLength);
            
            for (int i = 0; i < newX.Length; i++)
                newX[i] = (1f / newX[i]) * 1000; // *10^3
            
            for (int i = 0; i < newY.Length; i++)
                newY[i] = Mathf.Log(newY[i]);
            
            return (newX, newY);
        }

        protected override void ProcessData(float[] x_array, float[] y_array)
        {
            int newLength = x_array.Length;
            float[] newX = new float[newLength];
            Array.Copy(x_array, 0, newX, 0, newLength);
            
            for (int i = 0; i < newX.Length; i++)
                newX[i] /= 1000; 
            
            _processor.Fit(newX, y_array);
            
            float x1 = x_array[0];
            float x2 = x_array[^1];
            float y1 = _processor.A * x1 + _processor.B;
            float y2 = _processor.A * x2 + _processor.B;
            
            Vector2 start = new Vector2(x1, y1);
            Vector2 end = new Vector2(x2, y2);
            
            _graph.AddLine(in start, in end);
        }

        protected override void ShowProcessResult()
        {
            float Eg = 2 * _processor.A * 8.617333262145e-5f;
            float EgError = 2 * _processor.ErrorA * 8.617333262145e-5f;
            float R0 = Mathf.Exp(_processor.B);
            float R0Error = Mathf.Exp(_processor.ErrorB) * Mathf.Pow(10, -8);
            
            string text1 = "LnR=LnR<sub>0</sub> * Eg/2kT";
            
            string text2 = "a=Eg/2kT=>Eg=2a*k=" 
                           +Eg.ToString("#0.00") +" +- " 
                           +EgError.ToString("#0.0e-0") +" (eV)";
            
            string text3 = "b=LnR<sub>0</sub>=>R<sub>0</sub>=e<sup>b</sup>= " 
                           +R0.ToString("#0.00e-0") +" +- " 
                           +R0Error.ToString("#0.00e-00") +" (Om)";
            
            _resultView.AddTextLine(text1);
            _resultView.AddTextLine(text2);
            _resultView.AddTextLine(text3);
        }
    }
}