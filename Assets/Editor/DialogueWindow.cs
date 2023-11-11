using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System;

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
    }
}
