namespace Juspay
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface that identifies all entities returned by Juspay.
    /// </summary>
    public interface IJuspayEntity
    {
        Dictionary<string, object> Data { get; set; }
    }
}
