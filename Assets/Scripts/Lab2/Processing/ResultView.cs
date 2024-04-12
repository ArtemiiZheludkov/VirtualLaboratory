﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class ResultView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textPrefab;
        [SerializeField] private Transform _textParent;

        private List<TMP_Text> _texts;
        
        public void Init()
        {
            if (_texts == null)
                _texts = new List<TMP_Text>();
            else
                Clear();
        }

        public void AddTextLine(string addText)
        {
            TMP_Text textLine = Instantiate(_textPrefab, _textParent, false);
            textLine.text = addText;
        }

        public void Clear()
        {
            if (_texts == null)
                return;
            
            foreach (TMP_Text obj in _texts)
                Destroy(obj.gameObject);

            _texts.Clear();
        }
    }
}