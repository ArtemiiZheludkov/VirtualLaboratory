using UnityEngine;
using UnityEngine.UI;

namespace VirtualLaboratory.Lab2
{
    public class Lab2Manager : MonoBehaviour
    {
        [SerializeField] private VariantChoiser _variantChoiser;
        [SerializeField] private ExperimentController _experimentController;
        
        [Header("MENU")]
        [SerializeField] private GameObject _blockPanel;
        [SerializeField] private Button _start;
        [SerializeField] private Button _stop;
        
        private DataContainer _dataContainer;
        
        private void Start()
        {
            _variantChoiser.Init();
            _dataContainer = new DataContainer();
            
            _blockPanel.gameObject.SetActive(true);
            
            _start.onClick.AddListener(StartExperiment);
            _stop.onClick.AddListener(StopExperiment);
            
            _start.gameObject.SetActive(true);
            _stop.gameObject.SetActive(false);
        }

        private void StartExperiment()
        {
            _variantChoiser.Disable();
            _dataContainer.Init(_variantChoiser.Variant);
            _experimentController.Init(_dataContainer);
            
            _blockPanel.gameObject.SetActive(false);
            
            _start.gameObject.SetActive(false);
            _stop.gameObject.SetActive(true);
        }
        
        private void StopExperiment()
        {
            _variantChoiser.Enable();
            
            _blockPanel.gameObject.SetActive(true);
            
            _start.gameObject.SetActive(true);
            _stop.gameObject.SetActive(false);
        }
    }
}