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
            
            Observable.FromEvent(
                    h => new UnityEngine.Events.UnityAction(h),
                    h => _button.onClick.AddListener(h), 
                    h => _button.onClick.RemoveListener(h)
                ).Subscribe(_ => ClearInputField())
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
        
        private void ClearInputField()
        {
            _inputField.text = string.Empty;
        }
        
        private async UniTask UpdateLayout()
        {
            Canvas.ForceUpdateCanvases();
            await UniTask.DelayFrame(1);
    
            float textHeight = GetTextHeight();
            float containerHeight = _microContent.rectTransform.rect.height;
            _text.rectTransform.sizeDelta = new Vector3(_microContent.rectTransform.rect.width, _microContent.rectTransform.rect.height, 1);
            _text.rectTransform.anchoredPosition = new Vector3(0, 0, 0);
    
            if (textHeight > containerHeight)
            {
                Vector2 size = _microContent.rectTransform.sizeDelta;
                size.y = textHeight + 30;
                _microContent.rectTransform.sizeDelta = size;

                Canvas.ForceUpdateCanvases();
                await UniTask.DelayFrame(1);
                
                _text.rectTransform.anchoredPosition = new Vector3(0, 0, 0);

                _scrollRect.verticalNormalizedPosition = 0f;
            }
        }

        private float GetTextHeight()
        {
            if (_text is TextMeshProUGUI tmp)
            {
                tmp.ForceMeshUpdate();
                return tmp.preferredHeight;
            }

            Debug.LogError("You use don't right text!");
            return 0;
        }
    }
}