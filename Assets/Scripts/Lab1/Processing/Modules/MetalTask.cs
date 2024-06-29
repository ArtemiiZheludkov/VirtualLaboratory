using System;
using UnityEngine;

namespace VirtualLaboratory.Lab1
{
    public class MetalTask : ProcessModule
    {
        private LeastSquares _processor;
        private string alpha = "\u03B1", celsius = "\u00b0C";
        private float R0;

        public override void Init(Graph graph, ResultView resultView)
        {
            base.Init(graph, resultView);
            _processor = new LeastSquares();
        }

        protected override (float[], float[]) PrepareData(float[] x_array, float[] y_array)
        {
            int newLength = x_array.Length;
            
            float[] newX = new float[newLength];
            float[] newY = new float[newLength];
            Array.Copy(x_array, 0, newX, 0, newLength);
            Array.Copy(y_array, 0, newY, 0, newLength);
            
            return (newX, newY);
        }

        protected override void ProcessData(float[] x_array, float[] y_array)
        {
            R0 = y_array[0];
            _processor.Fit(x_array, y_array);
            
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
            float a = _processor.A / R0;
            
            string text1 = $"R=R<sub>0</sub> + R<sub>0</sub>*{alpha}*t";
            
            string text2 = $"a=R<sub>0</sub>*{alpha}=>{alpha}=a/R<sub>0</sub>=" 
                           +a.ToString("#0.00000") +" +- " 
                           +_processor.ErrorA.ToString("#0.00000") +$" (1/{celsius})";
            
            string text3 = "b=R<sub>0</sub>=" 
                           +_processor.B.ToString("#0.00") +" +- " 
                           +_processor.ErrorB.ToString("#0.00") +" (Om)";
            
            _resultView.AddTextLine(text1);
            _resultView.AddTextLine(text2);
            _resultView.AddTextLine(text3);
        }
    }
}