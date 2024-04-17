using UnityEngine;

namespace VirtualLaboratory.Lab1
{
    public class Lab1Manager : LabManager
    {
        [Header("Lab 1")]
        [SerializeField] private Heater _heater;
        [SerializeField] private MaterialLab[] _materials;

        protected override void Init()
        {
            base.Init();
            
            _variantChoiser.Init(_materials);
            _heater.Init();
        }

        protected override void StartClicked()
        {
            base.StartClicked();
            _heater.StartGauge();
        }

        protected override void StopClicked()
        {
            base.StopClicked();
            _heater.Stop();
        }
    }
}