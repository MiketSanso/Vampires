using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AttackAreasData", menuName = "AttackAreasData", order = 0)]
    public class AttackAreasData : ScriptableObject
    {
        [field: SerializeField] public float PlayerArea { get; private set; }
        [field: SerializeField] public float EnemyArea { get; private set; }
    }
}