using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TextsData", menuName = "TextsData", order = 0)]
    public class TextsData : ScriptableObject
    {
        [field: SerializeField] private string UserTextBeforeMessage { get; set; }
    }
}