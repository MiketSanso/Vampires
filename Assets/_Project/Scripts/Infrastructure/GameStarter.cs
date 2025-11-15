using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class GameStarter : MonoBehaviour
    {
        private PrefabsData _prefabsData;
        private DiContainer _diContainer;
        
        [Inject]
        private void Construct(DiContainer diContainer,
            PrefabsData prefabsData)
        {
            _prefabsData = prefabsData;
            _diContainer = diContainer;
        }

        private void Awake()
        {
            _diContainer.InstantiatePrefab(_prefabsData.BasicSpawner);
        }
    }
}