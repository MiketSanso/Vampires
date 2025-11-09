using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using _Project.Scripts.Model;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class BasicSpawner : NetworkObject, INetworkRunnerCallbacks
{
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    private NetworkRunner _runner;
    private GameSettingsModel _gameSettingsModel;
    private TransformsModel _transformsModel;
    private DiContainer _diContainer;
    private PrefabsData _prefabsData;
    
    [Inject]
    private void Construct(GameSettingsModel gameSettingsModel,
        DiContainer diContainer,
        TransformsModel playersModel,
        PrefabsData prefabsData)
    {
        _gameSettingsModel = gameSettingsModel;
        _diContainer = diContainer;
        _transformsModel = playersModel;
        _prefabsData = prefabsData;
    }
    
    private void Start()
    {
        StartGame();
    }

    private async void StartGame()
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        var scene = SceneRef.FromIndex(1);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid) {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = _gameSettingsModel.GameMode,
            SessionName = _gameSettingsModel.SessionName,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Vector3 spawnPosition = new Vector3(0, 3, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_prefabsData.PlayerPrefab, spawnPosition, Quaternion.identity, player);
            if (_spawnedCharacters.Count == 0) 
            {
                runner.Spawn(_prefabsData.MessagePrefab, Vector3.zero, Quaternion.identity);
            }            
            networkPlayerObject.AssignInputAuthority(player);
            _transformsModel.AddTarget(networkPlayerObject.transform); 

            _spawnedCharacters.Add(player, networkPlayerObject);
            SetNewListTransforms(_transformsModel.Targets);
        
            InjectDependencies(networkPlayerObject);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void SetNewListTransforms(List<Transform> transforms)
    {
        _transformsModel.AddNewTargetList(transforms); 
    }
    
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        InjectDependencies(obj);
    }
        
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
        
        if (runner.IsServer)
            SetNewListTransforms(_transformsModel.Targets);
    }
    
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W))
            data.direction += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            data.direction += Vector3.back;

        if (Input.GetKey(KeyCode.A))
            data.direction += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            data.direction += Vector3.right;

        input.Set(data);
    }
    
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){ }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){ }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){ }
    
    private void InjectDependencies(NetworkObject playerObject)
    {
        var injectables = playerObject.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (var injectable in injectables)
        {
            _diContainer.Inject(injectable);
        }
    }
}