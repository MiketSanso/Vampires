using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.ViewModel;
using Fusion;
using R3;
using UnityEngine;
using Zenject;

namespace _Project.Scripts
{
    public class MessagesModel : NetworkBehaviour
    {
        [Networked, Capacity(20)]
        private NetworkArray<NetworkString<_64>> _networkMessages { get; } = default;
        
        private ChatViewModel _chatViewModel;

        public ReadOnlyReactiveProperty<string[]> Messages => _messages;
        private readonly ReactiveProperty<string[]> _messages = new();

        [Inject]
        private void Construct(ChatViewModel chatViewModel)
        {
            _chatViewModel = chatViewModel;
        }
        
        private void Start()
        {
            base.Spawned();
            
            _chatViewModel.InitializeNetworkChatHandler(this);
            UpdateLocalMessages();
        }
    
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_AddMessage(string message)
        {
            List<string> newArray = new List<string>();

            for (int i = 0; i < _networkMessages.Length; i++)
            {
                string oldMessage = _networkMessages[i].ToString();
                if (!string.IsNullOrEmpty(oldMessage))
                {
                    newArray.Add(oldMessage);
                }
            }
            
            newArray.Add(message);

            if (newArray.Count > 20)
                newArray.RemoveAt(0);

            for (int i = 0; i < newArray.Count; i++)
            {
                _networkMessages.Set(i, newArray[i]);
            }
            
            RPC_UpdateAllClients();
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_UpdateAllClients()
        {
            UpdateLocalMessages(); 
        }
        
        private void UpdateLocalMessages()
        {
            var messages = _networkMessages
                .Where(msg => !string.IsNullOrEmpty(msg.ToString()))
                .Select(msg => msg.ToString())
                .ToArray();

            _messages.Value = messages;
        }
    }
}