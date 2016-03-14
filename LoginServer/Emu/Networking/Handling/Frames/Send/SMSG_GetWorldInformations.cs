using System;
using System.IO;
using System.Linq;
using System.Text;
using Commons.Models.Account;
using Commons.Models.Realm;
using Commons.Utils;
using LoginServer.Emu.Processors;
/*
   Author: Sagara, InCube, RBW
*/
namespace LoginServer.Emu.Networking.Handling.Frames.Send
{
    // ReSharper disable once InconsistentNaming
    public class SMSG_GetWorldInformations : APacketProcessor
    {
        private readonly AccountData _accInfo;
        public SMSG_GetWorldInformations(AccountData accountInfo)
        {
            _accInfo = accountInfo;
        }

        public override byte[] WritedData()
        {
            var rnd = new Random((int)DateTime.Now.Ticks);
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write((int)0);
                writer.Write((long)1457907576); // server started time

                var realms = NetworkService.WorldServers.Values.ToList();
                writer.Write((short)realms.Count);

                foreach (var realm in realms)
                {
                    writer.Write((short)realm.ChannelId); // channel id
                    writer.Write((short)realm.Id); // server id
                    writer.Write((short)16384); // test random values

                    writer.Write(BinaryExt.WriteFixedString($"{realm.ChannelName}", Encoding.Unicode, 62)); // channel name
                    writer.Write(BinaryExt.WriteFixedString($"{realm.RealmName}", Encoding.Unicode, 62)); // server name

                    writer.Write((byte)0);
                    writer.Write(BinaryExt.WriteFixedString(realm.RealmIp, Encoding.ASCII, 16)); // ip
                    writer.Write((byte)0);

					writer.Write(new byte[84]); // padded bytes - possible string inside - never used

                    writer.Write((short)realm.RealmPort); // server port

                    writer.Write((byte)1); // server population status
                    writer.Write((byte)1); // can be joined by public
                    writer.Write((byte)1); // unk

                    writer.Write((byte)AuthProcessor.GetCharacterCount(_accInfo.Id)); // created characters count
                    writer.Write((byte)0); // characters to be delete count

                    writer.Write((short) 0); // weird limitation / block

                    writer.Write((long) 0); // channel relogin delay time
                    writer.Write((long) 0); // last login to channel time

                    writer.Write((byte) 0); // exp/drop bonus

					writer.Write(new byte[13]); // padded bytes - never used

					writer.Write((byte) 0); // medal
                }

                return stream.ToArray();
            }
        }
    }
}
