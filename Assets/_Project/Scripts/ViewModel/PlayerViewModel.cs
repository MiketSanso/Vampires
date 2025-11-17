using _Project.Scripts.Model;
using R3;
using Zenject;

namespace _Project.Scripts.ViewModel
{
    public class PlayerViewModel : IPlayerViewModel, IInitializable
    {
        public ReadOnlyReactiveProperty<float> Health { get; private set; }
        public ReadOnlyReactiveProperty<float> Damage { get; private set; }
        public ReadOnlyReactiveProperty<float> Speed  { get; private set; }
        public ReadOnlyReactiveProperty<float> Experience { get; private set; }

        public ReactiveCommand<Unit> AddHealth { get; private set; }
        public ReactiveCommand<Unit> AddDamage { get; private set; }
        public ReactiveCommand<Unit> AddSpeed  { get; private set; }
        public ReactiveCommand<float> AddExperience { get; private set; }
        
        private PlayerModel _playerModel;
        
        public void InitializePlayerModel(PlayerModel playerModel)
        {
            _playerModel = playerModel;
            
            AddHealth = _playerModel.ReactiveAddHealth;
            AddDamage = _playerModel.ReactiveAddDamage;
            AddSpeed = _playerModel.ReactiveAddSpeed;
            AddExperience = _playerModel.ReactiveAddExperience;
            Health = _playerModel.Health;
            Damage = _playerModel.Damage;
            Speed = _playerModel.Speed;
            Experience = _playerModel.Experience;
        }
        
        public void Initialize()
        {
            
        }

        private void ChangeHealth(float health)
        {
            _sliderHealth.value = health;
        }
    
        private void ChangeExperience(float exp)
        {
            _sliderExperience.value = exp;
        }
    }
}