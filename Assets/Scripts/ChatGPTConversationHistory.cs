using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbdullahQadeer.ChatGPTWrapper
{
    public class ChatGPTConversationHistory
    {
        private List<string> _messages = new List<string>();
        
        public List<string> FetchMessages()
        {
            return _messages;
        }
    }
}
