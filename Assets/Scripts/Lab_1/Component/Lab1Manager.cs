using UnityEngine;
using UnityEngine.UI;

namespace VirtualLaboratory.Lab1
{
    public class Lab1Manager : MonoBehaviour
    {
        [SerializeField] private MaterialSwitcher _materialSwitcher;
        [SerializeField] private Heater _heater;
        [SerializeField] private CurrentSource _currentSource;

        [SerializeField] private GameObject _blockPanel;
        [SerializeField] private Button _start;
        [SerializeField] private Button _stop;

        private void Start()
        {
            _materialSwitcher.Init();
            _heater.Init();
            _currentSource.Init();
            _blockPanel.gameObject.SetActive(true);
            
            _start.onClick.AddListener(StartExperiment);
            _stop.onClick.AddListener(StopExperiment);
            
            _start.gameObject.SetActive(true);
            _stop.gameObject.SetActive(false);
        }

        private void StartExperiment()
        {
            _materialSwitcher.Disable();
            _heater.StartGauge();
            _currentSource.StartGauge();
            _blockPanel.gameObject.SetActive(false);
            
            _start.gameObject.SetActive(false);
            _stop.gameObject.SetActive(true);
        }
        
        private void StopExperiment()
        {
            _materialSwitcher.Enable();
            _heater.Stop();
            _currentSource.Stop();
            _blockPanel.gameObject.SetActive(true);
            
            _start.gameObject.SetActive(true);
            _stop.gameObject.SetActive(false);
        }
    }
}