using _Project.Scripts.Model;
using Fusion;
using R3;
using UnityEngine;
using Zenject;

namespace _Project.Scripts
{
    public class NetworkChatHandler : NetworkBehaviour
    {
        [HideInInspector, Networked] 
        public NetworkArray<NetworkString<_64>> Messages => default;
        
        private MessagesModel _messagesModel;
        
        private readonly CompositeDisposable _disposables = new();

        [Inject]
        private void Construct(MessagesModel messagesModel)
        {
            _messagesModel = messagesModel;
        }

        private void Start()
        {
            if (_messagesModel == null)
            {
                var diContainer = FindObjectOfType<SceneContext>()?.Container;
                if (diContainer != null)
                {
                    _messagesModel = diContainer.Resolve<MessagesModel>();
                }
            }

            _messagesModel.Messages.Subscribe(messages => RPC_AddMessage(messages.ToArray()))
                .AddTo(_disposables);
        }
        
        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_AddMessage(string[] messages)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                if (i < Messages.Length)
                {
                    Messages.Set(i, messages[i]);
                }
            }
    
            GetMessages();
        }
    
        private void GetMessages()
        {
            var result = new string[Messages.Length];
            for (int i = 0; i < Messages.Length; i++)
            {
                result[i] = Messages[i].ToString();
            }
            _messagesModel.UpdateAllMessages(result);
        }
    }
}