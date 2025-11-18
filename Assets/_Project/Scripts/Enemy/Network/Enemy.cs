using System.Linq;
using _Project.Scripts.Model;
using _Project.Scripts.ScriptableObjects;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Enemys
{
    public class Enemy : NetworkBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private EnemyModel _enemyModel;
        
        private NetworkCharacterController _characterController;
        private GameStateModel _gameStateModel;
        private EnemyData _enemyData;
        
        [Inject]
        private void Construct(IInstantiator instantiator, GameStateModel gameStateModel)
        {
            //    _playerModel = instantiator.Instantiate<PlayerModel>();

            _gameStateModel = gameStateModel;
        }

        public override void FixedUpdateNetwork()
        {
            if (_gameStateModel == null)
            {
                Debug.LogWarning(_gameStateModel);
                return;
            }
            
            if (_gameStateModel.IsGameActive)
            {
                Transform closestTransform;
                
                if (_gameStateModel.SpawnedCharacters.Count == 0) return;
                
                NetworkObject firstElement = _gameStateModel.SpawnedCharacters.First().Value;
                NetworkCharacterController foundController = firstElement.GetComponentInChildren<NetworkCharacterController>();
                
                closestTransform = foundController.transform;
                
                foreach (var element in _gameStateModel.SpawnedCharacters)
                {
                    NetworkCharacterController foundControllerIn = firstElement.GetComponentInChildren<NetworkCharacterController>();
                    
                    if (Vector3.Distance( _characterController.transform.position, closestTransform.transform.position) >
                        Vector3.Distance( _characterController.transform.position, foundControllerIn.transform.position))
                        closestTransform = foundControllerIn.transform; 
                }
                
                Vector3 targetPosition = closestTransform.transform.position;
                
                Vector3 moveDirection = (targetPosition - _characterController.transform.position).normalized;
                moveDirection.y = 0;
                _characterController.Move(moveDirection * Runner.DeltaTime);
            }
        }
    }
}