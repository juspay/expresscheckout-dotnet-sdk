namespace Juspay
{
    using System.Collections.Generic;

    public interface IJuspayResponseEntity
    {
        Dictionary<string, object> response { get; set; }
        Dictionary<string, object> Response { get; set; }
        JuspayResponseBase ResponseBase { get; set; }
        string RawContent { get; set; }
    }
}
