using UnityEngine;

namespace VirtualLaboratory.Lab3
{
    public class HallTaskTwo : ProcessModule
    {
        private float a, I, sigma;
        private string alpha = "\u03B1";
        private LeastSquares _processor;

        private void SetConstants()
        {
            DataContainer data = DataContainer.Instance;
            
            a = data.a;
            I = data.currentIz;
            
            int index = (data.currentIz / 10) - 1;
            float U = (data.Uplus[index] - data.Uminus[index]) / 2f;
            float ro = ((U * a * data.c) / (data.Ip[index] * data.d)) * 1000f;
            sigma = 1 / ro;
        }
        
        public override void Init(Graph graph, ResultView resultView)
        {
            base.Init(graph, resultView);
            _processor = new LeastSquares();
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
            float e = 1.6e-19f;
            
            float R = (a * _processor.A) / (I / 1000f);
            float n = 1 / (e * R);
            float my = R * sigma;

            string text1 = $"y = {alpha}*x + b";

            string text2 = $"U=(R<sub>H</sub>*B*I)/a => R<sub>H</sub>=(a*{alpha})/I= "
                           + R.ToString("#0.00") +" (Om)";
            
            string text3 = "n=1/(e*R<sub>H</sub>)=" + n.ToString("#0.00e+0") +" (сm<sup>-3</sup>)";
            string text4 = "μ=R<sub>H</sub>*σ=" + my.ToString("#0.00") +" (сm<sup>2</sup>/B*c)";

            _resultView.AddTextLine(text1);
            _resultView.AddTextLine(text2);
            _resultView.AddTextLine(text3);
            _resultView.AddTextLine(text4);
        }
    }
}