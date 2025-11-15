using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Model
{
    public class GameStateModel : NetworkBehaviour
    {
        public Dictionary<PlayerRef, NetworkObject> SpawnedCharacters => default;
        
        [Networked]
        public NetworkBool IsGameActive { get; private set; }
        
        public override void FixedUpdateNetwork()
        {               
            var firstElement = SpawnedCharacters.First();
            Transform tr = firstElement.Value.transform;
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void EndGame()
        {
            IsGameActive = false;
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void StartGame()
        {
            IsGameActive = true;
        }
    }
}