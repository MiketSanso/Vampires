using _Project.Scripts.ViewModel;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

namespace _Project.Scripts.View
{
    public class ChatView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _microContent;
        
        private IChatViewModel _iChatViewModel;
        
        private readonly CompositeDisposable _disposables = new();

        [Inject]
        private void Construct(IChatViewModel iChatViewModel)
        {
            _iChatViewModel = iChatViewModel;
        }

        private void Start()
        {
            if (_iChatViewModel == null)
            {
                var diContainer = FindObjectOfType<SceneContext>()?.Container;
                if (diContainer != null)
                {
                    _iChatViewModel = diContainer.Resolve<IChatViewModel>();
                }
            }
            
            Observable.FromEvent(
                    h => new UnityEngine.Events.UnityAction(h),
                    h => _button.onClick.AddListener(h), 
                    h => _button.onClick.RemoveListener(h)
                ).Subscribe(_ => _iChatViewModel.AddMessageCommand.Execute(_inputField.text))
                .AddTo(_disposables);

            _iChatViewModel.Messages.Subscribe(messages => 
            {
                UpdateUI(messages);
            }).AddTo(_disposables);
        }
        
        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        private void UpdateUI(string[] messages)
        {
            string newText = string.Empty;

            foreach (string message in messages)
            {
                newText += message;
            }

            _text.text = newText;

            UpdateLayout().Forget();
        }
        
        private async UniTask UpdateLayout()
        {
            float textHeight = _text.preferredHeight;
            float containerHeight = _microContent.rectTransform.rect.height;

            if (textHeight > containerHeight)
            {
                Vector2 size = _microContent.rectTransform.sizeDelta;
                size.y = textHeight;
                _microContent.rectTransform.sizeDelta = size;

               // await UniTask.DelayFrame(1);

               // LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRect.content);
                
                _scrollRect.verticalNormalizedPosition = 0f;
            }
        }
    }
}