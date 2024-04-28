using UnityEngine;

namespace VirtualLaboratory.Lab3
{
    public class HallTaskOne2 : ProcessModule
    {
        private LeastSquares _processor;
        
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
            string text1 = "y = a*x + b";

            string text2 = "ρ<sub>0</sub> = 1 / a";

            string text3 = "σ<sub>0</sub> = 1/ρ<sub>0</sub> = a = " 
                           +( _processor.A / 1000f).ToString("#0.0000") +"+- "
                           +( _processor.ErrorA / 1000f).ToString("#0.0000") +"(1/Om*cm); ";
            
            _resultView.AddTextLine(text1);
            _resultView.AddTextLine(text2);
            _resultView.AddTextLine(text3);
        }
    }
}