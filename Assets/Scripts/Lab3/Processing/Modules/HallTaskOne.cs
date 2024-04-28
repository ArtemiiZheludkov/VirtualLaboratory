namespace VirtualLaboratory.Lab3
{
    public class HallTaskOne : ProcessModule
    {
        private float a, c, d;
        private float[] ro;

        private void SetConstants()
        {
            a = DataContainer.Instance.a;
            c = DataContainer.Instance.c;
            d = DataContainer.Instance.d;
        }
        
        public override void Enable(float[] x_array, float[] y_array)
        {
            base.Enable(x_array, y_array);
            _graph.SetAxisName("U (mV)", "Iзр (мА)");
        }

        protected override (float[], float[]) PrepareData(float[] x_array, float[] y_array)
        {
            return (x_array, y_array);
        }

        protected override void ProcessData(float[] x_array, float[] y_array)
        {
            SetConstants();
            ro = new float[5];

            for (int i = 0; i < ro.Length; i++)
                ro[i] = ((x_array[i] * a * c) / (y_array[i] * d)) * 1000f;
        }

        protected override void ShowProcessResult()
        {
            float[] sigma = new float[5];
            
            for (int i = 0; i < ro.Length; i++)
                sigma[i] = 1 / ro[i];
            
            string text1 = "ρ<sub>I</sub> = (U<sub>ср</sub>*a*c) / (I<sub>зр</sub>*d) (Om*cm)";

            string text2 = "σ<sub>I</sub> = 1/ρ<sub>I</sub> (1/Om*cm) => " 
                           +"σ<sub>10</sub>=" + sigma[0].ToString("#0.000");

            string text3 = "σ<sub>20</sub>=" + sigma[1].ToString("#0.000") 
                           +"σ<sub>30</sub>=" + sigma[2].ToString("#0.000");

            string text4 = "σ<sub>40</sub>=" + sigma[3].ToString("#0.000") 
                           +"σ<sub>50</sub>=" + sigma[4].ToString("#0.000");
            
            _resultView.AddTextLine(text1);
            _resultView.AddTextLine(text2);
            _resultView.AddTextLine(text3);
            _resultView.AddTextLine(text4);
        }
    }
}