using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class Graph : MonoBehaviour
    {
        [SerializeField] private TMP_Text _Ip_text;
        [SerializeField] private GraphPointer _graphPointer;
        
        [Header("Graph")] 
        [SerializeField] private RectTransform _graphContainer;
        
        [Header("Label")] 
        [SerializeField] private RectTransform _labelContainer;
        [SerializeField] private RectTransform _labelTemplateX;
        [SerializeField] private RectTransform _labelTemplateY;

        [Header("Dash")] 
        [SerializeField] private RectTransform _dashContainer;
        [SerializeField] private RectTransform _dashTemplateX;
        [SerializeField] private RectTransform _dashTemplateY;
        
        private List<GameObject> _gameObjectList;
        
        private float _minY, _maxY;
        private float _minX, _maxX;
        private float _graphWidth, _graphHeight;
        private float _widthOffset, _heightOffset;

        public void Init(int maxPoints)
        {
            _graphWidth = _graphContainer.sizeDelta.x;
            _widthOffset = _graphWidth * 0.025f;
            _graphWidth -= _graphWidth * 0.05f;
            
            _graphHeight = _graphContainer.sizeDelta.y;
            _heightOffset = _graphHeight * 0.015f;
            _graphHeight -= _graphHeight * 0.05f;

            ClearGrid();
            _graphPointer.Init(maxPoints);
        }

        public void UpdateGraph(IEnumerable<float> x_list, IEnumerable<float> y_list, float Ip)
        {
            _Ip_text.text = "Ip = " + Ip.ToString("0.##") + " (mA)";
            
            List<float> xList = x_list.ToList();
            List<float> yList = y_list.ToList();
            
            UpdateBorders(xList.Min(), xList.Max(), out _minX, out _maxX);
            UpdateBorders(yList.Min(), yList.Max(), out _minY, out _maxY);
            CreateGrid(xList, yList);
        }
        
        private void UpdateBorders(float newMin, float newMax, out float currentMin, out float currentMax)
        {
            if (newMin > newMax)
                newMin = newMax / 2f;
            
            currentMin = newMin;
            currentMax = newMax;
        }

        private void ClearGrid()
        {
            if (_gameObjectList == null)
            {
                _gameObjectList = new List<GameObject>();
                return;
            }

            foreach (GameObject obj in _gameObjectList)
                Destroy(obj);
                
            _gameObjectList.Clear();
        }

        private void CreateGrid(List<float> x_list, List<float> y_list)
        {
            ClearGrid();
            _graphPointer.HideGraph();
            
            AdjustAxis(_labelTemplateX, _dashTemplateX, in _minX, in _maxX, false);
            AdjustAxis(_labelTemplateY, _dashTemplateY, in _minY, in _maxY, true);
            
            _graphPointer.ShowGraph(x_list, y_list, _graphWidth,  _graphHeight, 
                in _minX, in _maxX, in _minY, in _maxY, _widthOffset, _heightOffset);
        }

        private void AdjustAxis(RectTransform labelPrefab, RectTransform dashPrefab, in float min, in float max, bool isVertical)
        {
            int separatorCount = 8;
            
            for (int i = 0; i <= separatorCount; i++) 
            {
                float normalizedValue = i * 1f / separatorCount;
                Vector2 position;
                
                if (isVertical == true)
                    position = new Vector2(0f, (normalizedValue * _graphHeight) + _heightOffset);
                else
                    position = new Vector2((normalizedValue * _graphWidth) + _widthOffset, 0f);
                
                RectTransform label = CreateLabel(labelPrefab, in isVertical, position);
                
                if (i % 2 == 0)
                    label.GetComponent<TMP_Text>().text = (min + (normalizedValue * (max - min))).ToString("0.##");
                else
                    label.GetComponent<TMP_Text>().text = "";
                
                CreateDash(dashPrefab, position);
            }
            
            if (min < 0 && max > 0)
            {
                float normalizedZero = (0 - min) / (max - min);
                Vector2 position;
                
                if (isVertical == true)
                    position = new Vector2(0f, (normalizedZero * _graphHeight) + _heightOffset);
                else
                    position = new Vector2((normalizedZero * _graphWidth) + _widthOffset, 0f);
                
                RectTransform zeroLabel = CreateLabel(labelPrefab, in isVertical, position);
                zeroLabel.GetComponent<TMP_Text>().text = "0.00";
                CreateDash(dashPrefab, position);
            }
        }

        private RectTransform CreateLabel(RectTransform labelPrefab, in bool isVertical, in Vector2 position)
        {
            RectTransform label = Instantiate(labelPrefab, _labelContainer, false);
            label.gameObject.SetActive(true);
            _gameObjectList.Add(label.gameObject);
            
            if (isVertical == true)
                label.anchoredPosition = new Vector2(-10f, position.y);
            else
                label.anchoredPosition = new Vector2(position.x, -15f);

            return label;
        }

        private void CreateDash(RectTransform dashPrefab, in Vector2 position)
        {
            RectTransform dash = Instantiate(dashPrefab, _dashContainer, false);
            dash.gameObject.SetActive(true);
            dash.anchoredPosition = position;
            _gameObjectList.Add(dash.gameObject);
        }
    }
}