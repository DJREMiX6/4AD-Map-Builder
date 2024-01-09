using UnityEngine;

namespace Objects.UI
{
    public class UIMessage : MonoBehaviour
    {
        [SerializeField]
        private UIMessagesId _messageId;

        public UIMessagesId MessageId => _messageId;
    }
}
