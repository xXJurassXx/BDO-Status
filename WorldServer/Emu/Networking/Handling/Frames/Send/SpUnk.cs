﻿using System.IO;
using Commons.Utils;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SpUnk : APacketProcessor
    {
        private byte[] unk = //0x0c98
        {
            0x01,
            0x04, 0x09, 0xc0, 0xc6, 0x2d, 0x00, 0x00, 0x00,
            0x00, 0x00, 0xe0, 0x93, 0x04, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x40, 0x0d, 0x03, 0x00, 0x40, 0x0d,
            0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x04, 0x08, 0x0c, 0x10, 0x1c,
            0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
            0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
            0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
            0x20, 0x20, 0x33, 0x00, 0x00, 0x00, 0x01, 0x00,
            0x00, 0x00, 0x80, 0x51, 0x01, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x80, 0x51, 0x01, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x80, 0x51, 0x01, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x01, 0x00, 0x06, 0x00, 0x00,
            0x96, 0x00, 0x00, 0x00, 0x32, 0x00, 0x04, 0x01,
            0x00, 0x04, 0x02, 0x00, 0x04, 0x03, 0x00, 0x04,
            0x04, 0x00, 0x04, 0x05, 0x00, 0x04, 0x06, 0x00,
            0x04, 0x07, 0x00, 0x04, 0x08, 0x00, 0x04, 0x09,
            0x00, 0x04, 0x0b, 0x00, 0x04, 0x0c, 0x00, 0x04,
            0x0d, 0x00, 0x04, 0x0e, 0x00, 0x04, 0x0f, 0x00,
            0x04, 0x10, 0x00, 0x04, 0x11, 0x00, 0x04, 0x12,
            0x00, 0x04, 0x13, 0x00, 0x04, 0x14, 0x00, 0x04,
            0x15, 0x00, 0x04, 0x16, 0x00, 0x04, 0x17, 0x00,
            0x04, 0x18, 0x00, 0x04, 0x19, 0x00, 0x04, 0x1f,
            0x00, 0x04, 0x21, 0x00, 0x04, 0x22, 0x00, 0x04,
            0x24, 0x00, 0x04, 0x25, 0x00, 0x04, 0x26, 0x00,
            0x04, 0x27, 0x00, 0x04, 0x28, 0x00, 0x04, 0x29,
            0x00, 0x04, 0x2a, 0x00, 0x04, 0x2b, 0x00, 0x04,
            0x2c, 0x00, 0x04, 0x2d, 0x00, 0x04, 0x2e, 0x00,
            0x04, 0x3d, 0x00, 0x04, 0x3e, 0x00, 0x04, 0x3f,
            0x00, 0x04, 0x40, 0x00, 0x04, 0x65, 0x00, 0x04,
            0x66, 0x00, 0x04, 0x67, 0x00, 0x04, 0x68, 0x00,
            0x04, 0x69, 0x00, 0x04, 0x6a, 0x00, 0x04, 0x6b,
            0x00, 0x04, 0x6c, 0x00
        };

        public override byte[] WritedData()
        {
            //that client settings(opened slots, available classes)
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.WriteC(4);
                writer.WriteC(10);
                writer.WriteC(10);
                writer.Write("C0C62D0000000000E093040000000000E0930400E093040060EA0000000000000004080C10141518191A1C1F2020202020202020202020202020202020202020000000002E000000805101000000000080F403000000000080812B00000000000001010D000096000000010000".ToBytes());

                return stream.ToArray();
            }        
        }
    }
}
