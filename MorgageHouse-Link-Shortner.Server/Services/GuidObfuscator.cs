using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.AspNetCore.WebUtilities;
using MorgageHouse_Link_Shortner.Server.Interfaces;

namespace MorgageHouse_Link_Shortner.Server.Services
{
    public class GuidObfuscator : IObfuscator<Guid>
    {
        public bool TryDecode(string enCoded, out Guid decoded )
        {
            try
            {
                var decodedBytes = WebEncoders.Base64UrlDecode(enCoded);
                decoded = new Guid(decodedBytes);
                return true;
            }
            catch
            {
                decoded = Guid.Empty;
                return false;
            }
        }

        public string Encode(Guid value)
        {
            return WebEncoders.Base64UrlEncode(value.ToByteArray());
        }
    }
}
