using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Interfaces.Saga
{
    public interface IActivity
    {
        string Name { get; }

        Func<Task> Action { get; }

        string Id { get; }

        ICompensactionActivity Step(string name, Func<Task> action);

        IActivity StepWithoutBackup(string name, Func<Task> action);

        Task End();
    }
}