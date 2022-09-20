namespace F13StandardUtils.CollectTicket.Seat.Script.Core
{
    public class NumberSeatInspector: BaseSeatModifierInspector
    {
        public override bool InGroup(BaseSeatModifier m)
        {
            return m is NumberSeatModifier;
        }
    }
}