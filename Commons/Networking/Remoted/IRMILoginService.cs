using Commons.Models.Realm;

namespace Commons.Networking.Remoted
{
    public interface IRMILoginService
    {
        void Register(WsRealmInfo info);
    }
}
