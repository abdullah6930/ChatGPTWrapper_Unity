using AbdullahQadeer.helper;
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
        private static CircularList<BotMessage> _messages = new (ChatGPTWrapperData.Instance.MaxHistoryLimit);

        public static void ResizeCirularList(int size)
        {
            _messages.Resize(size);
        }


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
            _isInitialized = true;
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

        public static async Task<string> SendMessage(string message)
        {
            if(!_isInitialized)
            {
                Debug.Log("ChatGPTWrapper is not Initialized ..");
                Initialize();
            }
            var requestBody = new BotRequest
            {
                model = "gpt-3.5-turbo"
            };
            var chatBotMessage = new BotMessage
            {
                content = message
            };

            var wrapperData = ChatGPTWrapperData.Instance;
            if (wrapperData.RememberChatContext)
            {
                requestBody.messages = _messages.ToList();
            }
            else
            {
                requestBody.messages = new List<BotMessage>();
            }

            requestBody.messages.Add(chatBotMessage);
            StringContent requestContent;
            HttpResponseMessage response;
            try
            {
                var json = JsonUtility.ToJson(requestBody);
                requestContent = new StringContent(json, Encoding.UTF8, "application/json");
                response = await _httpClient.PostAsync("", requestContent);

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                BotResponse jsonResponse = JsonUtility.FromJson<BotResponse>(responseContent);

                var botMessage = jsonResponse.choices[0].message;
                if (ChatGPTWrapperData.Instance.RememberChatContext)
                {
                    _messages.Add(botMessage);
                }
                else
                {
                    _messages.Clear();
                }
                return botMessage.content;
            }
            catch (Exception)
            {
                return "Sorry Something is not right please contact developer.";
            }
        }

        [Serializable]
        public class BotResponse 
        {
            public string id;
            public string @object;
            public long created;
            public string model;
            public BotMessageUsage usage;
            public Choice[] choices;

            [Serializable]
            public class BotMessageUsage
            {
                public int prompt_tokens;
                public int completion_tokens;
                public int total_tokens;
            }

            [Serializable]
            public class Choice
            {
                public BotMessage message;
                public string finish_reason;
                public int index;
            }
        }

        [Serializable]
        public class BotRequest
        {
            public string model = "gpt-3.5-turbo";
            public List<BotMessage> messages;
            public float temperature = 0.7f; // 0.0 - 1.0
        }


        [Serializable]
        public class BotMessage
        {
            public string role = "user"; // useer or system
            public string content; // actual message that the user sent or received by ChatBot
        }
    }
}

