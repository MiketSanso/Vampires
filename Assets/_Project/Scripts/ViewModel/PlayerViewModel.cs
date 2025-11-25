using _Project.Scripts.Model;
using R3;
using Zenject;

namespace _Project.Scripts.ViewModel
{
    public class PlayerViewModel : IPlayerViewModel, IInitializable
    {
        private PlayerModel _playerModel;

        public ReadOnlyReactiveProperty<float> Health { get; private set; }
        public ReadOnlyReactiveProperty<float> Damage { get; private set; }
        public ReadOnlyReactiveProperty<float> Speed  { get; private set; }
        public ReadOnlyReactiveProperty<float> ExperienceState { get; private set; }
        public ReadOnlyReactiveProperty<float> ExperienceLevel { get; private set; }
        
        public ReactiveCommand<Unit> AddHealth { get; private set; }
        public ReactiveCommand<Unit> AddDamage { get; private set; }
        public ReactiveCommand<Unit> AddSpeed  { get; private set; }
        public ReactiveCommand<float> AddExperience { get; private set; }
        
        public void InitializePlayerModel(PlayerModel playerModel)
        {
            _playerModel = playerModel;
            
            AddHealth = _playerModel.ReactiveAddMaxHealth;
            AddDamage = _playerModel.ReactiveAddDamage;
            AddSpeed = _playerModel.ReactiveAddSpeed;
            AddExperience = _playerModel.ReactiveAddExperience;
            Health = _playerModel.Health;
            Damage = _playerModel.Damage;
            Speed = _playerModel.Speed;
            ExperienceState = _playerModel.Experience;
            ExperienceLevel = _playerModel.ExperienceLevel;
        }
        
        public void Initialize()
        {
            
        }
    }
}