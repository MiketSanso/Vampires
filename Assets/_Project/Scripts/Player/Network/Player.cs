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
            _gameCamera.gameObject.SetActive(true);
        }
    }
}
