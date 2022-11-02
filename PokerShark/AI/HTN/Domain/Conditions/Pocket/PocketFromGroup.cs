using FluidHTN;
using FluidHTN.Conditions;
using PokerShark.Poker.Deck;

namespace PokerShark.AI.HTN.Domain.Conditions.Pockt
{
    internal class PocketFromGroup : ICondition<Object>
    {
        public string Name { get; } = "If pocket belongs to one of the given groups";
        public List<int> GroupsQuery { get; set; }

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                return GroupsQuery.Contains((new PocketGroups()).GetStrength(c.GetPocket()));
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
