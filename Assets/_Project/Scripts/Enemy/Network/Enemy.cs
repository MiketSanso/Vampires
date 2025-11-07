using System.Collections.Generic;
using _Project.Scripts.Model;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Enemy
{
    public class Enemy : NetworkBehaviour
    {
        [SerializeField] private float _speed;
        
        private TransformsModel _playerModel;
        private GameStateModel _gameStateModel;

        private void Start()
        {
            var diContainer = FindObjectOfType<SceneContext>()?.Container;
            if (diContainer != null)
            {
                _playerModel = diContainer.Resolve<TransformsModel>();
                _gameStateModel = diContainer.Resolve<GameStateModel>();
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (_gameStateModel.IsGameActive)
            {
                Transform closestTransform = _playerModel.Targets[0];
                
                foreach (Transform playerTransform in _playerModel.Targets)
                {
                    if (Vector3.Distance(transform.position, closestTransform.position) >
                        Vector3.Distance(transform.position, playerTransform.position))
                        closestTransform = playerTransform;
                }
                
                Vector3 targetPosition = closestTransform.position;
                
                transform.position = Vector3.MoveTowards(
                    transform.position, 
                    targetPosition, 
                    _speed * Runner.DeltaTime
                );
                
                transform.LookAt(targetPosition);
            }
        }
    }
}