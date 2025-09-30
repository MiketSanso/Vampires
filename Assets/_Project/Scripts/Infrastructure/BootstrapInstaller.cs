using _Project.Scripts.Model;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Services;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private SceneNumbData _sceneNumbData;
        
        public override void InstallBindings()
        {
            Container.Bind<SceneChanger>().AsSingle().WithArguments(_sceneNumbData);
            Container.Bind<GameModeModel>().AsSingle();
        }
    }
}