using _Project.Scripts.Model;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Enemy
{
    public class Enemy : NetworkBehaviour
    {
        [SerializeField] private float _speed;
        
        private TransformsModel _transformModel;
        private GameStateModel _gameStateModel;

        public void Initialize(TransformsModel transformsModel,
            GameStateModel gameStateModel)
        {
            _transformModel = transformsModel;
            _gameStateModel = gameStateModel;
        }

        public override void FixedUpdateNetwork()
        {
            if (_gameStateModel.IsGameActive)
            {
                Transform closestTransform = _transformModel.Targets[0];
                
                foreach (Transform playerTransform in _transformModel.Targets)
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