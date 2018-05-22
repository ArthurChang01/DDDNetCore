using SharedKernel.Interfaces.Saga;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.BaseClasses.Saga
{
    public class SagaBuilder : IDisposable
    {
        #region Members

        private bool disposedValue = false;
        private int _counter = 0;
        private string _lastActivityId;
        private Queue<IActivity> _activityQ;
        private Stack<ICompensactionActivity> _activityT;

        #endregion Members

        #region Constructor

        public SagaBuilder()
        {
            this._activityQ = new Queue<IActivity>();
            this._activityT = new Stack<ICompensactionActivity>();
        }

        #endregion Constructor

        #region Properties

        public string Name { get; private set; }

        public int Amount { get { return this._activityQ.Count; } }

        #endregion Properties

        #region Public Methods

        public IActivity NewSaga(string name)
        {
            this.Name = name;

            IActivity activity = new ActivityBase(this);

            return activity;
        }

        public async Task Run()
        {
            try
            {
                this._lastActivityId = this._activityQ.Peek().Id;
                while (this._activityQ.TryDequeue(out IActivity activity))
                {
                    this._counter++;
                    await activity.Action();
                    this._lastActivityId = activity.Id;
                }
            }
            catch (Exception)
            {
                await this._pushBackAsync();
                throw;
            }
        }

        public void Reset()
        {
            this._activityQ.Clear();
            this._activityT.Clear();
            this._counter = 0;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void AddCompensactionActivity(CompensactionActivityBase item)
        {
            this._activityT.Push(item);
        }

        internal void AddActivity(ActivityBase item)
        {
            this._activityQ.Enqueue(item);
        }

        #endregion Internal Methods

        #region Private Methods

        private async Task _pushBackAsync()
        {
            int skipTimes = this._activityT.Count - this._counter;

            while (this._activityT.TryPop(out ICompensactionActivity activity))
            {
                if ((skipTimes--) > 0) continue;

                await activity.Action();
            }
        }

        #endregion Private Methods

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this._activityQ.Clear();
                    this._activityT.Clear();
                }

                this._activityQ = null;
                this._activityT = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}