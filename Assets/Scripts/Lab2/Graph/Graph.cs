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

        private float _graphWidth, _graphHeight;

        public void Init()
        {
            _graphWidth = _graphContainer.sizeDelta.x;
            _graphHeight = _graphContainer.sizeDelta.y;
        }

        public void UpdateGraph(IEnumerable<float> x_list, IEnumerable<float> y_list)
        {
            UpdateBordersY(y_list.First(), y_list.Last());
            CreateGrid(x_list, y_list, _minY, _maxY);
        }
        
        private void UpdateBordersY(float minY = 0f, float maxY = 1f)
        {
            _minY = minY - (minY * 0.25f);
            _maxY = maxY + (maxY * 0.25f);
        }

        private void ClearGrid()
        {
            foreach (GameObject obj in _gameObjectList)
                Destroy(obj);
                
            _gameObjectList.Clear();
        }

        private void CreateGrid(IEnumerable<float> x_list, IEnumerable<float> y_list, float yMin, float yMax)
        {
            ClearGrid();

            int y_count = y_list.Count();
            int maxVisibleValueAmount = y_count;

            if (maxVisibleValueAmount < 5)
                maxVisibleValueAmount = 5;
            
            float xSize = _graphWidth / (maxVisibleValueAmount + 1);
            int xIndex = 0;
            
            _graphPointer.ShowGraph(y_list, in _graphHeight, in yMin, in yMax, in xSize);
            
            foreach (float x in x_list)
            {
                float xPosition = xSize + xIndex * xSize;
                
                RectTransform labelX = Instantiate(_labelTemplateX, _labelContainer, false);
                labelX.gameObject.SetActive(true);
                labelX.anchoredPosition = new Vector2(xPosition, -7f);

                if (xIndex == 0 || y_count < 2 || xIndex % 2 == 0 || xIndex == y_count - 1)
                    labelX.GetComponent<TMP_Text>().text = x.ToString("0.##");
                else
                    labelX.GetComponent<TMP_Text>().text = "";
                
                _gameObjectList.Add(labelX.gameObject);
                
                RectTransform dashX = Instantiate(_dashTemplateX, _dashContainer, false);
                dashX.gameObject.SetActive(true);
                dashX.anchoredPosition = new Vector2(xPosition, -3f);
                _gameObjectList.Add(dashX.gameObject);

                xIndex++;
            }
            
            int separatorCount = 10;
            
            for (int i = 0; i <= separatorCount; i++) 
            {
                RectTransform labelY = Instantiate(_labelTemplateY, _labelContainer, false);
                labelY.gameObject.SetActive(true);
                
                float normalizedValue = i * 1f / separatorCount;
                labelY.anchoredPosition = new Vector2(-7f, normalizedValue * _graphHeight);
                
                if (i == 0 || i % 2 == 0 || i == separatorCount)
                    labelY.GetComponent<TMP_Text>().text = (yMin + (normalizedValue * (yMax - yMin))).ToString("0.##");
                else
                    labelY.GetComponent<TMP_Text>().text = "";
                
                _gameObjectList.Add(labelY.gameObject);
                
                RectTransform dashY = Instantiate(_dashTemplateY, _dashContainer, false);
                dashY.gameObject.SetActive(true);
                dashY.anchoredPosition = new Vector2(-4f, normalizedValue * _graphHeight);
                _gameObjectList.Add(dashY.gameObject);
            }
        }
    }
}