using UnityEngine;

namespace VirtualLaboratory
{
    public class WindowGraph_old : MonoBehaviour
    {
        [SerializeField] private GraphPoint _point;
        [SerializeField] private Transform _points;
        [SerializeField] private float _maxY;
        [SerializeField] private float _maxX;

        private float _startR, _startT;
        private float _maxR, _maxT;
        private float _nextT;
        
        public void Init(float startR, float maxR, float startT, float maxT)
        {
            _startR = startR;
            _startT = startT;
            _maxR = maxR;
            _maxT = maxT;
            _nextT = startT;
            
            foreach (Transform child in _points.transform)
                Destroy(child.gameObject);
        }
        
        public void AddDot(float R, float T)
        {
            if (_nextT > T)
                return;

            //float X = (T - _startT) / (_maxT - _startT) * _maxX;
            //float Y = (R - _startR) / (_maxR - _startR) * _maxY;
            //X += _startT;
            //Y += _startR;
            
            float normalizedX = (T - 0f) / (_maxT - 0f);
            float normalizedY = (R - (_startR * 0.9f)) / (_maxR - (_startR * 0.9f));
            float X = (normalizedX * (_maxX - 0f)) + 0f;
            float Y = (normalizedY * (_maxY - 0f)) + 0f;
            
            GraphPoint point = Instantiate(_point, parent:_points);
            //point.Init(_points, X, Y);
            //GameObject prefab = new GameObject("dot", typeof(GraphPoint));
            
            _nextT += 2f;
        }
    }
}