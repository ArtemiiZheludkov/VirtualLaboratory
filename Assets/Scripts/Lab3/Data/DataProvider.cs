using System;
using System.Globalization;
using UnityEngine;

namespace VirtualLaboratory.Lab3
{
    public class DataProvider
    {
        private TextAsset dataFile;
        private string[] lines;

        public void LoadData(int variant)
        {
            try
            {
                string resourcePath = $"lab3/lab3_variant_{variant}";
                dataFile = Resources.Load<TextAsset>(resourcePath);
                lines = dataFile.text.Split('\n');
                Debug.Log($"File loaded: {resourcePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error load file: {ex}");
            }
        }

        public float[] GetDataTaskOne(int variant, string name)
        {
            if (dataFile == null)
                LoadData(variant);

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine))
                    continue;

                string[] values = trimmedLine.Split(',');
                if (values[0].Trim().Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    float[] data = new float[values.Length - 1];

                    for (int i = 1; i < values.Length; i++)
                    {
                        if (float.TryParse(values[i], NumberStyles.Any, CultureInfo.InvariantCulture,
                                out float parsedValue))
                            data[i - 1] = parsedValue;
                    }

                    return data;
                }
            }

            return null;
        }

        public MagneticData GetDataTaskTwo(int variant, string nameIz)
        {
            if (dataFile == null)
                LoadData(variant);

            MagneticData data = new MagneticData(); 
            
            for (int i = 0; i < lines.Length; i++)
            {
                string trimmedLine = lines[i].Trim();
                if (string.IsNullOrEmpty(trimmedLine))
                    continue;

                string[] values = trimmedLine.Split(',');
                if (values[0].Trim().Equals(nameIz, StringComparison.OrdinalIgnoreCase))
                {
                    data.B = GetDataByLine(i+1);
                    data.I = GetDataByLine(i+2);
                    data.UPlus = GetDataByLine(i+3);
                    data.UMinus = GetDataByLine(i+4);
                }
            }

            return data;
        }

        private float[] GetDataByLine(int lineIndex)
        {
            string trimmedLine = lines[lineIndex].Trim();
            if (string.IsNullOrEmpty(trimmedLine))
                return null;

            string[] values = trimmedLine.Split(',');
            float[] data = new float[values.Length - 1];

            for (int i = 1; i < values.Length; i++)
            {
                if (float.TryParse(values[i], NumberStyles.Any, CultureInfo.InvariantCulture,
                        out float parsedValue))
                    data[i - 1] = parsedValue;
            }

            return data;
        }
    }
}