using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace VirtualLaboratory
{
    public class Graph : MonoBehaviour
    {
        [SerializeField] private GraphPointer _graphPointer;
        [SerializeField] private GraphLine _graphLine;
        [SerializeField] private Table _table;
        
        [Header("Graph")] 
        [SerializeField] private RectTransform _graphContainer;
        
        [Header("Text")]
        [SerializeField] private TMP_Text _addName;
        [SerializeField] private TMP_Text _xText;
        [SerializeField] private TMP_Text _yText;
        [SerializeField] private string _xDefaultName;
        [SerializeField] private string _yDefaultName;
        
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
            _graphPointer.Init(in maxPoints);
            _graphLine.Init();
            _table.Init(in maxPoints);
        }

        public void SetAddNameY(string addName)
        {
            _addName.text = addName;
        }

        public void UpdateGraph(float[] x_array, float[] y_array)
        {
            UpdateBorders(x_array.Min(), x_array.Max(), out _minX, out _maxX);
            UpdateBorders(y_array.Min(), y_array.Max(), out _minY, out _maxY);
            CreateGrid();
            
            _graphPointer.HideGraph();
            _graphPointer.ShowGraph(x_array, y_array, _graphWidth,  _graphHeight, 
                in _minX, in _maxX, in _minY, in _maxY, _widthOffset, _heightOffset);
            
            for (int i = 0; i < x_array.Length; i++)
                _table.UpdateRow(i, x_array[i], y_array[i]);
        }

        public void SetAxisName(string xName, string yName)
        {
            if (xName != null)
                _xText.text = xName;   
            
            if (yName != null)
                _yText.text = yName;
        }

        public void AddLine(in Vector2 start, in Vector2 end)
        {
            float x1 = ((start.x - _minX) / (_maxX - _minX)) * _graphWidth;
            float x2 = ((end.x - _minX) / (_maxX - _minX)) * _graphWidth;
            float y1 = ((start.y - _minY) / (_maxY - _minY)) * _graphHeight;
            float y2 = ((end.y - _minY) / (_maxY - _minY)) * _graphHeight;

            Vector2 newStart = new Vector2(x1 + _widthOffset, y1 + _heightOffset);
            Vector2 newEnd = new Vector2(x2 + _widthOffset, y2 + _heightOffset);
            
            _graphLine.AddLine(newStart, newEnd);
        }

        public void ClearGraph()
        {
            SetAxisName(_xDefaultName, _yDefaultName);
            _graphPointer.HideGraph();
            _graphLine.Clear();
            _table.HideTable();
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

        private void CreateGrid()
        {
            ClearGrid();
            AdjustAxis(_labelTemplateX, _dashTemplateX, in _minX, in _maxX, false);
            AdjustAxis(_labelTemplateY, _dashTemplateY, in _minY, in _maxY, true);
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