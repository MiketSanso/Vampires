using _Project.Scripts.Model;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Services;
using UnityEditor.U2D.Animation;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private SceneNumbData _sceneNumbData;
        [SerializeField] private PrefabsData _prefabsData;
        [SerializeField] private AttackAreasData _attackAreasData;
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private GameSettingsData _gameSettingsData;
        
        public override void InstallBindings()
        {
            Container.Bind<EnemyData>().FromInstance(_enemyData).AsSingle();
            Container.Bind<PlayerData>().FromInstance(_playerData).AsSingle();
            Container.Bind<GameSettingsData>().FromInstance(_gameSettingsData).AsSingle();
            Container.Bind<SceneNumbData>().FromInstance(_sceneNumbData).AsSingle();
            Container.Bind<SceneChanger>().AsSingle();
            Container.Bind<GameSettingsModel>().AsSingle();
            Container.Bind<PrefabsData>().FromInstance(_prefabsData).AsSingle();
        }
    }
}