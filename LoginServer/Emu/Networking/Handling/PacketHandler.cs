/*
   Author: Sagara, RBW
*/
using System;
using System.Collections.Concurrent;
using System.IO;
using Commons.Utils;
using LoginServer.Emu.Networking.Handling.Frames.Recv;
using LoginServer.Emu.Networking.Handling.Frames.Send;

namespace LoginServer.Emu.Networking.Handling
{
    public static class PacketHandler
    {
        private static readonly ConcurrentDictionary<short, Type> ClientFrames = new ConcurrentDictionary<short, Type>();
        private static readonly ConcurrentDictionary<Type, short> ServerFrames = new ConcurrentDictionary<Type, short>();

        static PacketHandler()
        {
			/* NA-EU v63 */

            ClientFrames.TryAdd(0x03E9, typeof(CMSG_Heartbeat));
            ClientFrames.TryAdd(0x0C7A, typeof(CMSG_GetCreateUserInformationToAuthenticServer));
            ClientFrames.TryAdd(0x0C7C, typeof(CMSG_LoginUserToAuthenticServer));
            ClientFrames.TryAdd(0x0C7F, typeof(CMSG_RegisterNickNameToAuthenticServer));
			//ClientFrames.TryAdd(0x0C81, typeof(CMSG_GetWorldInformations));

			ServerFrames.TryAdd(typeof(SMSG_FixedCharge), 0x0C79);
			ServerFrames.TryAdd(typeof(SMSG_GetCreateUserInformationToAuthenticServer), 0x0C7B);
            ServerFrames.TryAdd(typeof(SMSG_LoginUserToAuthenticServer), 0xC7D);
            ServerFrames.TryAdd(typeof(SMSG_RegisterNickNameToAuthenticServer), 0x0C80);
            ServerFrames.TryAdd(typeof(SMSG_GetWorldInformations), 0x0C82);
			ServerFrames.TryAdd(typeof(SMSG_GetContentServiceInfo), 0xC9D);
		}

        /// <summary>
        /// Handle packet
        /// </summary>
        /// <param name="client">Client connection</param>
        /// <param name="packetBody">packet data</param>
        public static void Process(ClientConnection client, byte[] packetBody)
        {
            if (packetBody[0] == 1) // if crypto flag is 1, decrypt
                client.Session.Transform(ref packetBody);

            using (var stream = new MemoryStream(packetBody))
            using (var reader = new BinaryReader(stream))
            {
                reader.ReadBoolean();
                var sequence = reader.ReadInt16();
                var opCode = reader.ReadInt16();
                var body = reader.ReadBytes(packetBody.Length - 5); // without crypt flag, sequence and opCode length

                client.SequenceId = sequence; // install sequence id

                if (ClientFrames.ContainsKey(opCode))
                {
                    Console.WriteLine("OpCode: {0:X4} (CMSG => {2})\n{1}", opCode, body.FormatHex(), ClientFrames[opCode].Name);
                    ((APacketProcessor) Activator.CreateInstance(ClientFrames[opCode])).Process(client, body);
                }
                else
                    Console.WriteLine($"Unknown packet\nOpCode {opCode}\nData:\n {body.FormatHex()}");
                        // if packet not exist, we cannot proccess hem, just write log
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
