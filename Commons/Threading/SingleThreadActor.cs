using System;
using System.Collections.Concurrent;
using System.Threading;
using Commons.Utils;
/*
  That file part of Code Monsters framework.
  Cerium Unity 2015 © 
*/
namespace Commons.Threading
{
    public class SingleThreadActor
    {
        private readonly ManualResetEvent _shutdownSignal = new ManualResetEvent(false);
        private readonly ConcurrentQueue<ActorAction> _actions = new ConcurrentQueue<ActorAction>();
        private bool _work;
        private readonly Thread _globalThread;

        public SingleThreadActor()
        {
            _globalThread = new Thread(() =>
            {
                while (_work)
                {
                    int count = _actions.Count;
                    for (int index = 0; index < count; ++index)
                    {
                        ActorAction result;

                        if (!_actions.TryDequeue(out result))
                            throw new Exception("Can't dequeue action");

                        if (result.CancellationToken != null && result.CancellationToken.IsCancellationRequested)
                            result.CancellationToken.Dispose();

                        else if (result.ActMs > DateTime.Now.UnixMilliseconds())
                            _actions.Enqueue(result);
                        else
                            result.Action(result.State);
                    }
                    Thread.Sleep((count + 1)%2);
                }
                _shutdownSignal.Set();
            });
        }

        public void Initialize()
        {
            _work = true;
            _globalThread.Start();
        }

        public void Shutdown()
        {
            _work = false;
            _shutdownSignal.WaitOne();
        }

        public void Act(ActorActionEvent action, long actMs = 0L, object state = null, CancellationTokenSource cancellationToken = null)
        {
            long num = DateTime.Now.UnixMilliseconds();
            if (actMs < num)
                actMs = num;

            _actions.Enqueue(new ActorAction
            {
                Action = action,
                ActMs = actMs,
                State = state,
                CancellationToken = cancellationToken
            });
        }

        public delegate void ActorActionEvent(object state);

        private class ActorAction
        {
            public CancellationTokenSource CancellationToken;
            public ActorActionEvent Action;
            public long ActMs;
            public object State;
        }
    }
}
