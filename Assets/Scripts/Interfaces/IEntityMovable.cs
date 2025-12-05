using UnityEngine;

namespace Interfaces
{
    public interface IEntityMovable
    {
        public float Slowdown { get; set; }
        public float Speed { get; set; }
        public float MaxSpeed { get; set; }
        private void Move() {}
    }
}