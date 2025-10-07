using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace _Project.Scripts
{
    public class EnemyPool
    {
        public event Action OnNeedToCreateNewEmemy;
        
        private Queue<NetworkObject> _enemies = new Queue<NetworkObject>();

        public void InitializePool(NetworkObject[] enemies)
        {
            foreach (var enemy in enemies)
            {
                enemy.gameObject.SetActive(false);
                _enemies.Enqueue(enemy);
            }
        }

        public void ActivateEnemy(Vector3 position)
        {
            if (_enemies.Count == 0)
                OnNeedToCreateNewEmemy?.Invoke();
            
            NetworkObject enemy = _enemies.Dequeue();

            if (!enemy.gameObject.activeSelf)
            {
                enemy.gameObject.SetActive(true);
                enemy.gameObject.transform.position = position;
            }
        }

        public void ReturnEnemy(NetworkObject enemy)
        {
            enemy.gameObject.SetActive(false);
            _enemies.Enqueue(enemy);
        }
    }
}