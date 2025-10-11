using System;
using System.Collections.Generic;
using _Project.Scripts.Model;
using Fusion;
using R3;
using Zenject;

namespace _Project.Scripts.ViewModel
{
    public class ChatViewModel : IChatViewModel, IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly GameSettingsModel _gameSettings;
        private readonly MessagesModel _messagesModel;
        
        public ReactiveCommand<string> AddMessageCommand { get; } = new();
        public Observable<Queue<string>> Messages { get; private set; }
        
        public ChatViewModel(MessagesModel messagesModel,
            GameSettingsModel gameSettings)
        {
            _messagesModel = messagesModel;
            _gameSettings = gameSettings;
        }
        
        public void Initialize()
        {
            Messages = _messagesModel.Messages;
            AddMessageCommand.Subscribe(message => AddMessage(message)).AddTo(_disposables);
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
        
        private void AddMessage(string message)
        {
            RPC_SendMessage(message);
        }
        
        private  void RPC_SendMessage(string message, RpcInfo info = default)
        {
            message = $"{message} :{_gameSettings.Nickname}\n\n";

            _messagesModel.AddMessage(message);
        }
    }
}