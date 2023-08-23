using System.IO;
using UnityEditor;
using UnityEngine;

namespace AbdullahQadeer.ChatGPTWrapper.Editor
{
    public class ChatGPTWrapperEditor : EditorWindow
    {
        private string apiKey = "";

        [MenuItem("ChatGPT WrapperEditor/Save API Key")]
        private static void ShowWindow()
        {
            GetWindow<ChatGPTWrapperEditor>("API Key");
        }

        private void OnGUI()
        {
            apiKey = EditorGUILayout.TextField("API Key:", apiKey);

            if (GUILayout.Button("Save API Key"))
            {
                SaveApiKeyToJson(apiKey);
            }
        }

        private void SaveApiKeyToJson(string apiKey)
        {
            // Create an instance of the serializable class
            var apiKeyData = new ApiKeyData
            {
                apiKey = apiKey
            };

            // Convert the dictionary to JSON format
            string jsonData = JsonUtility.ToJson(apiKeyData, true);

            // Path to the Resources folder
            string resourcesPath = Application.dataPath + "/Resources";

            // Path to the JSON file
            string jsonFilePath = Path.Combine(resourcesPath, "ChatGPTWrapperApiKey.json");

            // Write the JSON data to the file
            File.WriteAllText(jsonFilePath, jsonData);

            Debug.Log("ChatGPTWrapper API Key saved to Resources/ChatGPTWrapperApiKey.json");
        }
    }
}