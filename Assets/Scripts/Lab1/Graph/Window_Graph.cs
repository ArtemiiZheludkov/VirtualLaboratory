using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VirtualLaboratory.Lab1
{
    public class Window_Graph : MonoBehaviour
    {
        [SerializeField] private GraphPoint _point;

        [Header("Graph")] [SerializeField] private RectTransform _graphContainer;
        
        [Header("Label")] [SerializeField] private RectTransform _labelContainer;
        [SerializeField] private RectTransform _labelTemplateX;
        [SerializeField] private RectTransform _labelTemplateY;

        [Header("Dash")] [SerializeField] private RectTransform _dashContainer;
        [SerializeField] private RectTransform _dashTemplateX;
        [SerializeField] private RectTransform _dashTemplateY;
        
        [Header("Table")] [SerializeField] private Table _table;
        [Header("Ln")] [SerializeField] private Toggle _LnToggle;
        [Header("LSM")] [SerializeField] private Toggle _LSMToggle;
        [Header("GraphProcessor")][SerializeField] private GraphProcessor _processor;
        
        private List<GameObject> _gameObjectList;

        private List<float> _resistanceList;
        private List<float> _temperatureList;
        private List<float> _resistanceList_Ln;
        private List<float> _temperatureList_1T;
        private int _maxVisibleValueAmount = -1;

        private float _minR, _maxR;
        private int _nextT;

        private float _yMaximum, _yMinimum;
        private float _yMaximumLn, _yMinimumLn;

        private void Start()
        {
            _LnToggle.onValueChanged.AddListener(OnLnToggle);
            _LSMToggle.onValueChanged.AddListener(OnLSMToggle);
        }

        public void Init(float minR, float maxR, float startT, MaterialType materialType)
        {
            if (_gameObjectList != null)
            {
                foreach (GameObject obj in _gameObjectList)
                    Destroy(obj);
                
                _gameObjectList.Clear();
            }
            
            _gameObjectList = new List<GameObject>();
            _resistanceList = new List<float>();
            _temperatureList = new List<float>();
            _resistanceList_Ln = new List<float>();
            _temperatureList_1T = new List<float>();
            
            _nextT = Mathf.RoundToInt(startT);
            UpdateBordersY(minR, maxR);
            
            _table.Init();

            _LnToggle.isOn = false;
            _LSMToggle.isOn = false;
            
            _processor.Init(materialType);
        }
        
        public void AddPoint(float R, float T)
        {
            if (_nextT > T)
                return;
            
            _nextT += 2;
            _resistanceList.Add(R);
            _temperatureList.Add(T);
            _resistanceList_Ln.Add(Mathf.Log(R));
            _temperatureList_1T.Add((1 / T) * Mathf.Pow(10, 3));
            
            if (R > _maxR || R < _minR) 
                UpdateBordersY(R, R);

            if (_LnToggle.isOn == true)
            {
                _resistanceList_Ln.Sort();
                _temperatureList_1T.Sort();
                UpdateGraph(_resistanceList_Ln, _temperatureList_1T, _yMaximumLn, _yMinimumLn);
            }
            else
            {
                UpdateGraph(_resistanceList, _temperatureList, _yMaximum, _yMinimum);
            }
            
            _table.AddRow(T, R);
        }

        private void OnLnToggle(bool activated)
        {
            if (_resistanceList == null)
                return;
            
            if (activated == true)
            {
                _resistanceList_Ln.Sort();
                _temperatureList_1T.Sort();
                UpdateGraph(_resistanceList_Ln, _temperatureList_1T, _yMaximumLn, _yMinimumLn);
            }
            else
                UpdateGraph(_resistanceList, _temperatureList, _yMaximum, _yMinimum);
            
            _processor.OnLinearToggle(activated);
        }

        private void OnLSMToggle(bool activated)
        {
            if (activated == true)
            {
                if (_LnToggle.isOn == true)
                {
                    _resistanceList_Ln.Sort();
                    _temperatureList_1T.Sort();
                    UpdateGraph(_resistanceList_Ln, _temperatureList_1T, _yMaximumLn, _yMinimumLn);
                }
                else
                    UpdateGraph(_resistanceList, _temperatureList, _yMaximum, _yMinimum);
            }

            _processor.OnLSMToggle(activated);
        }

        private void UpdateBordersY(float minR = 0f, float maxR = 1f)
        {
            if (_resistanceList.Count < 2)
            {
                _minR = minR;
                _maxR = maxR;
                
                float yDifference = _maxR - _minR;
            
                if (yDifference <= 0)
                    yDifference = 5f;
            
                _yMaximum = _maxR + (yDifference * 0.1f);
                _yMinimum = _minR - (yDifference * 0.1f);
            }
            else
            {
                _minR = _resistanceList.Min();
                _maxR = _resistanceList.Max();
                
                _yMaximum = _maxR + (_maxR * 0.1f);
                _yMinimum = _minR - (_minR * 0.25f);
            }
            
            _yMaximumLn = Mathf.Log(_yMaximum);

            if (_yMinimum < 0.1f)
                _yMinimumLn = _yMinimum;
            else
                _yMinimumLn = Mathf.Log(_yMinimum);
        }

        private void UpdateGraph(List<float> R, List<float> T, float yMax, float yMin)
        {
            foreach (GameObject obj in _gameObjectList)
                Destroy(obj);
                
            _gameObjectList.Clear();
            
            _maxVisibleValueAmount = R.Count;

            if (_maxVisibleValueAmount < 5)
                _maxVisibleValueAmount = 5;
            
            float graphWidth = _graphContainer.sizeDelta.x;
            float graphHeight = _graphContainer.sizeDelta.y;
            float xSize = graphWidth / (_maxVisibleValueAmount + 1);
            int xIndex = 0;
            
            for (int i = 0; i < R.Count; i++)
            {
                float xPosition = xSize + xIndex * xSize;
                float yPosition = ((R[i] - yMin) / (yMax - yMin)) * graphHeight;
                
                _gameObjectList.Add(CreatePoint(new Vector2(xPosition, yPosition), i));
                
                RectTransform labelX = Instantiate(_labelTemplateX, _labelContainer, false);
                labelX.gameObject.SetActive(true);
                labelX.anchoredPosition = new Vector2(xPosition, -7f);

                if (i == 0 || R.Count < 2 || i % 2 == 0 || i == R.Count - 1)
                    labelX.GetComponent<TMP_Text>().text = T[i].ToString("0.##");
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
                labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
                
                if (i == 0 || i % 2 == 0 || i == separatorCount)
                    labelY.GetComponent<TMP_Text>().text = (yMin + (normalizedValue * (yMax - yMin))).ToString("0.##");
                else
                    labelY.GetComponent<TMP_Text>().text = "";
                
                _gameObjectList.Add(labelY.gameObject);
                
                RectTransform dashY = Instantiate(_dashTemplateY, _dashContainer, false);
                dashY.gameObject.SetActive(true);
                dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
                _gameObjectList.Add(dashY.gameObject);
            }

            if (_LSMToggle.isOn == true)
                _processor.CreateLSMLine(R, T, yMax, yMin, xSize, graphHeight);
        }
        
        private GameObject CreatePoint(Vector2 anchoredPosition, int index)
        {
            GraphPoint point = Instantiate(_point);
            point.Init(_graphContainer, anchoredPosition, _resistanceList[index], _temperatureList[index]);
            return point.gameObject;
        }
    }
}
