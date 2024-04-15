using System;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    [Serializable]
    public class DefaultVariant : IVariant
    {
        [SerializeField] protected string _fullName;
        [SerializeField] protected string _buttonName;
        
        public string FullName() => _fullName;
        public string ButtonName() => _buttonName;
    }
}