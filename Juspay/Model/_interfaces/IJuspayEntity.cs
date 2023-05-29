namespace Juspay
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface that identifies all entities returned by Juspay.
    /// </summary>
    public interface IJuspayEntity
    {
        JuspayResponse? JuspayResponse { get; set; }
        Dictionary<string, object> DictionaryObject { get; set; }
    }
}
