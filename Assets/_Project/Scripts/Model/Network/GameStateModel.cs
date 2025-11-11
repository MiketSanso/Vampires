using Fusion;
using UnityEngine;

namespace _Project.Scripts.Model
{
    public class GameStateModel : NetworkBehaviour
    {
        [Networked, Capacity(12)]
        public NetworkDictionary<PlayerRef, Player> SpawnedCharacters => default;
        
        [Networked]
        public NetworkBool IsGameActive { get; private set; }

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