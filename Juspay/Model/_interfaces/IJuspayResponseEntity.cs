namespace Juspay
{
    using System.Collections.Generic;

    public interface IJuspayResponseEntity
    {
        dynamic response { get; set; }
        dynamic Response { get; set; }
        JuspayResponseBase ResponseBase { get; set; }
        string RawContent { get; set; }
    }
}
