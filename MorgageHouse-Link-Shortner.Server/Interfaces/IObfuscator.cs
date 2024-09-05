namespace MorgageHouse_Link_Shortner.Server.Interfaces
{
    public interface IObfuscator<T> where T: struct
    {
        string Encode(T value);
        bool TryDecode(string enCoded, out T decoded);
    }
}
