using System.Collections.Generic;
using R3;

namespace _Project.Scripts.Model
{
    public class MessagesModel
    {
        private readonly ReactiveProperty<Queue<string>> _messages;
        
        public ReadOnlyReactiveProperty<Queue<string>> Messages => _messages;

        public MessagesModel()
        {
            _messages = new ReactiveProperty<Queue<string>>(new Queue<string>());
        }

        public void DeleteLastMessage()
        {
            if (_messages.Value.Count > 10)
            {
                var newQueue = new Queue<string>(_messages.Value);
                newQueue.Dequeue();
                _messages.Value = newQueue;
            }
        }

        public void AddMessage(string message)
        {
            var newQueue = new Queue<string>(_messages.Value);
            newQueue.Enqueue(message);
            _messages.Value = newQueue;
        }

        public void ClearMessages()
        {
            _messages.Value = new Queue<string>();
        }
    }
}