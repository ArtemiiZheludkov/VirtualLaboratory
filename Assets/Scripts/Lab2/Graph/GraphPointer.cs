using System.Collections.Generic;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class GraphPointer : MonoBehaviour
    {
        [Header("POINT SETTINGS")] 
        [SerializeField] private GameObject _point;
        [SerializeField] private float _pointSize;
        [SerializeField] private Transform _pointParent;

        private List<RectTransform> _points;

        public void Init(int maxPoints)
        {
            _points = new List<RectTransform>();

            for (int i = 0; i <= maxPoints; i++)
                CreateGraphPoint();
        }

        public void HideGraph()
        {
            foreach (RectTransform point in _points)
                point.gameObject.SetActive(false);
        }

        public void ShowGraph(IEnumerable<float> y_list, in float graphHeight, in float yMin, in float yMax, in float xSize)
        {
            int index = 0;

            foreach (float point in y_list)
            {
                float xPosition = xSize + index * xSize;
                float yPosition = ((point - yMin) / (yMax - yMin)) * graphHeight;
                
                SetPoint(_points[index], new Vector2(xPosition, yPosition));
                index++;
            }
        }

        private void SetPoint(RectTransform point, Vector2 anchoredPosition)
        {
            point.anchoredPosition = anchoredPosition;
        }

        private void CreateGraphPoint()
        {
            GameObject point = Instantiate(_point, _pointParent, false);

            RectTransform rectTransform = point.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(_pointSize, _pointSize);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            
            point.SetActive(false);
            _points.Add(rectTransform);
        }
    }
}