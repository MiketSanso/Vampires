using _Project.Scripts.Enemys;
using _Project.Scripts.Model;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PrefabsData", menuName = "PrefabsData", order = 0)]
    public class PrefabsData : ScriptableObject
    {
        [field: SerializeField] public Player Player { get; private set; }
        [field: SerializeField] public MessagesModel MessageModel { get; private set; }
        [field: SerializeField] public BasicSpawner BasicSpawner { get; private set; }
        [field: SerializeField] public Enemy Enemy { get; private set; }
        [field: SerializeField] public GameStateModel GameStateModel { get; private set; }
    }
}