using _Project.Scripts.Services;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class Bootstrap : MonoBehaviour
    {
        private SceneChanger _sceneChanger;

        [Inject]
        private void Construct(SceneChanger sceneChanger)
        {
            _sceneChanger = sceneChanger;
        }
        
        private void Start()
        {
            _sceneChanger.ChangeScene(_sceneChanger.SceneNumbDat.Menu);
        }
    }
}