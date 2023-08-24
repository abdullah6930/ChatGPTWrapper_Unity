using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbdullahQadeer.ChatGPTWrapper
{
    //[CreateAssetMenu(fileName = "ChatGPTWrapperData", menuName = "ScriptableObjects/ChatGPTWrapperData", order = 1)]
    public class ChatGPTWrapperData : ScriptableObject
    {
        #region Singleton
        private static ChatGPTWrapperData _instance;
        public static ChatGPTWrapperData Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Resources.Load<ChatGPTWrapperData>("ChatGPTWrapperData");
                return _instance;
            }
        }
        #endregion

        [SerializeField] private bool rememberChatContext; // Uses more data
        [SerializeField] private int maxHistoryLimit = 3; // Remember context of conversation upto the number provided 

        public bool RememberChatContext => rememberChatContext;
        public int MaxHistoryLimit 
        { 
            get 
            {
                if (maxHistoryLimit < 0)
                    maxHistoryLimit = 0;
                return maxHistoryLimit;
            } 
        }
    }
}
