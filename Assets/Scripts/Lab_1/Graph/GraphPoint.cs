using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VirtualLaboratory
{
    public class GraphPoint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _pointSize;
        [SerializeField] private Sprite _point;
        [SerializeField] private Color _pointColor;

        [Header("Tooltip"), Space(5)] [SerializeField] private GameObject _tooltip;
        [SerializeField] private TMP_Text _R_Text;
        [SerializeField] private TMP_Text _T_Text;
        
        public void Init(Transform parent, Vector2 anchoredPosition, float R, float T)
        {
            transform.SetParent(parent, false);

            Image image = GetComponent<Image>();
            image.sprite = _point;
            image.color = _pointColor;

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(_pointSize, _pointSize);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.anchoredPosition = anchoredPosition;

            _R_Text.text = "R = " + R.ToString("F2");
            _T_Text.text = "T = " + T.ToString("F1");
            HideTooltip();
        }
        
        public void ShowTooltip() => _tooltip.SetActive(true);
        
        public void HideTooltip() => _tooltip.SetActive(false);
        
        public  void OnPointerEnter(PointerEventData eventData) => ShowTooltip();
        
        public  void OnPointerExit(PointerEventData eventData) => HideTooltip();
    }
}