using System;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class PlasmaTaskTwo : ProcessModule
    {
        private float voltage_plasma;
        
        protected override (float[], float[]) PrepareData(float[] x_array, float[] y_array)
        {
            int indexI0 = FindIndexZero(y_array);
            
            voltage_plasma = x_array[indexI0];

            int newLength = x_array.Length - indexI0;
            int newStart = indexI0;
            
            float[] newX = new float[newLength];
            float[] newY = new float[newLength];
            Array.Copy(x_array, newStart, newX, 0, newLength);
            Array.Copy(y_array, newStart, newY, 0, newLength);
            
            return (newX, newY);
        }

        protected override void ProcessData(float[] x_array, float[] y_array)
        {
        }

        protected override void ShowProcessResult()
        {
        }
        
        private int FindIndexZero(float[] array)
        {
            int index0 = 0;

            for (int i = 0; i < array.Length; i++)
            {
                if (Mathf.Abs(array[i]) < Mathf.Abs(array[index0]))
                    index0 = i;
                else if (Mathf.Approximately(Mathf.Abs(array[i]), Mathf.Abs(array[index0])) && array[i] > array[index0])
                    index0 = i;
            }

            return index0;
        }
    }
}