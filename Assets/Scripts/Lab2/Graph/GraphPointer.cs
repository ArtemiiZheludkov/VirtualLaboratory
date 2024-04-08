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

        public void Init(in int maxPoints)
        {
            if (_points != null)
            {
                foreach (RectTransform obj in _points)
                    Destroy(obj.gameObject);
                
                _points.Clear();
            }
            else
                _points = new List<RectTransform>();
            
            for (int i = 0; i <= maxPoints; i++)
                CreateGraphPoint();
        }

        public void HideGraph()
        {
            foreach (RectTransform point in _points)
                point.gameObject.SetActive(false);
        }

        public void ShowGraph(float[] x_array, float[] y_array, float graphWidth, float graphHeight, 
            in float xMin, in float xMax, in float yMin, in float yMax, in float xOffset, in float yOffset)
        {
            for (int i = 0; i < x_array.Length; i++)
            {
                float xPosition = ((x_array[i] - xMin) / (xMax - xMin)) * graphWidth;
                float yPosition = ((y_array[i] - yMin) / (yMax - yMin)) * graphHeight;
                
                xPosition += xOffset;
                yPosition += yOffset;
                
                SetPoint(_points[i], new Vector2(xPosition, yPosition));
            }
        }

        private void SetPoint(RectTransform point, Vector2 anchoredPosition)
        {
            point.anchoredPosition = anchoredPosition;
            point.gameObject.SetActive(true);
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