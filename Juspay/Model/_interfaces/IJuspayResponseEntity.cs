namespace Juspay
{
    using System.Collections.Generic;

    public interface IJuspayResponseEntity
    {
        Dictionary<string, object> Response { get; set; }
    }
}
