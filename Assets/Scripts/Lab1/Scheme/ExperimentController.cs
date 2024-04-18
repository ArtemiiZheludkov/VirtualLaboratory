using System.Collections;
using UnityEngine;

namespace VirtualLaboratory.Lab1
{
    public class ExperimentController : CurrentInput
    {
        [SerializeField] private float _updateTime;
        [SerializeField] private float _stepTUpdate;
        
        [SerializeField] private DataView _dataView;
        [SerializeField] private Heater _heater;
        //[SerializeField] private DataProcessor _dataProcessor;
        [SerializeField] private Graph _graph;
        [SerializeField] private Noiser _noiser;

        private DataContainer _data;
        private MaterialLab _material;
        private int _index;

        private float _currentI, _currentT, _targetT;
        private bool _isStarted, _isPaused;

        public void Init(DataContainer data, MaterialLab material, float startT, float stepT, float maxT)
        {
            _data = data;
            _material = material;
            
            _noiser.Init();
            _heater.Init(SetTargetT, startT, stepT, maxT);
            _dataView.Init(_heater);
            _graph.Init(_data.MaxIndex);
            //_dataProcessor.Init(_data, _graph);

            _isStarted = false;
            _isPaused = false;
            
            _graph.ClearGraph();
        }
        
        public override void SetCurrent(float I)
        {
            _currentI = I;
            
            if (_currentI < 0.8f || _currentI > 1f)
            {
                _isPaused = true;
                _heater.DisableInteractivity();
            }
            else
            {
                if (_currentT < _targetT)
                    _isPaused = false;
                
                _heater.EnableInteractivity();
                TryStartExperiment();
            }
        }
        
        public void Stop()
        {
            StopAllCoroutines();
            
            _isStarted = false;
            _isPaused = false;
        }

        private void SetTargetT(float T)
        {
            _targetT = T;
            
            if (_isStarted == true && _currentT >= _targetT)
                _isPaused = true;
            else
                _isPaused = false;
            
            TryStartExperiment();
        }

        private void TryStartExperiment()
        {
            if (_isStarted == true)
                return;

            if (_currentI < 0.8f || _currentI > 1f)
                return;
            
            _targetT = _heater.GetTargetT();
            _currentT = _targetT;
            
            StartCoroutine(Experiment());
        }

        private IEnumerator Experiment()
        {
            Debug.Log("Start coroutine ----------- ");
            _index = 0;
            _graph.ClearGraph();
            //_dataProcessor.DisableProcessing();
            
            if (_currentT >= _targetT)
                _isPaused = true;
            
            _isStarted = true;
            
            while (_isStarted == true)
            {
                yield return new WaitForSeconds(_updateTime);
                
                if (_isPaused == true)
                    continue;
                
                if (_index >= 0 && _index <= _data.MaxIndex)
                {
                    _dataView.UpdateScheme(_material.CalculateResistance(_currentT), _currentT);
                    
                    if (_currentT >= _data.GetXByIndex(_index))
                    {
                        _graph.UpdateGraph(_data.GetXToIndex(_index), _data.GetYToIndex(_index));
                        _index += 1;
                    }

                    _currentT += _stepTUpdate;
                }
                
                if (_currentT >= _targetT)
                    _isPaused = true;

                if (_index > _data.MaxIndex)
                {
                    //_dataProcessor.EnableProcessing(_currentIp);
                    break;
                }
            }
        }
    }
}