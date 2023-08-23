using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AbdullahQadeer.ChatGPTWrapper
{
    [System.Serializable]
    public class ApiKeyData
    {
        public string apiKey;
    }

    public static class ChatGPTManager
    {
        private static string _apiKey = "";
        private static readonly string _url = "https://api.openai.com/v1/chat/completions";
        private static HttpClient _httpClient;

        private static bool _isInitialized;

        public static void Initialize()
        {
            if(_isInitialized) return;

            GetAPIKey();

            Debug.Log("Initializing ChatGPTWrapper ..");
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_url)
            };
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            Debug.Log("Initialized ChatGPTWrapper ..");
        }

        private static void GetAPIKey()
        {
            // Path to the Resources folder
            string resourcesPath = Application.dataPath + "/Resources";

            // Path to the JSON file
            string jsonFilePath = Path.Combine(resourcesPath, "ChatGPTWrapperApiKey.json");

            // Write the JSON data to the file
            var text = File.ReadAllText(jsonFilePath);
            var apiKeyData = JsonUtility.FromJson<ApiKeyData>(text);
            _apiKey = apiKeyData.apiKey;
        }

        public static async Task<string> SendMessage(string message, List<ChatBotMessage> chatBotMessages = null)
        {
            if(!_isInitialized)
            {
                Debug.Log("ChatGPTWrapper is not Initialized ..");
                Initialize();
            }
            var requestBody = new ChatBotRequestBody
            {
                model = "gpt-3.5-turbo"
            };
            var chatBotMessage = new ChatBotMessage
            {
                role = "user",
                content = message
            };

            if(chatBotMessages != null)
            {
                requestBody.messages = chatBotMessages;
            }
            else
            {
                requestBody.messages = new List<ChatBotMessage>();
            }

            requestBody.messages.Add(chatBotMessage);
            StringContent requestContent;
            HttpResponseMessage response;

            try
            {

                requestContent = new StringContent(JsonUtility.ToJson(requestBody), Encoding.UTF8, "application/json");
                response = await _httpClient.PostAsync("", requestContent);

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                ChatBotResponseBody jsonResponse = JsonUtility.FromJson<ChatBotResponseBody>(responseContent);
                return jsonResponse.Choices[0].Message.content;
            }
            catch (Exception ex)
            {
                return "Sorry Something went Wrong " + ex;
            }
        }

        [Serializable]
        public class ChatBotResponseBody 
        {
            public string Id;
            public string Object;
            public long Created;
            public string Model;
            public BotMessageUsage Usage;
            public Choice[] Choices;

            [Serializable]
            public class BotMessageUsage
            {
                public int PromptTokens;
                public int CompletionTokens;
                public int TotalTokens;
            }

            [Serializable]
            public class Choice
            {
                public ChatBotMessage Message;
                public string FinishReason;
                public int Index;
            }
        }

        [Serializable]
        public class ChatBotRequestBody
        {
            public string model = "gpt-3.5-turbo";
            public List<ChatBotMessage> messages;
            public float temperature = 0.7f; // 0.0 - 1.0
        }


        [Serializable]
        public class ChatBotMessage
        {
            public string role = "user"; // useer or system
            public string content; // actual message that the user sent or received by ChatBot
        }
    }
}

