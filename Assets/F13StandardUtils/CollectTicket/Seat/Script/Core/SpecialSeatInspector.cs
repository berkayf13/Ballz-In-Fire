namespace F13StandardUtils.CollectTicket.Seat.Script.Core
{
    public class SpecialSeatInspector: BaseSeatModifierInspector
    {
        public override bool InGroup(BaseSeatModifier m)
        {
            return m is SpecialSeatModifier;
        }
    }
}