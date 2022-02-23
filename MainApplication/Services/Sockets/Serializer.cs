using System.Text;

namespace MainApplication.Services.Sockets
{
    public static class Serializer
    {
        public static byte[] Serialize(string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }

        public static string Deserialize(byte[] buffer, int count)
        {
            return Encoding.UTF8.GetString(buffer, 0, count);
        }
    }
}
