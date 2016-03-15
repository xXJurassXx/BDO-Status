using System;
using System.IO;
using Commons.Enums;
using Commons.Models.Character;
using Commons.Utils;
/*
   Author: Sagara, RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    public class CMSG_CreateCharacterToField : APacketProcessor
    {       
        public override void Process(ClientConnection client, byte[] data)
        {
            var info = new CharacterData();

            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
				/* c,c,S(62),c,b(10),b(800) */
                var profession = (ClassType)reader.ReadByte();
                if (!Enum.IsDefined(typeof(ClassType), profession))
                {
                    Log.Error("Profession with hashcode {0} does not exist!", profession);
                    return;
                }
                
                var slot = reader.ReadByte();
                var name = reader.ReadString(62).Replace("\0", "");

                var zodiac = (Zodiac)reader.ReadByte();
                if (!Enum.IsDefined(typeof(Zodiac), zodiac))
                {
                    Log.Error("Zodiac with hashcode {0} does not exist!", zodiac);
                    return;
                }

                var appearancePresets = reader.ReadBytes(10); // (byte)face, (byte)hair, (byte)facialHair1, (byte)facialHair2, (byte)facialHair3, (byte)eyebrowns, (int)unk
				var appearanceOptions = reader.ReadBytes(800); // appearance full data

#if DEBUG
                Log.Debug("Character Creation Report:\n\tName: '{0}'\n\tSlot: {1}\n\tProfession: '{2}'\n\tZodiac: '{3}'", name, slot, profession, zodiac);
#endif

                Log.Info(appearancePresets.FormatHex());
                Log.Info(appearanceOptions.FormatHex());

                info.AccountId = client.Account.Id;
                info.CharacterName = name;
                info.ClassType = profession;
                info.Zodiac = zodiac;
                info.AppearancePresets = appearancePresets;
                info.AppearanceOptions = appearanceOptions;

                Core.Act(s => s.LobbyProcessor.CreateCharacterProcess(client, info));
            }
        }
    }
}