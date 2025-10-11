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

        public void AddMessage(string message)
        {
            var newQueue = new Queue<string>(_messages.Value);
            newQueue.Enqueue(message);
            _messages.Value = newQueue;
        }
        
        public void UpdateAllMessages(string[] messages)
        {
            var newQueue = new Queue<string>(_messages.Value);
            
            foreach (string message in messages)
                newQueue.Enqueue(message);
            
            _messages.Value = newQueue;

            DeleteLastMessage();
        }
        
        private void DeleteLastMessage()
        {
            if (_messages.Value.Count > 10)
            {
                var newQueue = new Queue<string>(_messages.Value);
                newQueue.Dequeue();
                _messages.Value = newQueue;
            }
        }
    }
}