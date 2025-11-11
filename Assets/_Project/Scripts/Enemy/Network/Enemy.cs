using System.Linq;
using _Project.Scripts.Model;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Enemy
{
    public class Enemy : NetworkBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private NetworkCharacterController _characterController;
        
        private GameStateModel _gameStateModel;

        [Inject]
        private void Construct(GameStateModel gameStateModel)
        {
            _gameStateModel = gameStateModel;
        }

        public override void FixedUpdateNetwork()
        {
            if (_gameStateModel.IsGameActive)
            {
                Transform closestTransform;
                
                if (_gameStateModel.SpawnedCharacters.Count == 0) return;
                
               var firstElement = _gameStateModel.SpawnedCharacters.First();
               closestTransform = firstElement.Value.transform;
               
               foreach (var element in _gameStateModel.SpawnedCharacters)
               {
                   Player player = element.Value;
                   
                   if (Vector3.Distance( _characterController.transform.position, closestTransform.position) >
                       Vector3.Distance( _characterController.transform.position, player.transform.position))
                       closestTransform = player.transform; 
               }
                
               Vector3 targetPosition = closestTransform.position;
               
               Vector3 moveDirection = (targetPosition - _characterController.transform.position).normalized;
               moveDirection.y = 0;
               _characterController.Move(moveDirection * Runner.DeltaTime);
               
               Vector3 lookAtPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
               transform.LookAt(lookAtPosition);
            }
        }
    }
}