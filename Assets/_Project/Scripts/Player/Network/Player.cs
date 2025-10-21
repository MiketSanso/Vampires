using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [HideInInspector, Networked] public NetworkString<_16> Nickname { get; set; }

    [SerializeField] private float _speed;
    [SerializeField] private NetworkCharacterController _characterController;
    [SerializeField] private Camera _gameCamera;
    [SerializeField] private AudioListener _audioListener;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _characterController.Move(_speed * data.direction * Runner.DeltaTime);
        }
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Camera oldMainCamera = Camera.main;
            if (oldMainCamera != null && oldMainCamera != _gameCamera)
            {
                oldMainCamera.gameObject.SetActive(false);
                oldMainCamera.tag = "Untagged";
            }

            _gameCamera.tag = "MainCamera";
            _gameCamera.enabled = true;
            _audioListener.enabled = true;
        }
        
        if (Object.HasInputAuthority)
        {
            RPC_SetNickname("Ваш никнейм"); //TODO: Тута какаято-то хуйня
        }
    }
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SetNickname(string newName)
    {
        Nickname = newName;
    }
}
