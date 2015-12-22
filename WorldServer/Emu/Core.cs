using System;
using System.Threading;
using Commons.Threading;
using Commons.Utils;
using WorldServer.Emu.Interfaces;
using WorldServer.Emu.Processors;

/*
  That file part of Code Monsters framework.
  Cerium Unity 2015 © 
*/
namespace WorldServer.Emu
{
    /// <summary>
    /// This class present World server single thread core
    /// </summary>
    internal class Core
    {
        /// <summary>
        /// Processors containeer
        /// </summary>
        internal class ProcessorsBlock
        {
            internal readonly object ProcessorsLock = new object();

            public AuthProcessor AuthProcessor { get; private set; }
            public LobbyProcessor LobbyProcessor { get; private set; }

            public CharacterProcessor CharacterProcessor { get; private set; }

            public void ReloadProcessors()
            {
                lock (ProcessorsLock)
                {
                    AuthProcessor = Reload(AuthProcessor);
                    LobbyProcessor = Reload(LobbyProcessor);
                    CharacterProcessor = Reload(CharacterProcessor);
                }
            }

            private static T Reload<T>(T source) where T : IProcessor
            {
                var instance = (T)Activator.CreateInstance(typeof(T));

                if (source != null)
                    instance.OnLoad(source.OnUnload());

                return instance;
            }
        }

        private readonly SingleThreadActor _engineActor = new SingleThreadActor();

        private readonly ProcessorsBlock _processorsBlock = new ProcessorsBlock();

        public delegate void CoreAction(ProcessorsBlock processors);

        public static readonly Core Instance = new Core();

        public Core()
        {
            _engineActor.Initialize();
        }

        /// <summary>
        /// Post action to main core thread
        /// </summary>
        /// <param name="action"></param>
        public static void Act(CoreAction action)
        {
            lock (Instance._processorsBlock.ProcessorsLock)
                Instance._engineActor.Act(state => action.Invoke(Instance._processorsBlock));
        }

        /// <summary>
        /// Post delayed action to main core thread
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delayMs">action delay</param>
        /// <param name="cancellationToken">cancellation token for this task</param>
        public static void Act(CoreAction action, long delayMs, CancellationTokenSource cancellationToken = null)
        {
            lock (Instance._processorsBlock.ProcessorsLock)
                Instance._engineActor.Act(state => action.Invoke(Instance._processorsBlock),
                    DateTime.Now.UnixMilliseconds() + delayMs, null, cancellationToken);
        }

        /// <summary>
        /// Start delayed task at selected DateTime
        /// </summary>
        /// <param name="action"></param>
        /// <param name="time"></param>
        /// <param name="cancellationToken">cancellation token for this task</param>
        public static void Act(CoreAction action, DateTime time, CancellationTokenSource cancellationToken = null)
        {
            lock (Instance._processorsBlock.ProcessorsLock)
                Instance._engineActor.Act(state => action.Invoke(Instance._processorsBlock),
                    time.UnixMilliseconds(), null, cancellationToken);
        }
    }
}
