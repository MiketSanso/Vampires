using _Project.Scripts.ViewModel;
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

        private void Start()
        {
            _playerViewModel.ExperienceState.Subscribe(a => ChangeExperience());
        }
        
        private void ChangeHealth()
        {
            _sliderHealth.value = _playerViewModel.Health.CurrentValue;
        }
    
        private void ChangeExperience()
        {
            _sliderExperience.value = _playerViewModel.ExperienceState.CurrentValue;
            _textExperience.text = _playerViewModel.ExperienceLevel.ToString();
        }
    }
}