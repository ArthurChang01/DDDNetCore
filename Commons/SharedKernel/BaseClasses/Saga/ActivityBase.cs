using SharedKernel.Interfaces.Saga;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.BaseClasses.Saga
{
    public class ActivityBase : IActivity
    {
        private string _id = Guid.NewGuid().ToString();
        private readonly SagaBuilder _sagaBuilder;

        public ActivityBase(SagaBuilder sb)
        {
            this._sagaBuilder = sb;
        }

        public string Id { get { return this._id; } }
        public string Name { get; private set; }
        public Func<Task> Action { get; private set; }

        public ICompensactionActivity Step(string name, Func<Task> action)
        {
            this.Name = name;
            this.Action = action;

            this._sagaBuilder.AddActivity(this);

            ICompensactionActivity compensaction = new CompensactionActivityBase(this._sagaBuilder);

            return compensaction;
        }

        public IActivity StepWithoutBackup(string name, Func<Task> action)
        {
            this.Name = name;
            this.Action = action;

            this._sagaBuilder.AddActivity(this);
            this._sagaBuilder.AddCompensactionActivity(new CompensactionActivityBase(this._sagaBuilder));

            return new ActivityBase(this._sagaBuilder);
        }

        public async Task End()
        {
            await this._sagaBuilder.Run();
        }
    }
}