using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace TansanMilMil.Util
{
    public static class ImportLocalizationFromCSV
    {
        [MenuItem("Tools/TansanMilMilUtil/ImportLocalizationFromCSV/Import From CSV")]
        private static void ImportFromCSV()
        {
            string csvPath = EditorUtility.OpenFilePanel(
                "Select CSV file for localization import",
                Application.dataPath,
                "csv"
            );

            if (string.IsNullOrEmpty(csvPath))
            {
                Debug.LogWarning("CSV import cancelled.");
                return;
            }

            ImportLocalizationDataFromCSV(csvPath);
        }

        private static void ImportLocalizationDataFromCSV(string csvPath)
        {
            try
            {
                if (!File.Exists(csvPath))
                {
                    Debug.LogError($"CSV file not found: {csvPath}");
                    return;
                }

                string[] lines = File.ReadAllLines(csvPath);
                if (lines.Length < 2)
                {
                    Debug.LogError("CSV file must have at least 2 lines (header and data)");
                    return;
                }

                // Parse header to get locale codes
                string[] headers = ParseCSVLine(lines[0]);
                if (headers.Length < 2)
                {
                    Debug.LogError("CSV file must have at least 2 columns (Key and at least one locale)");
                    return;
                }

                string keyColumnName = headers[0];
                List<string> localeIds = new List<string>();

                for (int i = 1; i < headers.Length; i++)
                {
                    localeIds.Add(headers[i]);
                }

                Debug.Log($"Found {localeIds.Count} locales: {string.Join(", ", localeIds)}");

                // Get string table collection by CSV filename
                string csvFileName = Path.GetFileNameWithoutExtension(csvPath);
                var stringTableCollection = LocalizationEditorSettings.GetStringTableCollection(csvFileName);
                if (stringTableCollection == null)
                {
                    Debug.LogError($"String table collection '{csvFileName}' not found. Please create one first.");
                    return;
                }

                int importedCount = 0;
                int updatedCount = 0;

                // Process each data line
                for (int lineIndex = 1; lineIndex < lines.Length; lineIndex++)
                {
                    string[] values = ParseCSVLine(lines[lineIndex]);
                    if (values.Length < 2 || string.IsNullOrEmpty(values[0]))
                        continue;

                    string key = values[0];

                    // Add or update the key in the shared table data
                    var sharedTableData = stringTableCollection.SharedData;
                    var keyId = sharedTableData.GetId(key);
                    if (keyId == SharedTableData.EmptyId)
                    {
                        sharedTableData.AddKey(key);
                        keyId = sharedTableData.GetId(key);
                        importedCount++;
                    }
                    else
                    {
                        updatedCount++;
                    }

                    // Update values for each locale
                    for (int i = 1; i < values.Length && i - 1 < localeIds.Count; i++)
                    {
                        if (string.IsNullOrEmpty(values[i]))
                            continue;

                        string localeId = localeIds[i - 1];
                        var stringTable = stringTableCollection.GetTable(localeId) as StringTable;

                        if (stringTable != null)
                        {
                            var entry = stringTable.GetEntry(keyId);
                            if (entry == null)
                            {
                                entry = stringTable.AddEntry(keyId, values[i]);
                            }
                            else
                            {
                                entry.Value = values[i];
                            }

                            EditorUtility.SetDirty(stringTable);
                        }
                        else
                        {
                            Debug.LogWarning($"String table for locale '{localeId}' not found. Skipping.");
                        }
                    }
                }

                EditorUtility.SetDirty(stringTableCollection.SharedData);
                AssetDatabase.SaveAssets();

                Debug.Log($"CSV import completed successfully! Imported {importedCount} new keys, updated {updatedCount} existing keys to collection '{csvFileName}' from: {csvPath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to import CSV: {e.Message}");
            }
        }

        private static string[] ParseCSVLine(string line)
        {
            List<string> result = new List<string>();
            bool inQuotes = false;
            string currentField = "";

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        // Double quote - add single quote to field
                        currentField += '"';
                        i++; // Skip next quote
                    }
                    else
                    {
                        // Toggle quote state
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    // Field separator
                    result.Add(currentField);
                    currentField = "";
                }
                else
                {
                    currentField += c;
                }
            }

            // Add the last field
            result.Add(currentField);

            return result.ToArray();
        }
    }
}
