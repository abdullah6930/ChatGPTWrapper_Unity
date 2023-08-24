using UnityEditor;

namespace AbdullahQadeer.ChatGPTWrapper.Editor
{
    [CustomEditor(typeof(ChatGPTWrapperData))]
    public class ChatGPTWrapperDataEditor : UnityEditor.Editor
    {
        private bool previousRememberChatContext;

        private void OnEnable()
        {
            previousRememberChatContext = ((ChatGPTWrapperData)target).RememberChatContext;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ChatGPTWrapperData data = (ChatGPTWrapperData)target;

            if (data.RememberChatContext != previousRememberChatContext)
            {
                previousRememberChatContext = data.RememberChatContext;
                if (data.RememberChatContext)
                {
                    ChatGPTManager.ResizeCirularList(data.MaxHistoryLimit);
                }
            }
        }
    }
}