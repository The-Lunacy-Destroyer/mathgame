namespace Movement
{
    public interface IEntityRadiusStoppable
    {
        // distance between enemy and target at which enemy stops
        public float StopRadius { get; set; }
    }
}