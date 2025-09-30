using System;
using _Project.Scripts.Model;
using _Project.Scripts.Services;
using _Project.Scripts.ViewModel;
using Fusion;
using R3;
using UnityEngine;
using Zenject;

public class ConnectViewModel : IConnectViewModel, IInitializable, IDisposable
{
    private readonly GameModeModel _gameModeModel;
    private readonly SceneChanger _sceneChanger;
    private readonly CompositeDisposable _disposables = new();
    
    public ReactiveCommand<Unit> ConnectAsHostCommand { get; } = new();
    public ReactiveCommand<Unit> ConnectAsPlayerCommand { get; } = new();
    
    public ConnectViewModel(GameModeModel gameModeModel,
        SceneChanger sceneChanger)
    {
        _gameModeModel = gameModeModel;
        _sceneChanger = sceneChanger;
    }

    public void Initialize()
    {
        ConnectAsHostCommand.Subscribe(_ => HandleConnectAsHost()).AddTo(_disposables);
        ConnectAsPlayerCommand.Subscribe(_ => HandleConnectAsPlayer()).AddTo(_disposables); 
    }
    
    public void Dispose()
    {
        _disposables.Dispose();
    }
    
    private void HandleConnectAsHost()
    {
        _gameModeModel.GameMode = GameMode.Host;
        _sceneChanger.ChangeScene(_sceneChanger.SceneNumbDat.Game);
    }
    
    private void HandleConnectAsPlayer()
    {
        _gameModeModel.GameMode = GameMode.Client;
        _sceneChanger.ChangeScene(_sceneChanger.SceneNumbDat.Game);
    }
}