namespace Juspay
{
    public interface ISign
    {
        string Sign(string key, string data);
        string VerifySign(string key, string data);
    }
}