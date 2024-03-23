using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class DataContainer : MonoBehaviour
    {
        private DataProvider _dataProvider;
        
        private List<float> Uz;
        private Dictionary<int, List<float>> Iz;

        private int _variant;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            _dataProvider = new DataProvider();
            _variant = 1;
            
            LoadData();
        }

        private void LoadData()
        {
            Uz = _dataProvider.GetData(_variant, "Uz(V)");
            Iz = new Dictionary<int, List<float>>();
            
            float[] Ip_values = { 0.1f, 0.15f, 0.2f, 0.25f, 0.3f, 0.35f, 0.4f };
            foreach (float Ip in Ip_values)
            {
                string requestString = $"Ip={Ip.ToString(CultureInfo.InvariantCulture)}(mA)";
                Iz.Add((int)(Ip*100), _dataProvider.GetData(_variant, requestString));
            }
        }
    }
}