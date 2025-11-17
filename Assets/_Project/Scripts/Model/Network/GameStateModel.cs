using Fusion;

namespace _Project.Scripts.Model
{
    public class GameStateModel : NetworkBehaviour
    {
        [Networked, Capacity(12)]
        public NetworkDictionary<PlayerRef, NetworkObject> SpawnedCharacters  => default;
        
        [Networked]
        public NetworkBool IsGameActive { get; private set; }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_EndGame()
        {
            IsGameActive = false;
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_StartGame()
        {
            IsGameActive = true;
        }
    }
}