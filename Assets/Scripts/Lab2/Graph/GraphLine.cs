using System.Collections.Generic;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class GraphLine : MonoBehaviour
    {
        [SerializeField] private RectTransform _linePrefab;
        [SerializeField] private Transform _lineParent;

        private List<RectTransform> _lines;

        public void Init()
        {
            Clear();
            _lines = new List<RectTransform>();
        }

        public void AddLine(in Vector2 point1, in Vector2 point2)
        {
            RectTransform line = CreateLine();
            line.anchoredPosition = point1;
            line.sizeDelta = new Vector2(Vector2.Distance(point1, point2), 7f);
            
            float angle = Mathf.Atan2(point2.y - point1.y, point2.x - point1.x) * Mathf.Rad2Deg;
            line.rotation = Quaternion.Euler(0, 0, angle);
        }

        public void Clear()
        {
            if (_lines != null)
            {
                foreach (RectTransform obj in _lines)
                    Destroy(obj.gameObject);
                
                _lines.Clear();
            }
        }

        private RectTransform CreateLine()
        {
            RectTransform line = Instantiate(_linePrefab, _lineParent, false);
            line.anchorMin = new Vector2(0, 0);
            line.anchorMax = new Vector2(0, 0);
            line.gameObject.SetActive(true);
            _lines.Add(line);

            return line;
        }
    }
}