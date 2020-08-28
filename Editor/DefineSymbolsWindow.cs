using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Playrika.GameFoundation.Editor
{
    public class DefineSymbolsWindow : EditorWindow
    {
        private const string _fileName = "DisabledScriptingDefineSymbols";

        private Vector2 _scrollPosition;

        private List<SymbolData> _enabledSymbols;

        private List<SymbolData> _disabledSymbols;

        private bool _isUnsavedChangesExist;


        [MenuItem("GameFoundation/Windows/Define Symbols")]
        public static void ShowWindow()
        {
            var window = GetWindow<DefineSymbolsWindow>("Define Symbols");
            window.minSize = new Vector2(0f, 0f);
        }


        private void OnEnable()
        {
            _disabledSymbols = new List<SymbolData>();
            _enabledSymbols = new List<SymbolData>();
            _isUnsavedChangesExist = false;

            var path = Path.Combine(Application.dataPath, _fileName);
            if (File.Exists(path))
            {
                var serializedDisabledSymbols = File.ReadAllText(path);
                AddSymbols(serializedDisabledSymbols, _disabledSymbols, false);
            }

            var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            var serializedEnabledSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            AddSymbols(serializedEnabledSymbols, _enabledSymbols, true);
        }

        private void OnGUI()
        {
            if (EditorApplication.isCompiling)
            {
                DrawCompilingPanel();
                return;
            }

            DrawMainPanel();
        }


        private void DrawCompilingPanel()
        {
            var style = new GUIStyle { margin = new RectOffset(10, 10, 10, 10) };

            GUILayout.BeginHorizontal(style);
            EditorGUILayout.HelpBox("Compiling..", MessageType.Info);
            GUILayout.EndHorizontal();
        }

        private void DrawMainPanel()
        {
            var scrollViewStyle = new GUIStyle { padding = new RectOffset(10, 10, 10, 10)} ;
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, scrollViewStyle, GUILayout.ExpandWidth(true));

            DrawSymbolsList(_enabledSymbols, _disabledSymbols, "Enabled Symbols:");
            EditorGUILayout.Space(10);
            DrawSymbolsList(_disabledSymbols, _enabledSymbols, "Disabled Symbols:");

            if (_isUnsavedChangesExist)
            {
                if (_enabledSymbols.Count > 0 || _disabledSymbols.Count > 0)
                    EditorGUILayout.Space(10);

                EditorGUILayout.HelpBox("Don't forget to save your changes!", MessageType.Warning);
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("+ Add"))
            {
                _enabledSymbols.Add(new SymbolData(string.Empty, true));
                _isUnsavedChangesExist = true;
            }

            if (GUILayout.Button("Save"))
                Save();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }

        private void DrawSymbolsList(List<SymbolData> symbols, List<SymbolData> oppositeSymbols, string header)
        {
            var toggleStyle = new GUIStyle(EditorStyles.toggle) { fixedWidth = 15f };
            var deleteButtonStyle = new GUIStyle(EditorStyles.miniButton) { fixedWidth = 65f };

            EditorGUILayout.LabelField(header);

            if (symbols.Count <= 0)
            {
                var noSymbolsMessageStyle = new GUIStyle(EditorStyles.label) { fontSize = 10 };
                EditorGUILayout.LabelField("There are no symbols.", noSymbolsMessageStyle);
            }
            else
            {
                foreach (var symbolData in symbols.ToList())
                {
                    GUILayout.BeginHorizontal();

                    var newEnabledValue = GUILayout.Toggle(symbolData.enabled, "", toggleStyle);
                    if (newEnabledValue != symbolData.enabled)
                    {
                        symbolData.enabled = newEnabledValue;
                        symbols.Remove(symbolData);
                        oppositeSymbols.Add(symbolData);
                        _isUnsavedChangesExist = true;
                    }

                    var newValue = GUILayout.TextField(symbolData.value);
                    if (newValue != symbolData.value)
                    {
                        symbolData.value = newValue;
                        _isUnsavedChangesExist = true;
                    }

                    if (GUILayout.Button("Delete", deleteButtonStyle))
                    {
                        symbols.Remove(symbolData);
                        _isUnsavedChangesExist = true;
                    }

                    GUILayout.EndHorizontal();
                }
            }
        }

        private void Save()
        {
            var path = Path.Combine(Application.dataPath, _fileName);
            if (_disabledSymbols.Count <= 0)
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            else
                File.WriteAllText(path, SerializeSymbols(_disabledSymbols, false));

            var serializedEnabledSymbols = SerializeSymbols(_enabledSymbols, true);
            var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, serializedEnabledSymbols);

            _isUnsavedChangesExist = false;
        }


        private void AddSymbols(string serializedData, List<SymbolData> targetList, bool enabled)
        {
            if (string.IsNullOrEmpty(serializedData))
                return;

            var symbols = serializedData.Split(';');
            if (symbols.Length <= 0)
                return;

            foreach (var symbol in symbols)
            {
                if (string.IsNullOrEmpty(symbol))
                    continue;

                targetList.Add(new SymbolData(symbol, enabled));
            }
        }

        private string SerializeSymbols(List<SymbolData> symbols, bool serializeEnabledSymbols)
        {
            var stringBuilder = new StringBuilder();

            foreach (var symbolData in symbols)
            {
                if (serializeEnabledSymbols && !symbolData.enabled || !serializeEnabledSymbols && symbolData.enabled)
                    continue;

                stringBuilder.Append(Regex.Replace(symbolData.value, "[^a-zA-Z0-9_]", ""));
                stringBuilder.Append(";");
            }

            return stringBuilder.ToString();
        }


        private class SymbolData
        {
            public string value { get; set; }

            public bool enabled { get; set; }

            public SymbolData(string value, bool enabled)
            {
                this.value = value;
                this.enabled = enabled;
            }
        }
    }
}