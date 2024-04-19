namespace VirtualLaboratory.Lab2
{
    public class DataProcessor : DefaultDataProcessor
    {
        private float _currentIp;
        private DataContainer _data;

        public void Init(DataContainer data, Graph graph)
        {
            _data = data;
            Init(graph);
        }

        public void EnableProcessing(float currentIp)
        {
            _currentIp = currentIp;
            EnableProcessing();
        }

        protected override void EnableCurrentModule()
        {
            string addName = "Ip = " + _currentIp.ToString("0.##") + " (mA)";
            _graph.SetAddNameY(addName);
            _currentModule.Enable(_data.GetUzData(), _data.GetIzData(_currentIp));
        }
    }
}