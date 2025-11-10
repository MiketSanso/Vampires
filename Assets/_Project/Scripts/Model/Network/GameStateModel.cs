using Fusion;

namespace _Project.Scripts.Model
{
    public class GameStateModel : NetworkBehaviour
    {
        [Networked]
        public NetworkBool IsGameActive { get; private set;  } = default;
        
        [Networked, Capacity(12)]
        public NetworkDictionary<PlayerRef, Player> SpawnedCharacters { get; } = default;

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