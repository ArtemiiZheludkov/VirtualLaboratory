namespace VirtualLaboratory.Lab3
{
    public class HallTaskOne : ProcessModule, IHaveConstants
    {
        private float a, c, d;
        private float[] ro;

        public void SetConstants(params float[] values)
        {
            a = values[0];
            c = values[1];
            d = values[2];
        }

        protected override (float[], float[]) PrepareData(float[] x_array, float[] y_array)
        {
            return (x_array, y_array);
        }

        protected override void ProcessData(float[] x_array, float[] y_array)
        {
            ro = new float[5];

            for (int i = 0; i < ro.Length; i++)
                ro[i] = (x_array[i] * a * c) / (y_array[i] * d);
        }

        protected override void ShowProcessResult()
        {
            float[] sigma = new float[5];
            
            for (int i = 0; i < ro.Length; i++)
                sigma[i] = 1 / ro[i];
            
            string text1 = "ρ<sub>I</sub> = (U<sub>ср</sub>*a*c) / (I<sub>зр</sub>*d) (Om*cm)";

            string text2 = "σ<sub>I</sub> = 1/ρ<sub>I</sub> => " 
                           +"σ<sub>10</sub>=" + sigma[0].ToString("#0.00") + " (1/Om*cm); ";

            string text3 = "σ<sub>20</sub>=" + sigma[1].ToString("#0.00") + " (1/Om*cm); " 
                           +"σ<sub>30</sub>=" + sigma[2].ToString("#0.00") + " (1/Om*cm); ";

            string text4 = "σ<sub>40</sub>=" + sigma[3].ToString("#0.00") + " (1/Om*cm); " 
                           +"σ<sub>50</sub>=" + sigma[4].ToString("#0.00") + " (1/Om*cm); ";
            
            _resultView.AddTextLine(text1);
            _resultView.AddTextLine(text2);
            _resultView.AddTextLine(text3);
            _resultView.AddTextLine(text4);
        }
    }
}