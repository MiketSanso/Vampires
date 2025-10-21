using System.Collections.Generic;
using _Project.Scripts.ViewModel;
using Fusion;
using R3;
using TMPro;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

namespace _Project.Scripts.View
{
    public class ChatView : NetworkBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private TMP_Text _text;
        
        private IChatViewModel _iChatViewModel;
        
        private readonly CompositeDisposable _disposables = new();

        [Inject]
        private void Construct(IChatViewModel iChatViewModel)
        {
            _iChatViewModel = iChatViewModel;
        }

        private void Start()
        {
            Observable.FromEvent(
                    h => new UnityEngine.Events.UnityAction(h),
                    h => _button.onClick.AddListener(h), 
                    h => _button.onClick.RemoveListener(h)
                ).Subscribe(_ => _iChatViewModel.AddMessageCommand.Execute(_inputField.text))
                .AddTo(_disposables);

            _iChatViewModel.Messages.Subscribe(messages => Rpc_UpdateUI(messages));
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
        private void Rpc_UpdateUI(Queue<string> messages)
        {
            string newText = string.Empty;

            foreach (string message in messages)
            {
                newText += message;
            }

            _text.text = newText;
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}