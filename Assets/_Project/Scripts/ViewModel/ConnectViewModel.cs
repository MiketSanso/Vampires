using System;
using _Project.Scripts.Model;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Services;
using _Project.Scripts.ViewModel;
using Fusion;
using R3;
using Zenject;

public class ConnectViewModel : IConnectViewModel, IInitializable, IDisposable
{
    private readonly GameSettingsModel _gameSettingsModel;
    private readonly SceneChanger _sceneChanger;
    private readonly CompositeDisposable _disposables = new();
    private readonly GameSettingsData _gameSettingsData;
    
    public ReactiveCommand<Unit> ConnectAsHostCommand { get; } = new();
    public ReactiveCommand<Unit> ConnectAsPlayerCommand { get; } = new();
    public ReactiveCommand<string> SetNameCommand { get; } = new();
    
    public ConnectViewModel(GameSettingsModel gameSettingsModel,
        SceneChanger sceneChanger)
    {
        _gameSettingsModel = gameSettingsModel;
        _sceneChanger = sceneChanger;
    }

    public void Initialize()
    {
        ConnectAsHostCommand.Subscribe(_ => HandleConnectAsHost()).AddTo(_disposables);
        ConnectAsPlayerCommand.Subscribe(_ => HandleConnectAsPlayer()).AddTo(_disposables); 
        SetNameCommand.Subscribe(name => HandleSetName(name)).AddTo(_disposables);
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
        if (_gameSettingsModel.Nickname != null)
        {
            _gameSettingsModel.GameMode = GameMode.Host;
            _gameSettingsModel.SessionCode = GenerateRandomCode();
            _sceneChanger.ChangeScene(_sceneChanger.SceneNumbData.Game);
        }
    }
    
    private void HandleConnectAsPlayer()
    {
        if (_gameSettingsModel.Nickname != null)
        {
            _gameSettingsModel.GameMode = GameMode.Client;
            _sceneChanger.ChangeScene(_sceneChanger.SceneNumbData.Game);
        }
    }

    private string GenerateRandomCode()
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var stringBuilder = new System.Text.StringBuilder(_gameSettingsData.CountCodeCharacters);
    
        for (int i = 0; i < _gameSettingsData.CountCodeCharacters; i++)
        {
            stringBuilder.Append(chars[UnityEngine.Random.Range(0, chars.Length)]);
        }
    
        return stringBuilder.ToString();
    }
}