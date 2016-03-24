using System;
using System.Collections.Concurrent;
using System.IO;
using Commons.Utils;
using WorldServer.Configs;
using WorldServer.Emu.Networking.Handling.Frames.Recv;
using WorldServer.Emu.Networking.Handling.Frames.Send;
using WorldServer.Emu.Networking.Handling.Frames.Send.OPSBlob;
/*
   Author: Sagara, RBW
*/
namespace WorldServer.Emu.Networking.Handling
{
    public static class PacketHandler
    {
        private static readonly ConcurrentDictionary<short, Type> ClientFrames = new ConcurrentDictionary<short, Type>();
        private static readonly ConcurrentDictionary<Type, short> ServerFrames = new ConcurrentDictionary<Type, short>();

        public static event RecvPacketCallback RecvEvent;
        private static readonly object RLock = new object();

        static PacketHandler()
        {
			/* NA-EU v63 */

			/* CLIENT ANY STATES */
            ClientFrames.TryAdd(0x03E9, typeof(CMSG_Heartbeat));
			ClientFrames.TryAdd(0x0FB6, typeof(CMSG_GetInstallationList));

			/* CLIENT LOBBY STATE */
			ClientFrames.TryAdd(0x0C99, typeof(CMSG_LoginUserToFieldServer));
			ClientFrames.TryAdd(0x0BE0, typeof(CMSG_CreateCharacterToField));
            ClientFrames.TryAdd(0x0BE3, typeof(CMSG_RemoveCharacterFromField));
			ClientFrames.TryAdd(0x0BDD, typeof(CMSG_RemoveCancelCharacterFromField));
			ClientFrames.TryAdd(0x0CE4, typeof(CMSG_EnterPlayerCharacterToField));
			ClientFrames.TryAdd(0x0C9B, typeof(CMSG_ExitFieldServerToServerSelection));

			/* CLIENT ENTER WORLD STATE */
			ClientFrames.TryAdd(0x10AC, typeof(CMSG_RefreshCacheData));
			ClientFrames.TryAdd(0x10D9, typeof(CMSG_ReadJournal));
			ClientFrames.TryAdd(0x0DAD, typeof(CMSG_ListSiegeGuild));
			ClientFrames.TryAdd(0x0F66, typeof(CMSG_GetWebBenefit));
			ClientFrames.TryAdd(0x0CEE, typeof(CMSG_SetReadyToPlay));
			ClientFrames.TryAdd(0x10DD, typeof(CMSG_WriteJournalPlayCutScene));
			ClientFrames.TryAdd(0x0CE1, typeof(CMSG_PaymentPasswordRegister));

			/* CLIENT IN GAME STATE */
			ClientFrames.TryAdd(0x0EA8, typeof(CMSG_Chat));
			ClientFrames.TryAdd(0x0BCA, typeof(CMSG_MovePlayer));
			ClientFrames.TryAdd(0x0BCF, typeof(CMSG_PlayerDirection));
			ClientFrames.TryAdd(0x0D0D, typeof(CMSG_DoAction));
			ClientFrames.TryAdd(0x0D06, typeof(CMSG_StartAction));
			ClientFrames.TryAdd(0x0E77, typeof(CMSG_ClearMiniGame));
			ClientFrames.TryAdd(0x0E61, typeof(CMSG_AcceptQuest));
			ClientFrames.TryAdd(0x0E64, typeof(CMSG_CompleteQuest));
			ClientFrames.TryAdd(0x0E60, typeof(CMSG_SaveCheckedQuest));
			ClientFrames.TryAdd(0x0E49, typeof(CMSG_ContactNpc));

			/* CLIENT LOGOUT STATE */
			ClientFrames.TryAdd(0x0CF1, typeof(CMSG_ListWaitingCountOfMyCharacter));
			ClientFrames.TryAdd(0x10DE, typeof(CMSG_RecentJournal));
			ClientFrames.TryAdd(0x0CFD, typeof(CMSG_ListEnchantFailCountOfMyCharacter));
			ClientFrames.TryAdd(0x0CFA, typeof(CMSG_SetPlayerCharacterMemo));
			ClientFrames.TryAdd(0x0CEF, typeof(CMSG_ExitFieldToCharacterSelection));
			ClientFrames.TryAdd(0x0BDB, typeof(CMSG_BeginDelayedLogout));
			ClientFrames.TryAdd(0x0BD8, typeof(CMSG_CancelDelayedLogout));
			ClientFrames.TryAdd(0x0BD9, typeof(CMSG_EndDelayedLogout));

			/* SERVER LOBBY STATE */
			ServerFrames.TryAdd(typeof(SMSG_FixedCharge), 0x0C79);
			ServerFrames.TryAdd(typeof(SMSG_GetContentServiceInfo), 0x0C9D);
			ServerFrames.TryAdd(typeof(SMSG_ChargeUser), 0x0C0A);
			ServerFrames.TryAdd(typeof(SMSG_LoginUserToFieldServer), 0x0C9A);
			ServerFrames.TryAdd(typeof(SMSG_CreateCharacterToField), 0x0BE1);
			ServerFrames.TryAdd(typeof(SMSG_RemoveCharacterFromField), 0x0BE4);
			ServerFrames.TryAdd(typeof(SMSG_CreateCharacterToFieldNak), 0x0BE2);
			ServerFrames.TryAdd(typeof(SMSG_ExitFieldServerToServerSelection), 0x0C9C);

			/* SERVER ENTER WORLD STATE */
			ServerFrames.TryAdd(typeof(SMSG_CancelFieldEnterWaiting), 0x0CF5);
			ServerFrames.TryAdd(typeof(SMSG_SetGameTime), 0x0D55);
			ServerFrames.TryAdd(typeof(SMSG_EnterPlayerCharacterToField), 0x0CE5);
			ServerFrames.TryAdd(typeof(SMSG_LoadField), 0x0CFF);
			ServerFrames.TryAdd(typeof(SMSG_AddPlayers), 0x0BB9);
			ServerFrames.TryAdd(typeof(SMSG_PlayerLogOnOff), 0x0D48);
			ServerFrames.TryAdd(typeof(SMSG_LoadFieldComplete), 0x0D00);
			ServerFrames.TryAdd(typeof(SMSG_EnterPlayerCharacterToFieldComplete), 0x0CE6);

			/* SERVER IN GAME STATE */
			ServerFrames.TryAdd(typeof(SMSG_Chat), 0x0EAF);
			ServerFrames.TryAdd(typeof(SMSG_ContactNpc), 0x0E4A);
			ServerFrames.TryAdd(typeof(SMSG_AcceptQuest), 0x0E62);
			ServerFrames.TryAdd(typeof(SMSG_UpdateQuest), 0x0E63);
			ServerFrames.TryAdd(typeof(SMSG_CompleteQuest), 0x0E65);
			ServerFrames.TryAdd(typeof(SMSG_GetAllEquipSlot), 0x0D68);
			ServerFrames.TryAdd(typeof(SMSG_AddItemToInventory), 0x0BF1);
			ServerFrames.TryAdd(typeof(SMSG_SetCharacterLevels), 0x0F82);
			ServerFrames.TryAdd(typeof(SMSG_RefreshPcCustomizationCache), 0x10AE);
			ServerFrames.TryAdd(typeof(SMSG_RefreshPcLearnedActiveSkillsCache), 0x10AF);
			ServerFrames.TryAdd(typeof(SBpPlayerSpawn.SMSG_RefreshPcEquipSlotCache), 0x10B0);
			ServerFrames.TryAdd(typeof(SBpPlayerSpawn.SMSG_RefreshUserBasicCache), 0x10B1);
			ServerFrames.TryAdd(typeof(SBpPlayerSpawn.SMSG_RefreshPcBasicCache), 0x10B2);

			/* SERVER LOGOUT STATE */
			ServerFrames.TryAdd(typeof(SMSG_ExitFieldToCharacterSelection), 0x0CF0);
			ServerFrames.TryAdd(typeof(SMSG_BeginDelayedLogout), 0x0BDC);

			/* TODO: CLEANUP BELOW - ALL OPCODES OUTDATED */
			//ServerFrames.TryAdd(typeof(SpEnterOnWorldResponse), 0xcf2);
			//ServerFrames.TryAdd(typeof(SpCharacterInformation), 0x0d3a);
			//ServerFrames.TryAdd(typeof(SBpPlayerSpawn.SpSpawnPlayer), 0x0BB9); // SMSG_AddPlayers
		}

		/// <summary>
		/// Handle packet
		/// </summary>
		/// <param name="client">Client connection</param>
		/// <param name="packetBody">packet data</param>
		public static void Process(ClientConnection client, byte[] packetBody)
        {
            if (packetBody[0] == 1) //if crypto flag is 1, decrypt
                client.Session.Transform(ref packetBody);

            using (var stream = new MemoryStream(packetBody))
            using (var reader = new BinaryReader(stream))
            {
                var isCrypt = reader.ReadBoolean(); // need just read for move position, we decrypt before
                var sequence = reader.ReadInt16();
                var opCode = reader.ReadInt16();
                var body = reader.ReadBytes(packetBody.Length - 5); // without crypt flag, sequence and opCode length

                client.SequenceId = sequence; // install sequence id

                lock(RLock)
                    RecvEvent?.Invoke(opCode, body, isCrypt);

                if (ClientFrames.ContainsKey(opCode)) // check, if packet exist
                    ((APacketProcessor)Activator.CreateInstance(ClientFrames[opCode])).Process(client, body); // process packet  
                else if(CfgCore.Default.LogUnkPackets)
                    Console.WriteLine($"Unknown packet\nOpCode {opCode}\nData:\n {body.FormatHex()}"); // if packet not exist, we cannot proccess hem, just write log
            }
        }

        /// <summary>
        /// Get the opcode by Packet type
        /// </summary>
        /// <param name="type">Packet type</param>
        /// <returns></returns>
        public static short GetOpCode(Type type)
        {
            if (!ServerFrames.ContainsKey(type))
                throw new Exception($"Cannot find packet OpCode for {type.Name}");

            return ServerFrames[type];
        }

        public delegate void RecvPacketCallback(short opCode, byte[] data, bool isCrypt = false);
    }
}
