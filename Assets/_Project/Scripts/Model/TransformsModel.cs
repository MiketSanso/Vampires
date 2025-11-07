using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Model
{
    public class TransformsModel
    {
        public List<Transform> Targets = new List<Transform>();

        public void AddNewTargetList(List<Transform> targets)
        {
            Targets = targets;
        }
        
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