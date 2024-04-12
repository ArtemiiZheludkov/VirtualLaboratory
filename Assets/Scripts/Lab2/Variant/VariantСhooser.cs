using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public abstract class VariantСhooser : MonoBehaviour
    {
        [SerializeField] private VariantButton _buttonPrefab;
        [SerializeField] private Transform _buttonsParent;
        
        protected VariantButton[] _buttons;
        
        protected abstract void SetVariant(int index);

        protected abstract void OnCreateButton(int index);

        protected virtual void OnClickedVariant(VariantButton buttonCall)
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                if (ReferenceEquals(_buttons[i], buttonCall) == true)
                    SetVariant(i);
                else
                    _buttons[i].Disable();
            }

            buttonCall.Enable();
        }

        protected virtual void CreateButtons(int variantsLength)
        {
            _buttons = new VariantButton[variantsLength];

            for (int i = 0; i < variantsLength; i++)
            {
                _buttons[i] = Instantiate(_buttonPrefab, _buttonsParent, false);
                _buttons[i].gameObject.SetActive(true);
                OnCreateButton(i);
            }
        }
    }
}