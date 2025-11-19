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
        
        private void InitializePlayerViewModel(IPlayerViewModel playerViewModel)
        {
            _playerViewModel = playerViewModel;
            
            _playerViewModel.Health.Subscribe(healthState =>
            {
                ChangeHealthSlider(healthState);
            });
            
            _playerViewModel.ExperienceState.Subscribe(experienceState =>
            {
                ChangeExperienceSlider(experienceState);
            });
            
            _playerViewModel.ExperienceLevel.Subscribe(experienceLevel =>
            {
                ChangeExperienceText(experienceLevel);
            });
        }
        
        private void ChangeHealthSlider(float healthState)
        {
            _sliderHealth.value = healthState;
        }
    
        private void ChangeExperienceSlider(float exeprienceState)
        {
            _sliderExperience.value = exeprienceState;
        }
        
        private void ChangeExperienceText(float exeprienceLevel)
        {
            _textExperience.text = exeprienceLevel.ToString();
        }
    }
}