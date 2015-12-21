using System;
using System.Collections.Concurrent;
using System.IO;
using Commons.Utils;
using WorldServer.Emu.Networking.Handling.Frames.Recv;
using WorldServer.Emu.Networking.Handling.Frames.Send;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking.Handling
{
    public static class PacketHandler
    {
        private static readonly ConcurrentDictionary<short, Type> ClientFrames = new ConcurrentDictionary<short, Type>();
        private static readonly ConcurrentDictionary<Type, short> ServerFrames = new ConcurrentDictionary<Type, short>();

        static PacketHandler()
        {
            ClientFrames.TryAdd(0x0c94, typeof(RpGetToken));
            ClientFrames.TryAdd(0x0be0, typeof(RpCreateCharacter));
            ClientFrames.TryAdd(0x0be3, typeof(RpDeleteCharacter));
            ClientFrames.TryAdd(0x0CDB, typeof(RpEnterOnWorldRequest));

            ServerFrames.TryAdd(typeof(SpUnk), 0x0c98);
            ServerFrames.TryAdd(typeof(SpCharacterList), 0x0c95);
            ServerFrames.TryAdd(typeof(SpCreateCharacter), 0x0be1);
            ServerFrames.TryAdd(typeof(SpCreateCharacterError), 0x0be2); 
            ServerFrames.TryAdd(typeof(SpDeleteCharacter), 0x0be4);
            ServerFrames.TryAdd(typeof(SpEnterOnWorldResponse), 0x0CEC);
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

                if (ClientFrames.ContainsKey(opCode)) //check, if packet exist
                    ((APacketProcessor)Activator.CreateInstance(ClientFrames[opCode])).Process(client, body);//process packet  
                else
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
    }
}
