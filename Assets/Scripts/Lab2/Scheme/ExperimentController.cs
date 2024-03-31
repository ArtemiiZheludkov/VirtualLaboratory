using System.Collections;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class ExperimentController : CurrentInput
    {
        [SerializeField] private float _updateTime;
        [SerializeField] private DataView _dataView;
        [SerializeField] private ControlButton _controlButton;

        [SerializeField] private Graph _graph;

        private DataContainer _data;
        private int _index;

        private float _currentIp;
        private bool _isPaused, _isStoped;

        public void Init(DataContainer data)
        {
            _data = data;
            
            _dataView.Init();
            _graph.Init(_data.MaxIndex);
            _controlButton.Init(StartClicked, StopClicked);

            _isStoped = true;
            _isPaused = false;
        }

        public override void SetCurrent(float Ip)
        {
            if (Ip < 0.1f || Ip > 0.4f)
            {
                Ip = 0f;
                _controlButton.DisableInteractivity();
            }
            else
                _controlButton.EnableInteractivity();

            if ((int)(Ip * 100f) != (int)(_currentIp * 100f))
            {
                _isStoped = true;
                _controlButton.SetStart();
            }
            
            _currentIp = Ip;
        }
        
        private void StartClicked()
        {
            if (_currentIp < 0.1f || _currentIp > 0.4f)
                return;

            _isStoped = false;
            
            if (_isPaused == false)
                StartCoroutine(Experiment());
            else
                _isPaused = false;
        }
        
        private void StopClicked()
        {
            if (_isStoped == true)
                StopAllCoroutines();

            _isPaused = !_isStoped;
        }

        private IEnumerator Experiment()
        {
            Debug.Log("Start coroutine ----------- ");
            _index = 0;
            
            while (_isStoped == false)
            {
                yield return new WaitForSeconds(_updateTime);
                
                if (_isPaused == true)
                    continue;

                if (_index >= 0 && _index <= _data.MaxIndex)
                {
                    _dataView.UpdateScheme(_data.GetDataUz(_index), _data.GetDataIz(_currentIp, _index));
                    _graph.UpdateGraph(_data.GetUzToIndex(_index), _data.GetIzToIndex(_currentIp, _index), _currentIp);
                    _index += 1; // UPDATE INDEX
                }
            }
        }
    }
}