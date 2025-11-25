using _Project.Scripts.ScriptableObjects;
using Fusion;
using R3;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Model
{
    public class PlayerModel : NetworkBehaviour
    {
        public ReactiveCommand<Unit> ReactiveAddMaxHealth = new();
        public ReactiveCommand<Unit> ReactiveAddDamage = new();
        public ReactiveCommand<Unit> ReactiveAddSpeed = new();
        public ReactiveCommand<float> ReactiveAddExperience = new();
        public ReactiveCommand<float> ReactiveTakeDamage = new();
        
        public ReadOnlyReactiveProperty<float> MaxHealth => _maxHealth;
        public ReadOnlyReactiveProperty<float> Health => _health;
        public ReadOnlyReactiveProperty<float> Damage => _damage;
        public ReadOnlyReactiveProperty<float> Speed => _speed;
        public ReadOnlyReactiveProperty<float> Experience => _experience;
        public ReadOnlyReactiveProperty<float> ExperienceLevel => _experienceLevel;
        
        private PlayerData _playerData;
        
        private readonly ReactiveProperty<float> _experience = new();
        private readonly ReactiveProperty<float> _maxHealth = new();
        private readonly ReactiveProperty<float> _health = new();
        private readonly ReactiveProperty<float> _damage = new();
        private readonly ReactiveProperty<float> _speed = new();
        private readonly ReactiveProperty<float> _experienceLevel = new();
        
        [Inject]
        private void Construct(PlayerData playerData)
        {
            _playerData = playerData;
        }

        private void Start()
        {
            _health.Value = _playerData.StartHealth;
            _damage.Value = _playerData.StartDamage;
            _speed.Value = _playerData.StartSpeed;
            _maxHealth.Value = _playerData.StartHealth;
            _experience.Value = 0;
            _experienceLevel.Value = 0;
            
            ReactiveAddMaxHealth.Subscribe(_ => _maxHealth.Value += _playerData.StepAddHealth);
            ReactiveAddDamage.Subscribe(_ => _damage.Value += _playerData.StepAddHealth);
            ReactiveAddSpeed.Subscribe(_ => _speed.Value += _playerData.StepAddHealth);
            ReactiveAddExperience.Subscribe(exp =>
            {
                _experience.Value += exp;
            });

            ReactiveTakeDamage.Subscribe(damage =>
            {
                Debug.Log(_health.Value + " " + damage);

                _health.Value = Mathf.Clamp(_health.Value - damage, 0, _maxHealth.Value);
                
                Debug.Log(_health.Value + " " + damage);

            });
        }
    }
}