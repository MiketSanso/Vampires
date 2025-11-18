using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameSettingsData", menuName = "GameSettingsData", order = 0)]
    public class GameSettingsData : ScriptableObject
    {
        [field: SerializeField] public int CountCodeCharacters { get; private set; }
        [field: SerializeField] public string CharactersForCode { get; private set; }
    }
}