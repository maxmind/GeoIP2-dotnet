using Newtonsoft.Json;

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
        /// Constructor
        /// </summary>
        public MaxMind() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public MaxMind(int? queriesRemaining = null)
        {
            QueriesRemaining = queriesRemaining;
        }

        /// <summary>
        /// The number of remaining queried in your account for the current
        /// end point.
        /// </summary>
        [JsonProperty("queries_remaining")]
        public int? QueriesRemaining { get; internal set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("MaxMind [{0}]", QueriesRemaining.HasValue ? QueriesRemaining.ToString() : string.Empty);
        }
    }
}