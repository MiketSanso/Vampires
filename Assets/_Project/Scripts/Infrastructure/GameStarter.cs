using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class GameStarter : MonoBehaviour
    {
        private PrefabsData _prefabsData;
        private IInstantiator _instantiator;
        
        [Inject]
        private void Construct(IInstantiator instantiator,
            PrefabsData prefabsData)
        {
            _prefabsData = prefabsData;
            _instantiator = instantiator;
        }

        private void Awake()
        {
            _instantiator.InstantiatePrefab(_prefabsData.BasicSpawner);
        }
    }
}