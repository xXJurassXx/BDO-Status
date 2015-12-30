using System;
using System.Collections.Concurrent;
using System.IO;
using Commons.Utils;
using WorldServer.Configs;
using WorldServer.Emu.Networking.Handling.Frames.Recv;
using WorldServer.Emu.Networking.Handling.Frames.Send;
using WorldServer.Emu.Networking.Handling.Frames.Send.OPSBlob;
/*
   Author:Sagara
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
            ClientFrames.TryAdd(0x03e9, typeof(RpHeartbeat));
            ClientFrames.TryAdd(0x0c94, typeof(RpGetToken));
            ClientFrames.TryAdd(0x0be0, typeof(RpCreateCharacter));
            ClientFrames.TryAdd(0x0be3, typeof(RpDeleteCharacter));
            ClientFrames.TryAdd(0x0cdb, typeof(RpEnterOnWorldRequest));
            ClientFrames.TryAdd(0x10b0, typeof(RpEnterOnWorldProcess));
            ClientFrames.TryAdd(0x0bdb, typeof(RpRequestDisconnect));
            ClientFrames.TryAdd(0x0e87, typeof(RpChat));
            ClientFrames.TryAdd(0x0d04, typeof(RpMovement));

            ServerFrames.TryAdd(typeof(SpUnk), 0x0c98);
            ServerFrames.TryAdd(typeof(SpUnk2), 0x0c74);
            ServerFrames.TryAdd(typeof(SpCharacterList), 0x0c95);
            ServerFrames.TryAdd(typeof(SpCreateCharacter), 0x0be1);
            ServerFrames.TryAdd(typeof(SpCreateCharacterError), 0x0be2);
            ServerFrames.TryAdd(typeof(SpDeleteCharacter), 0x0be4);
            ServerFrames.TryAdd(typeof(SpEnterOnWorldResponse), 0xcec);
            ServerFrames.TryAdd(typeof(SpSpawnCharacter), 0x0cdc);
            ServerFrames.TryAdd(typeof(SpCharacterInformation), 0x0d3a);
            ServerFrames.TryAdd(typeof(SpCharacterCustimozationData), 0x1085);
            ServerFrames.TryAdd(typeof(SpCharacterCustomizationResponse), 0x1086);
            ServerFrames.TryAdd(typeof(SpChat), 0x0e8e);
            ServerFrames.TryAdd(typeof(SpInventory), 0x0bf1); 
            ServerFrames.TryAdd(typeof(SpCharacterEquipment), 0x0d5a); 
            ServerFrames.TryAdd(typeof(SpUpdateLevel), 0x0f5f); 
            ServerFrames.TryAdd(typeof(SBpPlayerSpawn.SpSetPlayerName), 0x1089); 
            ServerFrames.TryAdd(typeof(SBpPlayerSpawn.SpSetPlayerEquipment), 0x1087); 
            ServerFrames.TryAdd(typeof(SBpPlayerSpawn.SpSetPlayerFamilyName), 0x1088);
            ServerFrames.TryAdd(typeof(SBpPlayerSpawn.SpSpawnPlayer), 0x0bb9);
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
                var isCrypt = reader.ReadBoolean(); //need just read for move position, we decrypt before
                var sequence = reader.ReadInt16();
                var opCode = reader.ReadInt16();
                var body = reader.ReadBytes(packetBody.Length - 5); //without crypt flag, sequence and opCode length

                client.SequenceId = sequence; //install sequence id

                lock(RLock)
                    RecvEvent?.Invoke(opCode, body, isCrypt);

                if (ClientFrames.ContainsKey(opCode)) //check, if packet exist
                    ((APacketProcessor)Activator.CreateInstance(ClientFrames[opCode])).Process(client, body);//process packet  
                else if(CfgCore.Default.LogUnkPackets)
                    Console.WriteLine($"Unknown packet\nOpCode {opCode}\nData:\n {body.FormatHex()}"); //if packet not exist, we cannot proccess hem, just write log
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
