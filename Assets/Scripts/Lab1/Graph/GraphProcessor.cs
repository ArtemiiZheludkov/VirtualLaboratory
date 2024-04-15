using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualLaboratory.Lab1
{
    [Serializable]
    public class GraphProcessor
    {
        [SerializeField] private TMP_Text T_text;
        [SerializeField] private TMP_Text R_text;
        
        [SerializeField] private Image _LSMLine;
        [SerializeField] private TMP_Text _a_text;
        [SerializeField] private TMP_Text _b_text;

        [SerializeField] private GameObject _LinearFunction;
        
        [SerializeField] private TMP_Text _formula;
        [SerializeField] private TMP_Text _aСonclusions;
        [SerializeField] private TMP_Text _bСonclusions;
        
        private MaterialType _material;
        private bool _isLinear;

        private float _R0, _a, _b;
        private string alpha = "\u03B1", celsius = "\u00b0C";

        public void Init(MaterialType materialType)
        {
            _material = materialType;
            _isLinear = false;
            
            _a = 0f;
            _b = 0f;
            
            _a_text.text = "---";
            _b_text.text = "---";
            _aСonclusions.text = "---";
            _bСonclusions.text = "---";

            if (_material == MaterialType.Metal)
            {
                _formula.text = "R=R<sub>0</sub> + R<sub>0</sub>*" + alpha + "*t";
                _LinearFunction.SetActive(false);
            }
            else
            {
                _formula.text = "R=R<sub>0</sub> * e<sup>Eg/2kT</sup>";
                _LinearFunction.SetActive(true);
            }
        }

        public void CreateLSMLine(List<float> R, List<float> T, float yMax, float yMin, float xSize, float graphHeight)
        {
            if (R.Count < 2)
                return;

            if (_material == MaterialType.Metal)
                _R0 = R[0];
            
            float a, b;
            LinearRegressionCalculation(R, T, out a, out b);

            float x1 = xSize;
            float x2 = xSize + (R.Count - 1) * xSize;
            float y1 = a * T[0] + b;
            float y2 = a * T[^1] + b;

            Vector2 A = new Vector2(x1, ((y1 - yMin) / (yMax - yMin)) * graphHeight);
            Vector2 B = new Vector2(x2, ((y2 - yMin) / (yMax - yMin)) * graphHeight);

            RectTransform rectTransform = _LSMLine.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.anchoredPosition = A;
            rectTransform.sizeDelta = new Vector2(Vector3.Distance(A, B + (B * 0.2f)), 7f);

            float angle = Mathf.Atan2(B.y - A.y, B.x - A.x) * Mathf.Rad2Deg;
            rectTransform.rotation = Quaternion.Euler(0, 0, angle);
            
            _a = a;
            _b = b;
            
            if (_material == MaterialType.Semiconductor)
                _a = a * 1e+3f;
            
            _a_text.text = _a.ToString();
            _b_text.text = _b.ToString();
            
            _LSMLine.gameObject.SetActive(true);
            CreateСonclusions();
        }
        
        public void LinearRegressionCalculation(List<float> Y, List<float> X, out float a, out float b)
        {
            int n = X.Count;

            float sumX = X.Sum();
            float sumY = Y.Sum();
            float sumXY = X.Zip(Y, (x, y) => x * y).Sum();
            float sumXSquare = X.Sum(x => x * x);

            a = (n * sumXY - sumX * sumY) / (n * sumXSquare - sumX * sumX);
            b = (sumY - a * sumX) / n;
        }

        public void OnLinearToggle(bool activated)
        {
            _isLinear = activated;
            
            if (activated == true)
            {
                T_text.text = "1/T (K)*10<sup>3</sup>";
                R_text.text = "LnR (Om)";
            }
            else
            {
                T_text.text = "T (K)";
                R_text.text = "R (Om)";
            }
        }
        
        public void OnLSMToggle(bool activated)
        {
            if (activated != true)
            {
                _a_text.text = "---";
                _b_text.text = "---";
                _aСonclusions.text = "---";
                _bСonclusions.text = "---";
                _LSMLine.gameObject.SetActive(false);
            }
        }

        private void CreateСonclusions()
        {
            if (_material == MaterialType.Metal)
            {
                float a = _a / _R0;
                _formula.text = $"R=R<sub>0</sub> + R<sub>0</sub>*{alpha}*t";
                _aСonclusions.text = $"a=R<sub>0</sub>*{alpha}=>{alpha}=a/R<sub>0</sub>=" +a.ToString("F5") + $" (1/{celsius})";
                _bСonclusions.text = $"b=R<sub>0</sub>={_b}";
            }
            else if (_material == MaterialType.Semiconductor && _isLinear == false)
            {
                _formula.text = "R=R<sub>0</sub> * e<sup>Eg/2kT</sup>";
                _aСonclusions.text = "---";
                _bСonclusions.text = "---";
            }
            else if (_material == MaterialType.Semiconductor)
            {
                float Eg = 2 * _a * 8.617333262145e-5f;
                float eb = Mathf.Exp(_b);
                _formula.text = "LnR=LnR<sub>0</sub> * Eg/2kT";
                _aСonclusions.text = "a=Eg/2kT=>Eg=2a*k=" + Eg.ToString("F3") + " eV";
                _bСonclusions.text = $"b=LnR<sub>0</sub>=>R<sub>0</sub>=e<sup>b</sup>={eb}";
            }
        }
    }
}