using _Project.Scripts.ViewModel;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.View
{
    public class ConnectSceneView : MonoBehaviour
    {
        [SerializeField] private Button _buttonConnectAsHost;
        [SerializeField] private Button _buttonConnectAsPlayer;
        [SerializeField] private Button _saveName;
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
                    h => _buttonConnectAsHost.onClick.AddListener(h), 
                    h => _buttonConnectAsHost.onClick.RemoveListener(h)
                ).Subscribe(_ => _iConnectViewModel.ConnectAsHostCommand.Execute(Unit.Default))
                .AddTo(_disposables);
            
            Observable.FromEvent(
                    h => new UnityEngine.Events.UnityAction(h),
                    h => _buttonConnectAsPlayer.onClick.AddListener(h),
                    h => _buttonConnectAsPlayer.onClick.RemoveListener(h)
                ).Subscribe(_ => _iConnectViewModel.ConnectAsPlayerCommand.Execute(Unit.Default))
                .AddTo(_disposables);
            
            Observable.FromEvent(
                    h => new UnityEngine.Events.UnityAction(h),
                    h => _saveName.onClick.AddListener(h),
                    h => _saveName.onClick.RemoveListener(h)
                ).Subscribe(_ => _iConnectViewModel.SetNameCommand.Execute(_inputField.text))
                .AddTo(_disposables);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}