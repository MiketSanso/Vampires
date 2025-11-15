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
        [SerializeField] private PrefabsData _prefabsData;
        
        public override void InstallBindings()
        {
            Container.Bind<SceneNumbData>().FromInstance(_sceneNumbData).AsSingle();
            Container.Bind<SceneChanger>().AsSingle();
            Container.Bind<GameSettingsModel>().AsSingle();
            Container.Bind<PrefabsData>().FromInstance(_prefabsData).AsSingle();
        }
    }
}