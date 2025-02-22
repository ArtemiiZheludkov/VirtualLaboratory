﻿using UnityEngine;

namespace VirtualLaboratory.Lab3
{
    public class HallTaskThree : ProcessModule
    {
        private float a, I, sigma;
        private float e = 1.602176487e-19f;
        private float m = 9.1093856e-31f;
        
        private LeastSquares _processor;
        DataContainer dataContainer;

        private void SetConstants()
        {
            dataContainer = DataContainer.Instance;
            
            a = dataContainer.a;
            I = dataContainer.currentIz;
            
            int index = (dataContainer.currentIz / 10) - 1;
            float U = (dataContainer.Uplus[index] - dataContainer.Uminus[index]) / 2f;
            float ro = ((U * a * dataContainer.c) / (dataContainer.Ip[index] * dataContainer.d)) * 1000f;
            sigma = 1 / ro;
        }
        
        public override void Init(Graph graph, ResultView resultView)
        {
            base.Init(graph, resultView);
            _processor = new LeastSquares();
        }
        
        public override void Enable(float[] x_array, float[] y_array)
        {
            base.Enable(x_array, y_array);
            _graph.SetAxisName("B (Тл)", "U (mV)");
        }

        protected override (float[], float[]) PrepareData(float[] x_array, float[] y_array)
        {
            return (x_array, y_array);
        }

        protected override void ProcessData(float[] x_array, float[] y_array)
        {
            SetConstants();
            
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
            ref MagneticData data = ref dataContainer.GetMagneticData(dataContainer.currentIz);
            
            float R = (a * _processor.A) / (I / 1000f);
            float[] w = new float[data.B.Length];

            for (int i = 0; i < w.Length; i++)
                w[i] = (e/m) * data.B[i];

            string text1 = "R<sub>H</sub>=" +R.ToString("#0.00") +" (Om); " 
                           +"σ<sub>H</sub>=" +sigma.ToString("#0.000") +" (1/Om*cm); ";

            string text2 = "ω=e*B/m (рад/с) => " 
                           +$"ω<sub>{data.B[0]}</sub>=" +w[0].ToString("#0.00e+0") +"; ";
            
            string text3 = $"ω<sub>{data.B[1]}</sub>=" +w[1].ToString("#0.00e+0") + "; "
                           +$"ω<sub>{data.B[2]}</sub>=" +w[2].ToString("#0.00e+0") +"; ";

            string text4 = $"ω<sub>{data.B[3]}</sub>=" +w[3].ToString("#0.00e+0") + "; "
                           +$"ω<sub>{data.B[4]}</sub>=" +w[4].ToString("#0.00e+0") + ";";

            _resultView.AddTextLine(text1);
            _resultView.AddTextLine(text2);
            _resultView.AddTextLine(text3);
            _resultView.AddTextLine(text4);
        }
    }
}