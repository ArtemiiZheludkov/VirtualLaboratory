using System.Collections;
using UnityEngine;

namespace VirtualLaboratory.Lab3
{
    public class ExperimentController : CurrentInput
    {
        [SerializeField] private float _updateTime;
        [SerializeField] private DataView _dataView;
        [SerializeField] private ControlButton _controlButton;
        [SerializeField] private MagneticController _magneticController;
        [SerializeField] private DataProcessor _dataProcessor;
        [SerializeField] private Graph _graph;

        private CurrentSource _currentSource;
        private DataContainer _data;
        private int _index;

        private int _currentIz;
        private bool _isPaused, _isStopped;
        private bool _magneticActivated;

        public void Init(DataContainer data, CurrentSource currentSource, int variant)
        {
            _data = data;
            _currentSource = currentSource;
            
            _dataView.Init(_magneticController);
            _graph.Init(_data.MaxIndex);
            _controlButton.Init(StartClicked, StopClicked);
            _magneticController.Init(MagneticOff, MagneticOn);
            _dataProcessor.Init(_data, _graph, variant);

            _isStopped = true;
            _isPaused = false;
            _magneticActivated = false;
            
            _graph.ClearGraph();
            _controlButton.SetStart();
            _magneticController.SetStop();
        }

        public override void SetCurrent(float Iz)
        {
            if (_magneticActivated == false && _isStopped == false)
                return;
            
            int I = (int)Iz;

            if (I < 10 || I > 50)
            {
                I = 0;
                if (_magneticActivated == true)
                    _controlButton.DisableInteractivity();
                else
                    _controlButton.EnableInteractivity();
            }
            else
            {
                _controlButton.EnableInteractivity();
            }

            if (I != _currentIz)
            {
                _isStopped = true;
                _controlButton.SetStart();
            }
            
            _currentIz = I;
        }

        private void StartClicked()
        {
            if (_magneticActivated == true)
                if (_currentIz < 10 || _currentIz > 50)
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
            {
                StopAllCoroutines();
                _currentSource.EnableInteractivity();
                _magneticController.EnableInteractivity();
            }

            _isPaused = !_isStopped;
        }
        
        private void MagneticOff()
        {
            _magneticActivated = false;
            _controlButton.EnableInteractivity();
        }

        private void MagneticOn()
        {
            _magneticActivated = true;

            if (_currentIz < 10 || _currentIz > 50)
                _controlButton.DisableInteractivity();
            else
                _controlButton.EnableInteractivity();
        }
        
        private void UpdateMagnetic(int index)
        {
            ref MagneticData data = ref _data.GetMagneticData(_currentIz);
            float[] U = new float[index + 1];

            for (int i = 0; i < U.Length; i++)
                U[i] = (data.UPlus[i] - data.UMinus[i]) / 2f;
            
            _dataView.UpdateScheme(data.UPlus[index], data.UMinus[index], data.I[index]);
            _graph.UpdateGraph(data.GetBToIndex(_index), U);
        }

        private void UpdateNoMagnetic(int index)
        {
            float[] U = new float[index + 1];

            for (int i = 0; i < U.Length; i++)
                U[i] = (_data.Uplus[i] - _data.Uminus[i]) / 2f;

            float I = 0f;
            if (index > 0)
                I = _data.Ip[index];
            
            _currentSource.SetValue(I);
            _currentIz = (int)I;
            
            _dataView.UpdateScheme(_data.Uplus[index], _data.Uminus[index], 0f);
            _graph.UpdateGraph(U, _data.GetIpToIndex(index));
        }

        private IEnumerator Experiment()
        {
            Debug.Log("Start coroutine ----------- ");
            PreStartExperiment();
            
            while (_isStopped == false)
            {
                yield return new WaitForSeconds(_updateTime);
                
                if (_isPaused == true)
                    continue;

                if (_index >= 0 && _index <= _data.MaxIndex)
                {
                    if (_magneticActivated == true)
                        UpdateMagnetic(_index);
                    else
                        UpdateNoMagnetic(_index);
                    
                    _index += 1;
                }

                if (_index > _data.MaxIndex)
                {
                    _currentSource.EnableInteractivity();
                    _magneticController.EnableInteractivity();
                    _dataProcessor.EnableProcessing(_currentIz);
                    
                    _isStopped = true;
                    _controlButton.SetStart();
                    break;
                }
            }
        }
        
        private void PreStartExperiment()
        {
            _index = 0;
            _graph.ClearGraph();
            _dataProcessor.DisableProcessing();
            _dataProcessor.SetModules(_magneticActivated);
            
            if (_magneticActivated == true)
            {
                _graph.SetAddNameY($"Iзр = {_currentIz} (mA)");
                _graph.SetAxisName("B (Тл)", "U (mV)");
                _currentSource.EnableInteractivity();
                _magneticController.DisableInteractivity();
            }
            else
            {
                _graph.SetAddNameY(" ");
                _graph.SetAxisName("U (mV)", "Iзр (мА)");
                _currentSource.DisableInteractivity();
                _magneticController.DisableInteractivity();
            }
        }
    }
}