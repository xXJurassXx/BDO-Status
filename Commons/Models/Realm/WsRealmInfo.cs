using System;

namespace Commons.Models.Realm
{
    [Serializable]
    public class WsRealmInfo
    {
        public int Id;
        public int ChannelId;
        public string ChannelName;
        public string RealmName;
        public string RealmPassword;
        public string RealmIp;
        public int RealmPort;
    }
}
