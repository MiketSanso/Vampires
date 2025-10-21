using System;
using _Project.Scripts.Model;
using R3;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.ViewModel
{
    public class ChatViewModel : IChatViewModel, IInitializable, IDisposable
    {
        private  MessagesModel _messagesModel;
        
        private readonly CompositeDisposable _disposables = new();
        private readonly GameSettingsModel _gameSettings;
        
        public ReactiveCommand<string> AddMessageCommand { get; } = new();
        public Observable<string[]> Messages { get; private set; }
        
        public ChatViewModel(GameSettingsModel gameSettings)
        {
            _gameSettings = gameSettings;
        }
        
        public void Initialize()
        {
            AddMessageCommand.Subscribe(message => AddMessage(message)).AddTo(_disposables);
        }

        public void InitializeNetworkChatHandler(MessagesModel messagesModel)
        {
            _messagesModel = messagesModel;
            Messages = _messagesModel.Messages;
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
        
        private void AddMessage(string message)
        {
            message = $"{message} :{_gameSettings.Nickname}\n\n";

            _messagesModel.RPC_AddMessage(message);        
        }

    }
}