namespace VirtualLaboratory.Lab2
{
    public class DefaultModule : ProcessModule
    {
        protected override (float[], float[]) PrepareData(float[] x_array, float[] y_array)
        {
            return (x_array, y_array);
        }

        protected override void ProcessData(float[] x_array, float[] y_array)
        {
        }

        protected override void ShowProcessResult()
        {
        }
    }
}