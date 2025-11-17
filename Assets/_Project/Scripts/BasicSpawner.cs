using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.Model;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class BasicSpawner : NetworkObject, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;
    private GameSettingsModel _gameSettingsModel;
    private DiContainer _diContainer;
    private PrefabsData _prefabsData;
    private GameStateModel _gameStateModel;
    private SceneNumbData _sceneNumbData;
    
    [Inject]
    private void Construct(GameSettingsModel gameSettingsModel,
        DiContainer diContainer,
        PrefabsData prefabsData,
        SceneNumbData sceneNumbData)
    {
        _gameSettingsModel = gameSettingsModel;
        _diContainer = diContainer;
        _prefabsData = prefabsData;
        _sceneNumbData = sceneNumbData;
    }

    private async void Start()
    {
        var scene = SceneRef.FromIndex(_sceneNumbData.Game);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid) {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = _gameSettingsModel.GameMode,
            SessionName = _gameSettingsModel.SessionName,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef playerRef)
    {
        if (runner.IsServer)
        {
            NetworkObject networkStateModel  = InstantiateNetworkObject(_prefabsData.GameStateModel, Vector3.zero);
            NetworkObject networkMessageModel = InstantiateNetworkObject(_prefabsData.MessageModel, Vector3.zero);

            if (networkStateModel.TryGetComponent(out GameStateModel stateModel) &&
                networkMessageModel.TryGetComponent(out MessagesModel messagesModel))
            {
                if (messagesModel is IInitializable initializable)
                    initializable.Initialize();
            
            
                _gameStateModel = stateModel;
            
                _diContainer.Bind<GameStateModel>()
                    .FromInstance(_gameStateModel)
                    .AsSingle();
            }
            else 
                Debug.Log("Spawned incorrect object!");
            
            _gameStateModel.RPC_StartGame();
            
            
            Vector3 spawnPosition = new Vector3(0, 4, 0);
            
            NetworkObject player = InstantiateNetworkObject(_prefabsData.Player, spawnPosition, playerRef);
            NetworkObject enemy = InstantiateNetworkObject(_prefabsData.Enemy, spawnPosition + new Vector3(1, 0, 3));
            
            player.AssignInputAuthority(playerRef);

            _gameStateModel.SpawnedCharacters.Set(playerRef, player);
        }
    }
    
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        InjectDependencies(obj);
    }
        
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_gameStateModel.SpawnedCharacters.TryGet(player, out NetworkObject playerObject))
        {
            runner.Despawn(playerObject);

            if (runner.IsServer)
            {
                _gameStateModel.SpawnedCharacters.Remove(player);
            }
        }
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
    
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
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
    
    private void InjectDependencies(NetworkObject networkObject)
    {
        _diContainer.Inject(networkObject);
        
        var injectables = networkObject.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (var injectable in injectables)
        {
            _diContainer.Inject(injectable);
        }
    }

    private NetworkObject InstantiateNetworkObject(NetworkBehaviour networkBehaviour, Vector3 spawnPosition, PlayerRef playerRef)
    {
        if (networkBehaviour.TryGetComponent(out NetworkObject networkObject))
        {
            NetworkObject newObject = _runner.Spawn(networkObject, spawnPosition, Quaternion.identity, playerRef);
            InjectDependencies(newObject);
            return newObject;
        }
        
        Debug.LogError("Error, network object not found!");
        return null;
    }
    
    private NetworkObject InstantiateNetworkObject(NetworkBehaviour networkBehaviour, Vector3 spawnPosition)
    {
        if (networkBehaviour.TryGetComponent(out NetworkObject networkObject))
        {
            NetworkObject newObject = _runner.Spawn(networkObject, spawnPosition, Quaternion.identity);
            InjectDependencies(newObject);
            return newObject;
        }
        
        Debug.LogError("Error, network object not found!");
        return null;
    }
}