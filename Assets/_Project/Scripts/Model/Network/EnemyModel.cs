using _Project.Scripts.ScriptableObjects;
using Fusion;
using R3;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Model
{
    public class EnemyModel : NetworkBehaviour
    {
        public ReactiveCommand<float> ReactiveTakeDamage = new();
        public ReadOnlyReactiveProperty<float> MaxHealth => _maxHealthRp;
        public ReadOnlyReactiveProperty<float> Health => _healthRp;
        public ReadOnlyReactiveProperty<float> Damage => _damageRp;
        public ReadOnlyReactiveProperty<float> Speed => _speedRp;
        
        private EnemyData _enemyData;
        
        private readonly ReactiveProperty<float> _maxHealthRp = new();
        private readonly ReactiveProperty<float> _healthRp = new();
        private readonly ReactiveProperty<float> _damageRp = new();
        private readonly ReactiveProperty<float> _speedRp = new();
        
        [Networked] 
        private float _maxHealth { get; set; }
        [Networked] 
        private float _health { get; set; }
        [Networked] 
        private float _damage { get; set; }
        [Networked] 
        private float _speed { get; set; }
        
        [Inject]
        private void Construct(EnemyData enemyData)
        {
            _enemyData = enemyData;
        }

        private void Start()
        {
            if (Runner.IsServer)
            {
                _health = _enemyData.Health;
                _damage = _enemyData.Damage;
                _speed = _enemyData.Speed;
                _maxHealth = _enemyData.MaxHealth;
    
                ReactiveTakeDamage.Subscribe(damage =>
                {
                    if (Runner.IsServer)
                    {
                        _health = Mathf.Clamp(_health - damage, 0, _maxHealth);
                        _healthRp.Value = _health;
                    }
                });
            }
        }
    }
}