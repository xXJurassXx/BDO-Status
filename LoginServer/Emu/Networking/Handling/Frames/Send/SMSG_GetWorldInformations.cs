using System;
using System.IO;
using System.Linq;
using System.Text;
using Commons.Models.Account;
using Commons.Models.Realm;
using Commons.Utils;
using LoginServer.Emu.Processors;
/*
   Author:Sagara, InCube
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
                writer.Skip(4);
                writer.Write((long)DateTime.Now.Ticks);

                var realms = NetworkService.WorldServers.Values.ToList();
                writer.Write((short)realms.Count);

                foreach (var realm in realms)
                {
                    writer.Write((short)realm.ChannelId); 
                    writer.Write((short)realm.Id);
                    writer.Write((short)rnd.Next(0xFF, 0xFFFF));

                    writer.Write(BinaryExt.WriteFixedString($"{realm.ChannelName}", Encoding.Unicode, 62));
                    writer.Write(BinaryExt.WriteFixedString($"{realm.RealmName}", Encoding.Unicode, 62));

                    writer.Write((byte)0x00);
                    writer.Write(BinaryExt.WriteFixedString(realm.RealmIp, Encoding.ASCII, 16));
                    writer.Write((byte)0x00);

                    var shit = new byte[84];
                    rnd.NextBytes(shit);
                    
                    writer.Write((short)realm.RealmPort); // Port
                    writer.Write((byte)0x01); // Status
                    writer.Write((byte)0x01); // Can join
                    writer.Write((byte)0x01); // unk

                    writer.Write((byte)AuthProcessor.GetCharacterCount(_accInfo.Id));
                    writer.Write((byte)0x00); // Characters To Delete

                    writer.Write((short) 0x0000); // Limitation
                    writer.Write((long) (DateTime.Now.Ticks - 3600)); // Channel delay UNIX
                    writer.Write((long) (DateTime.Now.Ticks - 1800)); // Last login to channel UNIX
                    writer.Write((byte) 0x00); // Bonus
                    var shit2 = new byte[13];
                    rnd.NextBytes(shit2);
                    writer.Write((byte) 0x00); // This guy deserves a medal!
                }

                return stream.ToArray();
            }
        }
    }
}
