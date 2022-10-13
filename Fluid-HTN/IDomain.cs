using System.Collections.Generic;
using FluidHTN.Compounds;

namespace FluidHTN
{
    public interface IDomain<TWorldStateEntry>
    {
        TaskRoot<TWorldStateEntry> Root { get; }
        void Add(ICompoundTask<TWorldStateEntry> parent, ITask<TWorldStateEntry> subtask);
        void Add(ICompoundTask<TWorldStateEntry> parent, Slot<TWorldStateEntry> slot);
    }
}
