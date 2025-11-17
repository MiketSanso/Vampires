using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [field: SerializeField] public float StartHealth { get; private set; }
        [field: SerializeField] public float StartDamage { get; private set; }
        [field: SerializeField] public float StartSpeed { get; private set; }
        
        [field: SerializeField] public float StepAddSpeed { get; private set; }
        [field: SerializeField] public float StepAddDamage { get; private set; }
        [field: SerializeField] public float StepAddHealth { get; private set; }
    }
}