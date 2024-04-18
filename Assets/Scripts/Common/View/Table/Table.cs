using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VirtualLaboratory
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private TMP_Text _xText;
        [SerializeField] private TMP_Text _yText;
        [SerializeField] private TableElement _rowPrefab;
        [SerializeField] private Transform _tableContent;

        private List<TableElement> _elements;

        public void Init(in int maxPoints)
        {
            if (_elements != null)
            {
                foreach (TableElement element in _elements)
                    Destroy(element.gameObject);
                
                _elements.Clear();
            }
            else
                _elements = new List<TableElement>();
            
            for (int i = 0; i <= maxPoints; i++)
                CreateElement();
        }
        
        public void HideTable()
        {
            foreach (TableElement element in _elements)
                element.ClearText();
        }
    
        public void UpdateRow(int index, float x, float y)
        {
            if (index < 0 || index > _elements.Count)
                return;
            
            _elements[index].SetText(in x, in y);
        }

        public void SetHeaders(string xName, string yName)
        {
            if (xName != null)
                _xText.text = xName;

            if (yName != null)
                _yText.text = yName;
        }
        
        private void CreateElement()
        {
            TableElement element = Instantiate(_rowPrefab, _tableContent);
            element.Init();
            _elements.Add(element);
        }
    }
}