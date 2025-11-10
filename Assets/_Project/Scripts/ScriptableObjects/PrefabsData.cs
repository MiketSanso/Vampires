using Fusion;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PrefabsData", menuName = "PrefabsData", order = 0)]
    public class PrefabsData : ScriptableObject
    {
        [field: SerializeField] public NetworkObject Player { get; private set; }
        [field: SerializeField] public NetworkObject MessageModel { get; private set; }
        [field: SerializeField] public NetworkObject BasicSpawner { get; private set; }
        [field: SerializeField] public NetworkObject Enemy { get; private set; }
        [field: SerializeField] public NetworkObject GameStateModel { get; private set; }
    }
}