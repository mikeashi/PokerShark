using FluidHTN;
using FluidHTN.Compounds;

namespace PokerShark.AI.HTN.Tasks.CompoundTasks
{
    internal class ExpectedUtilitySelector : Selector<object>
    {
        protected override DecompositionStatus OnDecompose(IContext<object> ctx, int startIndex, out Queue<ITask<object>> result)
        {
            Plan.Clear();

            var task = FindBestTask(ctx, startIndex, out var taskIndex);
            if (task == null)
            {
                result = Plan;
                return DecompositionStatus.Failed;
            }

            return OnDecomposeTask(ctx, task, taskIndex, null, out result);
        }

        protected virtual ITask<object>? FindBestTask(IContext<object> ctx, int startIndex, out int bestIndex)
        {
            var bestScore = double.MinValue;
            bestIndex = -1;
            ITask<object>? bestTask = null;

            for (var taskIndex = startIndex; taskIndex < Subtasks.Count; taskIndex++)
            {
                var task = Subtasks[taskIndex];
                if (task is VariableUtilityTask utilityTask)
                {
                    if (utilityTask.IsValid(ctx) == false)
                        continue;

                    var score = utilityTask.GetExpectedUtility(ctx);
                    if (bestScore < score)
                    {
                        bestScore = score;
                        bestTask = task;
                        bestIndex = taskIndex;
                    }
                }
            }

            return bestTask;
        }

    }
}
