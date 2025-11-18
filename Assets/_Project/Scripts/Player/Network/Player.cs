using _Project.Scripts.Model;
using Fusion;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Player : NetworkBehaviour
{
    [SerializeField] private NetworkCharacterController _characterController;
    [SerializeField] private Camera _gameCamera;
    [SerializeField] private AudioListener _audioListener;
    [SerializeField] private Slider _sliderHealth;
    [SerializeField] private Slider _sliderExperience;
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
}
