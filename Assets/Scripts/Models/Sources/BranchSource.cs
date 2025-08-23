using UnityEngine;
using Base_Classes;
using Enums;
namespace Models.Sources
{
    public class BranchSource : Collectable
    {
          
        public override void OnCollect(GameObject collector)
        {
            base.OnCollect(collector);
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }
}