#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace AbdullahQadeer.ChatGPTWrapper.Example
{
    public class ChatGPTLogExample : MonoBehaviour
    {
        public string message;

        void Start()
        {
            ChatGPTManager.Initialize();
        }

        public async void SendMessage(string message)
        {
            Debug.Log(message);
            var response = await ChatGPTManager.SendMessage(message);
            Debug.Log(response);
        }
#if UNITY_EDITOR

        [CustomEditor(typeof(ChatGPTLogExample))]
        public class ChatGPTLogExampleEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                // Draw the default inspector GUI
                DrawDefaultInspector();

                // Get a reference to the script being edited
                var myScript = (ChatGPTLogExample)target;

                // Add custom GUI elements here
                if (GUILayout.Button("Send Button"))
                {
                    myScript.SendMessage(myScript.message);
                }
            }
        }
#endif
    }
}
