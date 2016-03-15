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
			/* CLIENT ANY STATES */
            ClientFrames.TryAdd(0x03E9, typeof(CMSG_Heartbeat));
			ClientFrames.TryAdd(0x0FB2, typeof(CMSG_GetInstallationList));

			/* CLIENT LOBBY STATE */
			ClientFrames.TryAdd(0x0C98, typeof(CMSG_LoginUserToFieldServer));
			ClientFrames.TryAdd(0x0BE0, typeof(CMSG_CreateCharacterToField));
            ClientFrames.TryAdd(0x0BE3, typeof(CMSG_RemoveCharacterFromField));
			ClientFrames.TryAdd(0x0BDD, typeof(CMSG_RemoveCancelCharacterFromField));
			ClientFrames.TryAdd(0x0CE3, typeof(CMSG_EnterPlayerCharacterToField));
			ClientFrames.TryAdd(0x0C9A, typeof(CMSG_ExitFieldServerToServerSelection));

			/* CLIENT ENTER WORLD STATE */
			ClientFrames.TryAdd(0x10A7, typeof(CMSG_RefreshCacheData));
			ClientFrames.TryAdd(0x10D3, typeof(CMSG_ReadJournal));
			ClientFrames.TryAdd(0x0DAA, typeof(CMSG_ListSiegeGuild));
			ClientFrames.TryAdd(0x0F62, typeof(CMSG_GetWebBenefit));
			ClientFrames.TryAdd(0x0CE0, typeof(CMSG_SetReadyToPlay));
			ClientFrames.TryAdd(0x10D7, typeof(CMSG_WriteJournalPlayCutScene));
			ClientFrames.TryAdd(0x0CE0, typeof(CMSG_PaymentPasswordRegister));

			/* CLIENT IN GAME STATE */
			ClientFrames.TryAdd(0x0EA5, typeof(CMSG_Chat));
			ClientFrames.TryAdd(0x0BCA, typeof(CMSG_MovePlayer));
			ClientFrames.TryAdd(0x0BCF, typeof(CMSG_PlayerDirection));
			ClientFrames.TryAdd(0x0D0C, typeof(CMSG_DoAction));
			ClientFrames.TryAdd(0x0D05, typeof(CMSG_StartAction));

			/* CLIENT LOGOUT STATE */
			ClientFrames.TryAdd(0x0CF0, typeof(CMSG_ListWaitingCountOfMyCharacter));
			ClientFrames.TryAdd(0x10D8, typeof(CMSG_RecentJournal));
			ClientFrames.TryAdd(0x0CFC, typeof(CMSG_ListEnchantFailCountOfMyCharacter));
			ClientFrames.TryAdd(0x0CF9, typeof(CMSG_SetPlayerCharacterMemo));
			ClientFrames.TryAdd(0x0BDB, typeof(CMSG_BeginDelayedLogout));
			ClientFrames.TryAdd(0x0BD8, typeof(CMSG_CancelDelayedLogout));
			ClientFrames.TryAdd(0x0BD9, typeof(CMSG_EndDelayedLogout));

			/* SERVER LOBBY STATE */
			ServerFrames.TryAdd(typeof(SMSG_GetContentServiceInfo), 0x0C9C);
			ServerFrames.TryAdd(typeof(SMSG_ChargeUser), 0x0C0A);
			ServerFrames.TryAdd(typeof(SMSG_LoginUserToFieldServer), 0x0C99);
			ServerFrames.TryAdd(typeof(SMSG_FixedCharge), 0x0C78);
			ServerFrames.TryAdd(typeof(SMSG_CreateCharacterToField), 0x0BE1);
			ServerFrames.TryAdd(typeof(SMSG_RemoveCharacterFromField), 0x0BE4);
			ServerFrames.TryAdd(typeof(SMSG_CreateCharacterToFieldNak), 0x0BE2);
			ServerFrames.TryAdd(typeof(SMSG_ExitFieldServerToServerSelection), 0x0C9B);

			/* SERVER ENTER WORLD STATE */
			ServerFrames.TryAdd(typeof(SMSG_CancelFieldEnterWaiting), 0x0CF4);
			ServerFrames.TryAdd(typeof(SMSG_SetGameTime), 0x0D52);
			ServerFrames.TryAdd(typeof(SMSG_EnterPlayerCharacterToField), 0x0CE4);
			ServerFrames.TryAdd(typeof(SMSG_LoadField), 0x0CFE);
			ServerFrames.TryAdd(typeof(SMSG_AddPlayers), 0x0BB9);
			ServerFrames.TryAdd(typeof(SMSG_PlayerLogOnOff), 0x0D45);
			ServerFrames.TryAdd(typeof(SMSG_LoadFieldComplete), 0x0CFF);
			ServerFrames.TryAdd(typeof(SMSG_EnterPlayerCharacterToFieldComplete), 0x0CE5);

			/* SERVER IN GAME STATE */
			ServerFrames.TryAdd(typeof(SMSG_Chat), 0x0EAC);
			ServerFrames.TryAdd(typeof(SMSG_GetAllEquipSlot), 0x0D65);
			ServerFrames.TryAdd(typeof(SMSG_AddItemToInventory), 0x0BF1);
			ServerFrames.TryAdd(typeof(SMSG_SetCharacterLevels), 0x0F7E);
			ServerFrames.TryAdd(typeof(SMSG_RefreshPcCustomizationCache), 0x10A8);
			ServerFrames.TryAdd(typeof(SMSG_RefreshPcLearnedActiveSkillsCache), 0x10A9);
			ServerFrames.TryAdd(typeof(SBpPlayerSpawn.SMSG_RefreshPcEquipSlotCache), 0x10AA);
			ServerFrames.TryAdd(typeof(SBpPlayerSpawn.SMSG_RefreshUserBasicCache), 0x10AB);
			ServerFrames.TryAdd(typeof(SBpPlayerSpawn.SMSG_RefreshPcBasicCache), 0x10AC);

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
