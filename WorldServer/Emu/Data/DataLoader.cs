using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NLog;
using WorldServer.Emu.Data.Templates;

namespace WorldServer.Emu.Data
{
    public static class DataLoader
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static string DataPath = "./static_data/";

        public static ItemDataTemplate Items { get; private set; }

        private static readonly List<Loader> Loaders = new List<Loader>
        {
            LoadItemPack
        };

        private delegate int Loader();

        public static void LoadAll(string datapath = "./static_data/")
        {
            DataPath = datapath;

            while (!Parallel.For(0, Loaders.Count, delegate (int i)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                var readed = Loaders[i].Invoke();
                stopwatch.Stop();

                Log.Info("Data: {0,-26} {1,7} values in {2}s", Loaders[i].Method.Name, readed, (stopwatch.ElapsedMilliseconds / 1000.0).ToString("0.00"));
            }).IsCompleted)
            {
            }

            Thread.Sleep(1000);
        }

        public static int LoadItemPack()
        {
            Items = Deserialize<ItemDataTemplate>("Item_Table.xml");

            return Items.ItemTemplateList.Count;
        }

        private static TXml Deserialize<TXml>(string fileName)
        {
            TXml t = default(TXml);

            if (File.Exists($"{DataPath}{fileName}"))
                using (var xmlStream = File.OpenRead(Path.Combine(DataPath, fileName)))
                    t = (TXml) new XmlSerializer(typeof (TXml)).Deserialize(xmlStream);                
           
            return t;
        }
    }
}
