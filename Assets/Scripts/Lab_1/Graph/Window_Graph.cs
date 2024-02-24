﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtualLaboratory;

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
    
    [Header("Ln")] [SerializeField] private Toggle _LnToggle;
    [SerializeField] private TMP_Text _textCelsium;

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
        _LnToggle.onValueChanged.AddListener(OnToggleChanged);
        _textCelsium.gameObject.SetActive(true);
    }

    public void Init(float minR, float maxR, float startT)
    {
        if (_gameObjectList != null)
        {
            foreach (GameObject obj in _gameObjectList)
                Destroy(obj);
            
            _gameObjectList.Clear();
        }
        
        _nextT = Mathf.RoundToInt(startT);
        SetMinMaxY(minR, maxR);
        
        _gameObjectList = new List<GameObject>();
        _resistanceList = new List<float>();
        _temperatureList = new List<float>();
        _resistanceList_Ln = new List<float>();
        _temperatureList_1T = new List<float>();
    }
    
    public void AddPoint(float R, float T)
    {
        if (_nextT > T)
            return;
		
        if (R > _maxR)
            SetMinMaxY(_minR, R);
        else if (R < _minR)
            SetMinMaxY(R, _maxR);
        
        _nextT += 2;
        _resistanceList.Add(R);
        _temperatureList.Add(T);
        _resistanceList_Ln.Add(Mathf.Log(R));
        Debug.Log(Mathf.Log(R));
        _temperatureList_1T.Add((1 / (T + 273)) * Mathf.Pow(10, 3));

        if (_LnToggle.isOn == true)
            UpdateGraph(_resistanceList_Ln, _temperatureList_1T, _yMaximumLn, _yMinimumLn);
        else
            UpdateGraph(_resistanceList, _temperatureList, _yMaximum, _yMinimum);
    }

    private void OnToggleChanged(bool activated)
    {
        if (_resistanceList == null)
            return;
        
        if (activated == true)
            UpdateGraph(_resistanceList_Ln, _temperatureList_1T, _yMaximumLn, _yMinimumLn);
        else
            UpdateGraph(_resistanceList, _temperatureList, _yMaximum, _yMinimum);
        
        _textCelsium.gameObject.SetActive(!activated);
    }

    private void SetMinMaxY(float minR, float maxR)
	{
		_minR = minR;
        _maxR = maxR;
		
		float yDifference = _maxR - _minR;
        
        if (yDifference <= 0)
            yDifference = 5f;
        
        _yMaximum = _maxR + (yDifference * 0.2f);
        _yMinimum = _minR - (yDifference * 0.2f);

        _yMaximumLn = Mathf.Log(_yMaximum);
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
            labelX.GetComponent<TMP_Text>().text = T[i].ToString("F1");
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
            labelY.GetComponent<TMP_Text>().text = (yMin + (normalizedValue * (yMax - yMin))).ToString("F2");
            _gameObjectList.Add(labelY.gameObject);
            
            RectTransform dashY = Instantiate(_dashTemplateY, _dashContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
            _gameObjectList.Add(dashY.gameObject);
        }
    }

    private GameObject CreatePoint(Vector2 anchoredPosition, int index)
    {
        GraphPoint point = Instantiate(_point);
        point.Init(_graphContainer, anchoredPosition, _resistanceList[index], _temperatureList[index]);
        return point.gameObject;
    }
}
