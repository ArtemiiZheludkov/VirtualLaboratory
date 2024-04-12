﻿using System;

namespace VirtualLaboratory.Lab2
{
    public class PlasmaTaskOne : ProcessModule
    {
        private LeastSquares _processor;

        public override void Init(Graph graph, ResultView resultView)
        {
            base.Init(graph, resultView);
            _processor = new LeastSquares();
        }

        protected override (float[], float[]) PrepareData(float[] x_array, float[] y_array)
        {
            int newLength = 8;
            
            float[] newX = new float[8];
            float[] newY = new float[8];
            Array.Copy(x_array, 0, newX, 0, newLength);
            Array.Copy(y_array, 0, newY, 0, newLength);
            
            return (newX, newY);
        }

        protected override void ProcessData(float[] x_array, float[] y_array)
        {
            _processor.Fit(x_array, y_array);
        }

        protected override void ShowProcessResult()
        {
            string text1 = "a = " +_processor.A.ToString("#0.00") 
                                  +" +- " +_processor.ErrorA.ToString("#0.000");
            
            string text2 = "b = " +_processor.B.ToString("#0.00") 
                                  +" +- " +_processor.ErrorB.ToString("#0.000") 
                                  + " = I<sub>2</sub>";
            
            _resultView.AddTextLine(text1);
            _resultView.AddTextLine(text2);
        }
    }
}