using _Project.Scripts.ViewModel;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.View
{
    public class JoinView : MonoBehaviour
    {
        [SerializeField] private Button _buttonAgree;
        [SerializeField] private Button _buttonExit;
        [SerializeField] private TMP_InputField _inputField;
        
        private IConnectViewModel _iConnectViewModel;
        
        private readonly CompositeDisposable _disposables = new();

        [Inject]
        private void Construct(IConnectViewModel iConnectViewModel)
        {
            _iConnectViewModel = iConnectViewModel;
        }

        private void Start()
        {
            Observable.FromEvent(
                    h => new UnityEngine.Events.UnityAction(h),
                    h => _buttonAgree.onClick.AddListener(h), 
                    h => _buttonAgree.onClick.RemoveListener(h)
                ).Subscribe(_ => _iConnectViewModel.ConnectAsPlayerCommand.Execute(_inputField.text))
                .AddTo(_disposables);
            
            _iConnectViewModel.OnPanelActivated.Subscribe(_ => Activate()).AddTo(_disposables);
            _buttonExit.onClick.AddListener(Deactivate);

            Deactivate();
        }

        private void Activate()
        {
            gameObject.SetActive(true);
        }

        private void Deactivate()
        {
            _inputField.text = "";
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _buttonExit.onClick.RemoveListener(Deactivate);
            _disposables.Dispose();
        }
    }
}