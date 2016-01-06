using System;
using System.IO;
using System.Linq;
using System.Text;
using Commons.Models.Account;
using Commons.Utils;
using LoginServer.Emu.Processors;
/*
   Author:Sagara
*/
namespace LoginServer.Emu.Networking.Handling.Frames.Send
{
    public class SpServerlist : APacketProcessor
    {
        private readonly AccountData _accInfo;
        public SpServerlist(AccountData accountInfo)
        {
            _accInfo = accountInfo;
        }

        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Skip(4);
                writer.WriteD(DateTime.Now.Millisecond);
                writer.Skip(4);

                var realms = NetworkService.WorldServers.Values.ToList();

                writer.WriteH(realms.Count);

                for (int i = 0; i < realms.Count; i++)
                {
                    var realm = realms[i];

                    writer.WriteH(realm.ChannelId); 
                    writer.WriteH(realm.Id);

                    writer.Write(BinaryExt.WriteFixedString($" {realm.ChannelName}", Encoding.Unicode, 62));
                    writer.Write(BinaryExt.WriteFixedString($" {realm.RealmName}", Encoding.Unicode, 62));

                    writer.Write("CB4B00".ToBytes());

                    writer.Write(BinaryExt.WriteFixedString(realm.RealmIp, Encoding.ASCII, 16));
                    writer.WriteH(realm.RealmPort);

                    writer.WriteC(1);
                    writer.WriteC(1);
                    writer.WriteC(1);

                    writer.WriteC(AuthProcessor.GetCharacterCount(_accInfo.Id));
                    writer.WriteC(_accInfo.MaxSlotCount);

                    writer.Write("0000FEFFFFFFFFFFFFFFD48D6155000000000000000000000000000000000000".ToBytes());
                }

                return stream.ToArray();
            }
        }
    }
}
