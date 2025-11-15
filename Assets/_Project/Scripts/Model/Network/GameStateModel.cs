using System.Linq;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Model
{
    public class GameStateModel : NetworkBehaviour
    {
        [Networked, Capacity(12)]
        public NetworkDictionary<PlayerRef, NetworkObject> SpawnedCharacters => default;
        
        [Networked]
        public NetworkBool IsGameActive { get; private set; }
        
        public override void FixedUpdateNetwork()
        {               
            var firstElement = SpawnedCharacters.First();
            Transform tr = firstElement.Value.transform;
            Debug.Log(tr.position);

        }

        public void EndGame()
        {
            IsGameActive = false;
        }

        public void StartGame()
        {
            IsGameActive = true;
        }
    }
}