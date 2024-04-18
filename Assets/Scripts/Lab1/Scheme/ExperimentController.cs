using System.Collections;
using UnityEngine;

namespace VirtualLaboratory.Lab1
{
    public class ExperimentController : CurrentInput
    {
        [SerializeField] private float _stepTUpdate;
        [SerializeField] private DataView _dataView;
        //[SerializeField] private ControlButton _controlButton;
        //[SerializeField] private DataProcessor _dataProcessor;
        [SerializeField] private Graph _graph;
        [SerializeField] private Noiser _noiser;

        private DataContainer _data;
        private int _index;

        private float _currentIp, _currentT;
        private float _updateTime;
        private bool _isPaused, _isStopped;

        public void Init(DataContainer data)
        {
            _data = data;
            
            _dataView.Init();
            _graph.Init(_data.MaxIndex);
            _noiser.Init();
            //_controlButton.Init(StartClicked, StopClicked);
            //_dataProcessor.Init(_data, _graph);
            
            _isStopped = true;
            _isPaused = false;
            
            _graph.ClearGraph();
        }
        
        public override void SetCurrent(float I)
        {
            if (I < 0.75f || I > 1f)
            {
                I = 0f;
                //_controlButton.DisableInteractivity();
            }
            else
               // _controlButton.EnableInteractivity();

            if ((int)(I * 100f) != (int)(_currentIp * 100f))
            {
                _isStopped = true;
                //_controlButton.SetStart();
            }
            
            _currentIp = I;
        }

        private void SetUpdateTime(float time)
        {
            _updateTime = time;
        }

        private void StartClicked()
        {
            if (_currentIp < 0.1f || _currentIp > 1f)
                return;

            _isStopped = false;
            
            if (_isPaused == false)
                StartCoroutine(Experiment());
            else
                _isPaused = false;
        }
        
        private void StopClicked()
        {
            if (_isStopped == true)
                StopAllCoroutines();

            _isPaused = !_isStopped;
        }

        private IEnumerator Experiment()
        {
            Debug.Log("Start coroutine ----------- ");
            _index = 0;
            _currentT = _data.GetXByIndex(0);
            _graph.ClearGraph();
            //_dataProcessor.DisableProcessing();
            
            while (_isStopped == false)
            {
                yield return new WaitForSeconds(_updateTime);
                
                if (_isPaused == true)
                    continue;
                
                if (_index >= 0 && _index <= _data.MaxIndex)
                {
                    if (_currentT > _data.GetXByIndex(_index))
                    {
                        _dataView.UpdateScheme(_data.GetXByIndex(_index), _data.GetYByIndex(_index));
                        _graph.UpdateGraph(_data.GetXToIndex(_index), _data.GetYToIndex(_index));
                        _index += 1;
                    }

                    _currentT += _stepTUpdate;
                }

                if (_index > _data.MaxIndex)
                {
                    //_dataProcessor.EnableProcessing(_currentIp);
                    break;
                }
            }
        }
    }
}