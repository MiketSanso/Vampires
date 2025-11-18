using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 0)]
    public class AttackAreasData : ScriptableObject
    {
        [field: SerializeField] public float PlayerArea { get; private set; }
    }
}