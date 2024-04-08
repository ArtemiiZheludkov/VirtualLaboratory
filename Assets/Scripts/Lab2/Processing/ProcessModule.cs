using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public abstract class ProcessModule : MonoBehaviour
    {
        public string Name;
        private Graph _graph;
        
        public virtual void Init(Graph graph)
        {
            _graph = graph;
        }
        
        public virtual void Enable(float[] x_array, float[] y_array, float currentIp)
        {
            _graph.ClearGraph();
            //text clear
            
            var (newX, newY) = PrepareData(x_array, y_array);
            _graph.UpdateGraph(newX, newY, currentIp);
            
            ProcessData(newX, newY);
            ShowProcessResult();
        }
        
        public virtual void Disable()
        {
            _graph.ClearGraph();
            //text clear
        }
        
        protected abstract (float[], float[]) PrepareData(float[] x_array, float[] y_array);

        protected abstract void ProcessData(float[] x_array, float[] y_array);
        
        protected abstract void ShowProcessResult();
    }
}