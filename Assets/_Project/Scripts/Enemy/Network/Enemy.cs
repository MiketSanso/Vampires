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
        [SerializeField] private EnemyModel _enemyModel;
        [SerializeField] private NetworkCharacterController _characterController;
        
        private GameStateModel _gameStateModel;
        private EnemyData _enemyData;
        private AttackAreasData _attackAreasData;
        
        [Inject]
        private void Construct(GameStateModel gameStateModel,
            AttackAreasData attackAreasData,
            EnemyData enemyData)
        {
            _gameStateModel = gameStateModel;
            _attackAreasData = attackAreasData;
            _enemyData = enemyData;
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
                IDamageable damageableEntity;
                
                if (_gameStateModel.SpawnedCharacters.Count == 0) return;
                
                NetworkObject firstElement = _gameStateModel.SpawnedCharacters.First().Value;
                
                if (firstElement.TryGetComponent<IDamageable>(out IDamageable damageableElement))
                    damageableEntity = damageableElement;
                else
                {
                    Debug.LogError("Player has no damageable component!");
                    return;
                }
                
                closestTransform = firstElement.GetComponentInChildren<NetworkCharacterController>().transform;
                
                foreach (var element in _gameStateModel.SpawnedCharacters)
                {
                    NetworkCharacterController foundControllerIn = element.Value.GetComponentInChildren<NetworkCharacterController>();

                    if (Vector3.Distance(_characterController.transform.position, closestTransform.transform.position) >
                        Vector3.Distance(_characterController.transform.position, foundControllerIn.transform.position))
                    {
                        closestTransform = foundControllerIn.transform; 
                         if (element.Value.TryGetComponent<IDamageable>(out IDamageable damageable))
                             damageableEntity = damageable;
                         else
                             Debug.LogError("Player has no damageable component!");
                    } 
                }
                
                Vector3 targetPosition = closestTransform.transform.position;
                
                if (damageableEntity != null && Vector3.Distance(targetPosition, _characterController.transform.position) <= _attackAreasData.EnemyArea)
                    damageableEntity.TakeDamage(_enemyData.Damage);
                
                Vector3 moveDirection = (targetPosition - _characterController.transform.position).normalized;
                moveDirection.y = 0;
                _characterController.Move(moveDirection * Runner.DeltaTime);
            }
        }
    }
}