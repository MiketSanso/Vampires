using R3;

namespace _Project.Scripts.ViewModel
{
    public interface IPlayerViewModel
    {
        public ReadOnlyReactiveProperty<float> Health { get; }
        public ReadOnlyReactiveProperty<float> Damage { get; }
        public ReadOnlyReactiveProperty<float> Speed { get; }
        public ReadOnlyReactiveProperty<float> Experience { get; }

        public ReactiveCommand<Unit> AddHealth { get; }
        public ReactiveCommand<Unit> AddDamage { get; }
        public ReactiveCommand<Unit> AddSpeed { get; }
        public ReactiveCommand<float> AddExperience { get; }
    }
}