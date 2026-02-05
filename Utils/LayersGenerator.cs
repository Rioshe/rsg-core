#if UNITY_EDITOR
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace RSG
{
    public class LayersGenerator : EditorWindow
    {
        private const string OUTPUT_FILE_PATH = "Assets/_Project/Scripts/Generated/Layers.cs";
        private const string LAYER_MASK_SUFFIX = "_MASK";
        private const string LAYER_INDEX_SUFFIX = "_INDEX";
        private const int MAX_LAYER_COUNT = 32;

        [MenuItem("Tools/Generate Layers")]
        public static void GenerateLayer()
        {
            StringBuilder sb = new();

            sb.AppendLine("// This file is auto-generated. Do not modify.");
            sb.AppendLine("public static class Layers");
            sb.AppendLine("{");
            sb.AppendLine();
            for (int i = 0; i < MAX_LAYER_COUNT; i++)
            {
                string layerName = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(layerName))
                {
                    sb.AppendLine($"    // {layerName}");
                    sb.AppendLine($"    public const string {SanitizeVariableName(layerName)} = \"{layerName}\";");
                    string bitMask = ConvertIntToBitMask(i);
                    sb.AppendLine($"    public const int {SanitizeVariableName(layerName,LAYER_MASK_SUFFIX)} = {bitMask};");
                    sb.AppendLine($"    public const int {SanitizeVariableName(layerName,LAYER_INDEX_SUFFIX)} = {i};");
                }
            }
            
            sb.AppendLine();
            sb.AppendLine("}");
            
            // Write to file
            string path = Path.GetDirectoryName(OUTPUT_FILE_PATH);
            Directory.CreateDirectory(path ?? throw new InvalidOperationException());
            File.WriteAllText(OUTPUT_FILE_PATH, sb.ToString());

            AssetDatabase.Refresh();
            Debug.Log("Layers.cs generated successfully.");
        }
        
        private static string SanitizeVariableName(string name, string suffix = null)
        {
            string sanitizedName = ConvertToUpperCase(name);
            if (!string.IsNullOrEmpty(suffix))
                sanitizedName += suffix;
            
            return sanitizedName;
        }

        private static string ConvertIntToBitMask(int index)
        {
            int bitmask = 1 << index;
            return "0x" + bitmask.ToString("X8");
        }

        private static string ConvertToUpperCase(string input)
        {
            string output = "";
            bool canAdd = false;
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsLower(input[i]))
                {
                    canAdd = true;
                }

                if (i != 0 && char.IsUpper(input[i]) && canAdd)
                {
                    output += '_';
                    canAdd = false;
                }

                output += input[i];
            }

            output = output.ToUpper();
            output = Regex.Replace(output, @"\s+", "");
            return output;
        }
    }
}
#endif
