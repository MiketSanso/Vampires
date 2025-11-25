using System;
using System.Text;
using _Project.Scripts.Model;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Services;
using _Project.Scripts.ViewModel;
using Fusion;
using R3;
using Zenject;

public class ConnectViewModel : IConnectViewModel, IInitializable, IDisposable
{
    private readonly Subject<Unit> _onPanelActivated = new Subject<Unit>();
    public Observable<Unit> OnPanelActivated => _onPanelActivated;
    
    private readonly GameSettingsModel _gameSettingsModel;
    private readonly SceneChanger _sceneChanger;
    private readonly CompositeDisposable _disposables = new();
    private readonly GameSettingsData _gameSettingsData;
    
    public ReactiveCommand<Unit> ConnectAsHostCommand { get; } = new();
    public ReactiveCommand<string> ConnectAsPlayerCommand { get; } = new();
    public ReactiveCommand<string> SetNameCommand { get; } = new();
    public ReactiveCommand<Unit> ShowPanelCodeCommand { get; } = new();

    public ConnectViewModel(GameSettingsModel gameSettingsModel,
        SceneChanger sceneChanger,
        GameSettingsData gameSettingsData)
    {
        _gameSettingsModel = gameSettingsModel;
        _sceneChanger = sceneChanger;
        _gameSettingsData = gameSettingsData;
    }

    public void Initialize()
    {
        ConnectAsHostCommand.Subscribe(_ => HandleConnectAsHost()).AddTo(_disposables);
        ConnectAsPlayerCommand.Subscribe(code => HandleConnectAsPlayer(code)).AddTo(_disposables); 
        SetNameCommand.Subscribe(name => HandleSetName(name)).AddTo(_disposables);
        ShowPanelCodeCommand.Subscribe(_ => _onPanelActivated.OnNext(Unit.Default)).AddTo(_disposables);
    }
    
    public void Dispose()
    {
        _disposables.Dispose();
    }

    private void HandleSetName(string name)
    {
        _gameSettingsModel.Nickname = name;
    }
    
    private void HandleConnectAsHost()
    {
        if (!string.IsNullOrWhiteSpace(_gameSettingsModel.Nickname))
        {
            _gameSettingsModel.GameMode = GameMode.Host;
            _gameSettingsModel.SessionCode = GenerateRandomCode();
            _sceneChanger.ChangeScene(_sceneChanger.SceneNumbData.Game);
        }
    }
    
    private void HandleConnectAsPlayer(string code)
    {
        if (!string.IsNullOrWhiteSpace(_gameSettingsModel.Nickname))
        {
            _gameSettingsModel.GameMode = GameMode.Client;
            _gameSettingsModel.SessionCode = code;
            _sceneChanger.ChangeScene(_sceneChanger.SceneNumbData.Game);
        }
    }

    private string GenerateRandomCode()
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder stringBuilder = new StringBuilder(_gameSettingsData.CountCodeCharacters);
    
        for (int i = 0; i < _gameSettingsData.CountCodeCharacters; i++)
        {
            stringBuilder.Append(chars[UnityEngine.Random.Range(0, chars.Length)]);
        }
    
        return stringBuilder.ToString();
    }
}