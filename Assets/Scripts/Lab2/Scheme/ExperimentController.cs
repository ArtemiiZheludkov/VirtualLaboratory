using System.Collections;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class ExperimentController : CurrentInput
    {
        [SerializeField] private float _updateTime;
        [SerializeField] private DataView _dataView;
        [SerializeField] private ControlButton _controlButton;

        private DataContainer _data;
        private int _index;

        private float _currentIp;
        private bool _isPaused, _isStoped;

        public void Init(DataContainer data)
        {
            _data = data;
            
            _dataView.Init();
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

            _isPaused = false;
            _isStoped = false;
            StartCoroutine(Experiment());
        }
        
        private void StopClicked()
        {
            if (_isStoped == true)
                StopAllCoroutines();

            _isPaused = !_isStoped;
        }

        private IEnumerator Experiment()
        {
            while (_isStoped == false)
            {
                yield return new WaitForSeconds(_updateTime);
                Debug.Log("Update");
                
                if (_isPaused == true)
                    continue;

                if (_index >= 0 && _index <= _data.MaxIndex)
                {
                    _dataView.UpdateScheme(_data.GetDataUz(_index), _data.GetDataIz(_currentIp, _index));
                    _index += 1; // UPDATE INDEX
                }
            }
        }
    }
}