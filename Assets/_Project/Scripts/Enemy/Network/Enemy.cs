using System.Linq;
using _Project.Scripts.Model;
using _Project.Scripts.ScriptableObjects;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Enemys
{
    public class Enemy : NetworkBehaviour, IDamageable
    {
        [SerializeField] private EnemyModel _enemyModel;
        [SerializeField] private NetworkCharacterController _characterController;
        
        private GameStateModel _gameStateModel;
        private EnemyData _enemyData;
        private AttackAreasData _attackAreasData;
        private float _timeRecharge;
        
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
                Transform nearestTransform;
                IDamageable damageableEntity;
                
                if (_gameStateModel.SpawnedCharacters.Count == 0) return;
                
                _timeRecharge += Time.deltaTime;
                
                NetworkObject firstElement = _gameStateModel.SpawnedCharacters.First().Value;
                
                if (firstElement.TryGetComponent<IDamageable>(out IDamageable damageableElement))
                    damageableEntity = damageableElement;
                else
                {
                    Debug.LogError("Player has no damageable component!");
                    return;
                }
                
                nearestTransform = firstElement.GetComponentInChildren<NetworkCharacterController>().transform;

                SearchNearestPlayer(ref nearestTransform, ref damageableEntity);
                
                Vector3 targetPosition = nearestTransform.transform.position;

                if (_timeRecharge >= _enemyData.TimeRecharge)
                    TryAttack(damageableEntity, targetPosition);
                
                Vector3 moveDirection = (targetPosition - _characterController.transform.position).normalized;
                moveDirection.y = 0;
                _characterController.Move(moveDirection * Runner.DeltaTime);
            }
        }

        private void TryAttack(IDamageable damageableEntity, Vector3 targetPosition)
        {
            if (damageableEntity != null && Vector3.Distance(targetPosition, _characterController.transform.position) <=
                _attackAreasData.EnemyArea)
            {
                damageableEntity.TakeDamage(_enemyData.Damage);
                _timeRecharge = 0;
            }
        }

        private void SearchNearestPlayer(ref Transform closestTransform, ref IDamageable damageableEntity)
        {
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
        }

        public void TakeDamage(float damage)
        {
            throw new System.NotImplementedException();
        }
    }
}