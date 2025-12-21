using UnityEngine;

namespace Projectile
{
    public class LaserController : ProjectileController
    {
        protected override void OnTriggerStay2D(Collider2D other)
        {
            base.OnTriggerStay2D(other);
            if (!SourceObject)
            {
                SourceObject = GetComponentInParent<EntityController>()?.gameObject;
            }
        }
    }
}