using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace VirtualLaboratory
{
    public class MainGauge : MonoBehaviour
    {
        public bool Enabled { get; private set; }

        [FormerlySerializedAs("_materialSwitcher")] [SerializeField] private MaterialSwitcherOld materialSwitcherOld; 
        
        [Space(5), Header("Buttons")]
        [SerializeField] private GaugeButton _turnButton;
        [SerializeField] private GaugeButton _heatButton;
        [SerializeField] private GaugeButton _coolButton;

        [Header("Text")] 
        [SerializeField] private TextMeshPro _temperatureText;
        [SerializeField] private TextMeshPro _resistanceText;

        [Header("Change in temperature by 1 C")]
        [SerializeField] private float _changeTime;
        [SerializeField] private float _minTemperature;
        [SerializeField] private float _maxTemperature;

        [Space(10)] 
        [SerializeField] private MaterialLab _metal;
        [SerializeField] private MaterialLab _semiconductor;
        
        private bool _isHeat;

        private void Start()
        {
            Enabled = false;
            _isHeat = false;
            
            _metal.Init(20);
            _semiconductor.Init(20);

            _resistanceText.DOFade(0f, 0f);
            _temperatureText.DOFade(0f, 0f);
            
            _turnButton.SubscribeOnClick(OnTurnButton);
            _turnButton.SetDisabledState();
            
            _heatButton.SubscribeOnClick(OnHeatClicked);
            _heatButton.SetDisabledState();
            
            _coolButton.SubscribeOnClick(OnCoolClicked);
            _coolButton.SetDisabledState();

            StartCoroutine(GaugeUpdate());
        }

        public void StopHeat()
        {
            _isHeat = false;
            _heatButton.SetDisabledState();
            _coolButton.SetDisabledState();
        }

        private IEnumerator GaugeUpdate()
        {
            while (true)
            {
                yield return new WaitForSeconds(_changeTime);

                if (_isHeat == true)
                {
                    if (materialSwitcherOld.Material == MaterialType.Metal)
                    { 
                        //_metal.Heat();
                        //_semiconductor.Cool();
                        
                        _resistanceText.text = _metal.Resistance.ToString("F2");
                        _temperatureText.text = _metal.Temperature.ToString("F1");
                    }
                    else if (materialSwitcherOld.Material == MaterialType.Semiconductor)
                    {
                        //_metal.Cool();
                        //_semiconductor.Heat();
                        
                        _resistanceText.text = _semiconductor.Resistance.ToString("F2");
                        _temperatureText.text = _semiconductor.Temperature.ToString("F1");
                    }
                }
                else
                {
                    Afk();
                }
            }
        }

        private void Afk()
        {
            //_metal.Cool();
            //_semiconductor.Cool();
                
            if (materialSwitcherOld.Material == MaterialType.Metal)
            { 
                _resistanceText.text = _metal.Resistance.ToString("F2");
                _temperatureText.text = _metal.Temperature.ToString("F1");
            }
            else if (materialSwitcherOld.Material == MaterialType.Semiconductor)
            {
                _resistanceText.text = _semiconductor.Resistance.ToString("F2");
                _temperatureText.text = _semiconductor.Temperature.ToString("F1");
            }
        }

        private void OnTurnButton()
        {
            Enabled = !Enabled;

            if (Enabled == true)
            {
                _turnButton.SetEnabledState(); 
                
                Afk();
                
                _resistanceText.DOFade(1f, 0.45f);
                _temperatureText.DOFade(1f, 0.45f);
            }
            else
            {
                _turnButton.SetDisabledState();
                _heatButton.SetDisabledState();
                _coolButton.SetDisabledState();
                
                _isHeat = false;
                
                _resistanceText.DOFade(0f, 0f);
                _temperatureText.DOFade(0f, 0f);
            }
        }

        private void OnHeatClicked()
        {
            if (materialSwitcherOld.Enabled == false || Enabled == false)
                return;

            if (_isHeat == false)
            {
                _isHeat = true;
                _heatButton.SetEnabledState();
            }
            else
            {
                _isHeat = false;
                _heatButton.SetDisabledState();
            }
            
            _coolButton.SetDisabledState();
        }
        
        private void OnCoolClicked()
        {
            if (materialSwitcherOld.Enabled == false || Enabled == false)
                return;
            
            if (_isHeat == true)
                _coolButton.SetEnabledState();
            else
                _coolButton.SetDisabledState();
            
            _isHeat = false;
            _heatButton.SetDisabledState();
        }
    }
}
