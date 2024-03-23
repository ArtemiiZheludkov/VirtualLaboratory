using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace VirtualLaboratory.Lab2
{
    public class DataProvider
    {
        public List<float> GetData(int variant, string name)
        {
            try
            {
                Debug.Log($"Try load: {name}");
                
                string resourcePath = $"lab2/lab2_variant_{variant}";
                
                TextAsset dataFile = Resources.Load<TextAsset>(resourcePath);
                
                string[] lines = dataFile.text.Split('\n');
                List<float> data = new List<float>();
            
                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    if (string.IsNullOrEmpty(trimmedLine))
                        continue;

                    string[] values = trimmedLine.Split(',');
                    if (values[0].Trim().Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        for (int i = 1; i < values.Length; i++)
                        {
                            if (float.TryParse(values[i], NumberStyles.Any, CultureInfo.InvariantCulture, out float parsedValue))
                                data.Add(parsedValue);
                        }
                        
                        Debug.Log($"Data loaded: {name}\n" + $"Count = {data.Count}");
                        break;
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error load data: {ex}");
                return null;
            }
        }
    }
}