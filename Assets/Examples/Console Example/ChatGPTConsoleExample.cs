#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace AbdullahQadeer.ChatGPTWrapper.Example
{
    public class ChatGPTConsoleExample : MonoBehaviour
    {
        public string message;

        void Start()
        {
            ChatGPTManager.Initialize();
        }

        public async void SendMessageToBot(string message)
        {
            Debug.Log("User : " + message);
            var response = await ChatGPTManager.SendMessage(message);
            Debug.Log("Bot : " + response);
        }
#if UNITY_EDITOR

        [CustomEditor(typeof(ChatGPTConsoleExample))]
        public class ChatGPTConsoleExampleEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                // Draw the default inspector GUI
                DrawDefaultInspector();

                // Get a reference to the script being edited
                var myScript = (ChatGPTConsoleExample)target;

                // Add custom GUI elements here
                if (GUILayout.Button("Send Button"))
                {
                    myScript.SendMessageToBot(myScript.message);
                }
            }
        }
#endif
    }
}
