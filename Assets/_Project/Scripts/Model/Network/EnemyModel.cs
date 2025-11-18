using _Project.Scripts.ScriptableObjects;
using Fusion;
using R3;
using Zenject;

namespace _Project.Scripts.Model
{
    public class EnemyModel : NetworkBehaviour
    {
        public ReadOnlyReactiveProperty<float> Health => _health;
        public ReadOnlyReactiveProperty<float> Damage => _damage;
        public ReadOnlyReactiveProperty<float> Speed => _speed;
        public ReadOnlyReactiveProperty<float> Experience => _experience;
        
        private EnemyData _enemyData;
        
        private readonly ReactiveProperty<float> _experience = new();
        private readonly ReactiveProperty<float> _health = new();
        private readonly ReactiveProperty<float> _damage = new();
        private readonly ReactiveProperty<float> _speed = new();
        
        [Inject]
        private void Construct(EnemyData enemyData)
        {
            _enemyData = enemyData;
        }

        public override void Spawned()
        {
            _health.Value = _enemyData.Health;
            _damage.Value = _enemyData.Damage;
            _speed.Value = _enemyData.Speed;
            _experience.Value = 0;
        }
    }
}