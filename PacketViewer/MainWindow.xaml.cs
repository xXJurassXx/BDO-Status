using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using Commons.Utils;
using Microsoft.Win32;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;

namespace PacketViewer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static List<GamePacket> _packets = new List<GamePacket>();
        private static MainWindow _instance;

        public MainWindow()
        {
            InitializeComponent();
            _instance = this;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var od = new OpenFileDialog { Filter = "*.pcap|*.pcap|*.cap|*.cap" };
            if (od.ShowDialog() == true)
            {
                try
                {
                    var selectedDevice = new OfflinePacketDevice(od.FileName);
                    using (var communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
                    {
                        communicator.SetFilter("tcp port 8889");
                        communicator.ReceivePackets(0, DispatcherHandler);
                    }

                    _instance.Logger.Content = $"Packets {_packets.Count} readed";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Something wrong\n{ex}");
                }
            }
        }

        private void DispatcherHandler(Packet packet)
        {
            IpV4Datagram ip = packet.Ethernet.IpV4;


        }

        public struct GamePacket
        {
            public string From;
            public short OpCode;
            public byte[] Data;
        }
    }
}
