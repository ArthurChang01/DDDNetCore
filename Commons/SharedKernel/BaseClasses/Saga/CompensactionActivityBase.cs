using SharedKernel.Interfaces.Saga;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.BaseClasses.Saga
{
    public class CompensactionActivityBase : ICompensactionActivity
    {
        private string _id = Guid.NewGuid().ToString();
        private SagaBuilder _sagaBuilder;

        public CompensactionActivityBase(SagaBuilder sagaBuilder)
        {
            _sagaBuilder = sagaBuilder;
            this.Name = "Empty";
            this.Action = async () => { await Task.Run(() => { }); };
        }

        public string Id { get { return this._id; } }
        public string Name { get; private set; }
        public Func<Task> Action { get; private set; }

        public IActivity Undo(string name, Func<Task> action)
        {
            this.Name = name;
            this.Action = action;

            this._sagaBuilder.AddCompensactionActivity(this);

            IActivity activity = new ActivityBase(this._sagaBuilder);

            return activity;
        }
    }
}