using _Project.Scripts.ViewModel;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.View
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Slider _sliderHealth;
        [SerializeField] private Slider _sliderExperience;
        [SerializeField] private TMP_Text _textExperience;
        
        private IPlayerViewModel _playerViewModel;
        
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        public void InitializePlayerViewModel(IPlayerViewModel playerViewModel)
        {
            _playerViewModel = playerViewModel;
            
            _playerViewModel.Health.Subscribe(healthState =>
            {
                _sliderHealth.value = healthState/100;
            });
            
            _playerViewModel.ExperienceState.Subscribe(experienceState =>
            {
                _sliderExperience.value = experienceState/100;
            });
            
            _playerViewModel.ExperienceLevel.Subscribe(experienceLevel =>
            {
                _textExperience.text = experienceLevel.ToString();
            });
        }
    }
}