using DG.Tweening;
using UnityEngine;

namespace VirtualLaboratory
{
    public class MaterialSwitcherOld : MonoBehaviour
    {
        public bool Enabled { get; private set; }
        public MaterialType Material { get; private set; }

        [SerializeField] private MainGauge _mainGauge;
        
        [SerializeField] private MaterialType _startMaterial;
        
        [Space(5), Header("Turn button")] 
        [SerializeField] private GaugeButton _turnButton;
        
        [Space(5), Header("Switch button")] 
        [SerializeField] private GaugeButton _switchButton;
        [SerializeField] private float _angleM;
        [SerializeField] private float _angleS;
        
        private void Start()
        {
            Enabled = false;
            
            SwitchMaterial(_startMaterial);
            
            _turnButton.SubscribeOnClick(OnTurnButton);
            _turnButton.SetDisabledState();
            
            _switchButton.SubscribeOnClick(OnSwitchButton);
        }
        
        private void SwitchMaterial(MaterialType material)
        {
            Material = material;
            
            if (Material == MaterialType.Metal)
                _switchButton.transform.DOLocalRotate(new Vector3(_angleM, 0, 90f), 0.75f);
            else if (Material == MaterialType.Semiconductor)
                _switchButton.transform.DOLocalRotate(new Vector3(_angleS, 0, 90f), 0.75f);
        }
        
        private void OnTurnButton()
        {
            Enabled = !Enabled;

            if (Enabled == true)
                _turnButton.SetEnabledState();
            else
            {
                _turnButton.SetDisabledState();
                _mainGauge.StopHeat();
            }
        }

        private void OnSwitchButton()
        {
            if (Material == MaterialType.Metal)
                SwitchMaterial(MaterialType.Semiconductor);
            else if (Material == MaterialType.Semiconductor)
                SwitchMaterial(MaterialType.Metal);
        }
    }
}
