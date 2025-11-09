using Fusion;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PrefabsData", menuName = "PrefabsData", order = 0)]
    public class PrefabsData : ScriptableObject
    {
        [field: SerializeField] public NetworkPrefabRef PlayerPrefab { get; private set; }
        [field: SerializeField] public NetworkPrefabRef MessagePrefab { get; private set; }
        [field: SerializeField] public BasicSpawner BasicSpawner { get; private set; }
    }
}