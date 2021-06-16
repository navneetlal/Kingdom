using System.Threading.Tasks;

namespace KingdomApi.Services
{
    public class OtpManager
    {
        public async static Task<string> GenerateOTP()
        {
            var number = await Nanoid.Nanoid.GenerateAsync("0123456789", 6);
            var code = await Nanoid.Nanoid.GenerateAsync("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 2);
            return $"{code}-{number}";
        }
    }
}
