using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Model
{
    public class TransformsModel
    {
        public List<Transform> Targets { get; private set; } = new List<Transform>();

        public void AddTarget(Transform target)
        {
            if (target != null)
                Targets.Add(target);
        }
        
        public void RemoveTarget(Transform target)
        {
            if (target != null && Targets.Contains(target))
                Targets.Remove(target);
        }

        public void RemoveAllTargets()
        {
            Targets.Clear();
        }
    }
}