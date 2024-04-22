using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLaboratory.Lab3
{
    public class MagneticController : ControlButton
    {
        [Header("UI")]
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private GameObject _interactImage;

        public override void Init(Action onStartClicked, Action onStopClicked)
        {
            base.Init(onStartClicked, onStopClicked);
            _valueText.text = "0.0";
        }
        
        public void SetValue(float value)
        {
            _valueText.text = value.ToString("#0.00");
        }

        public override void EnableInteractivity()
        {
            base.EnableInteractivity();
            _interactImage.SetActive(false);
        }

        public override void DisableInteractivity()
        {
            base.DisableInteractivity();
            _interactImage.SetActive(true);
        }

        protected override void StartClicked()
        {
            _slider.value = 1;
            base.StartClicked();
        }

        protected override void StopClicked()
        {
            _slider.value = 0;
            base.StopClicked();
        }
    }
}