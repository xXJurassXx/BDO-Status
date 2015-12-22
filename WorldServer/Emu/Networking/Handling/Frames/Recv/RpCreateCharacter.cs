using System;
using System.IO;
using Commons.Enums;
using Commons.Models.Character;
using Commons.Utils;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    public class RpCreateCharacter : APacketProcessor
    {       
        public override void Process(ClientConnection client, byte[] data)
        {
            var info = new CharacterData();

            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                var profession = (ClassType)reader.ReadByte();

                if (!Enum.IsDefined(typeof(ClassType), profession))
                {
                    Log.Error("Profession with hashcode {0} are not exist", profession);
                    return;
                }
                
                var unk = reader.ReadByte();
                var name = reader.ReadString(62).Replace("\0", "");
                var zodiac = (Zodiac)reader.ReadByte();

                if (!Enum.IsDefined(typeof(Zodiac), zodiac))
                {
                    Log.Error("Zodiac with hashcode {0} are not exist", zodiac);
                    return;
                }

                var appearancePresets = reader.ReadBytes(10);
                var appearanceOptions = reader.ReadBytes(800);

#if DEBUG
                Log.Debug(
                    "Character creation report\n\tName: '{0}'\n\tUnk: {1}\n\tProfession: '{2}'\n\tZodiac: '{3}'",
                    name, unk, profession, zodiac);
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