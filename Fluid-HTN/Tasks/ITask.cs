﻿using System.Collections.Generic;
using FluidHTN.Compounds;
using FluidHTN.Conditions;

namespace FluidHTN
{
    public interface ITask<TWorldStateEntry>
    {
        /// <summary>
        ///     Used for debugging and identification purposes
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     The parent of this task in the hierarchy
        /// </summary>
        ICompoundTask<TWorldStateEntry> Parent { get; set; }

        /// <summary>
        ///     The conditions that must be satisfied for this task to pass as valid.
        /// </summary>
        List<ICondition<TWorldStateEntry>> Conditions { get; }

        /// <summary>
        ///     Add a new condition to the task.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        ITask<TWorldStateEntry> AddCondition(ICondition<TWorldStateEntry> condition);

        /// <summary>
        ///     Check the task's preconditions, returns true if all preconditions are valid.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        bool IsValid(IContext<TWorldStateEntry> ctx);

        DecompositionStatus OnIsValidFailed(IContext<TWorldStateEntry> ctx);
    }
}
