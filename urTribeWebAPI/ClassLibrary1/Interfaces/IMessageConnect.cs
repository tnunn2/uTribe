
namespace urTribeWebAPI.Messaging
{
    public interface IMessageConnect
    {
        string SendRequest(string url, string data);
    }
}
