using System;
using _Project.Scripts.Model;
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
        public Observable<string[]> Messages { get; private set; }
        
        public ChatViewModel(MessagesModel messagesModel,
            GameSettingsModel gameSettings)
        {
            _messagesModel = messagesModel;
            _gameSettings = gameSettings;
        }
        
        public void Initialize()
        {
            Messages = _messagesModel.Messages.Select(queue => queue.ToArray())
                .AsObservable();
            AddMessageCommand.Subscribe(message => AddMessage(message)).AddTo(_disposables);
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
        
        private void AddMessage(string message)
        {
            message = $"{message} :{_gameSettings.Nickname}\n\n";

            _messagesModel.AddMessage(message);        
        }

    }
}