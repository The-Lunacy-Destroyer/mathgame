namespace Movement
{
    public interface IEntityMovable
    {
        // slowdown when movement vector magnitude is zero
        public float Slowdown { get; set; }
        
        // scale of applied movement force
        public float MoveForce { get; set; }
        
        public float MaxSpeed { get; set; }
        
        private void Move() {}
    }
}