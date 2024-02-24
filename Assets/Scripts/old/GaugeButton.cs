using System;
using DG.Tweening;
using UnityEngine;

namespace VirtualLaboratory
{
    public class GaugeButton : MonoBehaviour
    {
        [SerializeField] private float _enabledPosX;
        [SerializeField] private float _disabledPosX;

        [Space(15)]
        [SerializeField] private bool _isChangeColor = false;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _enabledMaterial;
        [SerializeField] private Material _disabledMaterial;

        private Action _click;

        public void SetEnabledState()
        {
            transform.DOLocalMoveX(_enabledPosX, 0.5f).OnComplete(() => 
            {
                if (_isChangeColor == true)
                    _renderer.material = _enabledMaterial;
            });
        }

        public void SetDisabledState()
        {
            transform.DOLocalMoveX(_disabledPosX, 0.5f).OnComplete(() => 
            {
                if (_isChangeColor == true)
                    _renderer.material = _disabledMaterial;
            });
        }

        public void SubscribeOnClick(Action onClick)
        {
            _click = onClick;
        }

        private void OnMouseDown()
        {
            _click?.Invoke();
        }
    }
}
