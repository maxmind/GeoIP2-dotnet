namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    /// Contains data related to your MaxMind account.
    /// 
    /// This record is returned by all the end points.
    /// </summary>
    public class MaxMind
    {
        /// <summary>
        /// The number of remaining queried in your account for the current
        /// end point.
        /// </summary>
        public int QueriesRemaining { get; internal set; }
    }
}