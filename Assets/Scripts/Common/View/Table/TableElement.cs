using TMPro;
using UnityEngine;

namespace VirtualLaboratory
{
    public class TableElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text _x;
        [SerializeField] private TMP_Text _y;
        
        public void Init()
        {
            ClearText();
        }

        public void SetText(in float x, in float y)
        {
            _x.text = x.ToString("0.##");
            _y.text = y.ToString("0.##");
        }

        public void ClearText()
        {
            _x.text = "";
            _y.text = "";
        }
    }
}