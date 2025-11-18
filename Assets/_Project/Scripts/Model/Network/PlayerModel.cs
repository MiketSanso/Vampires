using _Project.Scripts.ScriptableObjects;
using Fusion;
using R3;
using Zenject;

namespace _Project.Scripts.Model
{
    public class PlayerModel : NetworkBehaviour
    {
        public ReactiveCommand<Unit> ReactiveAddHealth;
        public ReactiveCommand<Unit> ReactiveAddDamage;
        public ReactiveCommand<Unit> ReactiveAddSpeed;
        public ReactiveCommand<float> ReactiveAddExperience;
        
        public ReadOnlyReactiveProperty<float> Health => _health;
        public ReadOnlyReactiveProperty<float> Damage => _damage;
        public ReadOnlyReactiveProperty<float> Speed => _speed;
        public ReadOnlyReactiveProperty<float> Experience => _experience;
        public ReadOnlyReactiveProperty<float> ExperienceLevel => _experienceLevel;

        
        private PlayerData _playerData;
        
        private readonly ReactiveProperty<float> _experience = new();
        private readonly ReactiveProperty<float> _health = new();
        private readonly ReactiveProperty<float> _damage = new();
        private readonly ReactiveProperty<float> _speed = new();
        private readonly ReactiveProperty<float> _experienceLevel = new();
        
        [Inject]
        private void Construct(PlayerData playerData)
        {
            _playerData = playerData;
        }

        public override void Spawned()
        {
            _health.Value = _playerData.StartHealth;
            _damage.Value = _playerData.StartDamage;
            _speed.Value = _playerData.StartSpeed;
            _experience.Value = 0;
            _experienceLevel.Value = 0;
            
            ReactiveAddHealth.Subscribe(_ => _health.Value += _playerData.StepAddHealth);
            ReactiveAddDamage.Subscribe(_ => _health.Value += _playerData.StepAddHealth);
            ReactiveAddSpeed.Subscribe(_ => _health.Value += _playerData.StepAddHealth);
            ReactiveAddExperience.Subscribe(exp =>
            {
                _health.Value += exp;
            });
        }
    }
}