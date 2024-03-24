using System.Collections;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class ExperimentController : MonoBehaviour
    {
        [SerializeField] private float _updateTime;
        [SerializeField] private DataView _dataView;

        private DataContainer _data;
        private int _index;

        private float _currentIp;

        public void Init()
        {
            _dataView.Init();
        }

        private IEnumerator Experiment()
        {
            while (true)
            {
                yield return new WaitForSeconds(_updateTime);
                
                _dataView.UpdateScheme(_data.GetDataUz(_index), _data.GetDataIz(_currentIp, _index));
                _index += 1;
            }
        }
    }
}