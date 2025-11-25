using _Project.Scripts;
using _Project.Scripts.Model;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour, IDamageable
{
    [SerializeField] private NetworkCharacterController _characterController;
    [SerializeField] private Camera _gameCamera;
    [SerializeField] private AudioListener _audioListener;
    [SerializeField] private PlayerModel _playerModel;
    
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _characterController.Move(_playerModel.Speed.CurrentValue * data.direction * Runner.DeltaTime);
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
            _gameCamera.gameObject.SetActive(true);
        }
    }

    public void TakeDamage(float damage)
    {
        _playerModel.ReactiveTakeDamage.Execute(damage);
    }
}
