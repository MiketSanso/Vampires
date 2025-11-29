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
        
        public ReadOnlyReactiveProperty<float> MaxHealth => _maxHealthRp;
        public ReadOnlyReactiveProperty<float> Health => _healthRp;
        public ReadOnlyReactiveProperty<float> Damage => _damageRp;
        public ReadOnlyReactiveProperty<float> Speed => _speedRp;
        public ReadOnlyReactiveProperty<float> Experience => _experienceRp;
        public ReadOnlyReactiveProperty<int> ExperienceLevel => _experienceLevelRp;
        
        private PlayerData _playerData;
        
        private readonly ReactiveProperty<float> _experienceRp = new();
        private readonly ReactiveProperty<float> _maxHealthRp = new();
        private readonly ReactiveProperty<float> _healthRp = new();
        private readonly ReactiveProperty<float> _damageRp = new();
        private readonly ReactiveProperty<float> _speedRp = new();
        private readonly ReactiveProperty<int> _experienceLevelRp = new();
        
        [Networked] 
        private float _maxHealth { get; set; }
        [Networked] 
        private float _health { get; set; }
        [Networked] 
        private float _damage { get; set; }
        [Networked] 
        private float _speed { get; set; }
        [Networked] 
        private float _experience { get; set; }
        [Networked] 
        private int _experienceLevel { get; set; }
        
        [Inject]
        private void Construct(PlayerData playerData)
        {
            _playerData = playerData;
        }

        private void Start()
        {
            _health = _playerData.StartHealth;
            _damage = _playerData.StartDamage;
            _speed = _playerData.StartSpeed;
            _maxHealth = _playerData.StartHealth;
            _experience = 0;
            _experienceLevel = 0;
            
            ReactiveAddMaxHealth.Subscribe(_ =>
            {
                ChangeValue(ref _maxHealth, _playerData.StepAddHealth);
                _maxHealth += _playerData.StepAddHealth;
            });
            ReactiveAddDamage.Subscribe(_ =>
            {
                ChangeValue(ref _damage, _playerData.StepAddHealth);

                _damage += _playerData.StepAddHealth;
            });
            ReactiveAddSpeed.Subscribe(_ =>
            {
                ChangeValue(ref _speed, _playerData.StepAddHealth);

                _speed += _playerData.StepAddHealth;
            });
            ReactiveAddExperience.Subscribe(exp =>
            {
                ChangeValue(ref _experience, exp);

                _experience += exp;
            });

            ReactiveTakeDamage.Subscribe(damage =>
            {
                _health = Mathf.Clamp(_health - damage, 0, _maxHealth);
            });
        }

        private void ChangeValue(ref float value, float newValue)
        {
            if (Runner.IsServer)
            {
                value += newValue;
            }
        }
    }
}