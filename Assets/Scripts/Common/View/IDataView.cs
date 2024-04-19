namespace VirtualLaboratory
{
    public interface IDataView
    {
        public void Init();
        public void UpdateScheme(params float[] values);
    }
}