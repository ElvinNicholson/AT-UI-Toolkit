using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System.IO;

namespace DialogueEditor
{
    public class DialogueWindow : EditorWindow
    {
        public string fileName;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowWindow()
        {
            GetWindow<DialogueWindow>("Dialogue Editor");
        }

        private void OnEnable()
        {
            AddGraphview();
            AddToolbar();
        }

        private void AddGraphview()
        {
            DialogueGraphview graphview = new DialogueGraphview();
            graphview.StretchToParentSize();
            rootVisualElement.Add(graphview);
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            Label fileNameLabel = new Label();
            fileNameLabel.text = "File Name: ";

            TextField fileNameTextField = new TextField();
            fileNameTextField.value = "New Dialogue File";
            SerializedObject serializedObject = new SerializedObject(this);
            fileNameTextField.BindProperty(serializedObject.FindProperty("fileName"));

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "New Dialogue File";
            }

            Button saveButton = new Button();
            saveButton.text = "Save";
            saveButton.clicked += Save;

            Button loadButton = new Button();
            loadButton.text = "Load";
            loadButton.clicked += Load;

            toolbar.Add(fileNameLabel);
            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);
            toolbar.Add(loadButton);

            toolbar.styleSheets.Add((StyleSheet)EditorGUIUtility.Load("DialogueToolbar.uss"));

            rootVisualElement.Add(toolbar);
        }

        private void Save()
        {
            DialogueGraphview graphView = rootVisualElement.Query<DialogueGraphview>();
            graphView.Save(fileName);
        }

        private void Load()
        {
            string absolutePath = EditorUtility.OpenFilePanel("Load dialogue .asset file", "Assets", "asset");
            if (string.IsNullOrEmpty(absolutePath))
            {
                return;
            }

            string relativePath = "Assets" + absolutePath.Substring(Application.dataPath.Length);
            fileName = Path.GetFileName(relativePath);
            fileName = fileName.Substring(0, fileName.Length - 6);

            MainDialogueAsset mainDialogueAsset = GetAssetFromDatabase<MainDialogueAsset>(relativePath);
            DialogueGraphview graphView = rootVisualElement.Query<DialogueGraphview>();
            graphView.Load(mainDialogueAsset);
        }

        private T GetAssetFromDatabase<T>(string path) where T : ScriptableObject
        {
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
}
