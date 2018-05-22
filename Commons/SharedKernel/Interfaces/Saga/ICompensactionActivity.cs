using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Interfaces.Saga
{
    public interface ICompensactionActivity
    {
        string Name { get; }

        Func<Task> Action { get; }

        string Id { get; }

        IActivity Undo(string name, Func<Task> action);
    }
}