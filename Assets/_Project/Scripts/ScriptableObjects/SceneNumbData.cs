using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SceneNumbData", menuName = "SceneNumbData", order = 0)]
    public class SceneNumbData : ScriptableObject
    {
        [field: SerializeField] public int Menu { get; private set; }
        [field: SerializeField] public int Game { get; private set; }
    }
}