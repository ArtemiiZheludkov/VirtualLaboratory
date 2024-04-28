using UnityEngine;

namespace VirtualLaboratory
{
    public abstract class ProcessModule : MonoBehaviour, IVariant
    {
        [SerializeField] protected string _name;
        
        protected Graph _graph;
        protected ResultView _resultView;
        
        public string FullName() => _name;
        public string ButtonName() => _name;
        
        public virtual void Init(Graph graph, ResultView resultView)
        {
            _graph = graph;
            _resultView = resultView;
        }
        
        public virtual void Enable(float[] x_array, float[] y_array)
        {
            _graph.ClearGraph();
            _resultView.Clear();
            
            var (newX, newY) = PrepareData(x_array, y_array);
            _graph.UpdateGraph(newX, newY);
            
            ProcessData(newX, newY);
            ShowProcessResult();
        }
        
        public virtual void Disable()
        {
            _graph.ClearGraph();
            _resultView.Clear();
        }

        protected abstract (float[], float[]) PrepareData(float[] x_array, float[] y_array);

        protected abstract void ProcessData(float[] x_array, float[] y_array);
        
        protected abstract void ShowProcessResult();
    }
}