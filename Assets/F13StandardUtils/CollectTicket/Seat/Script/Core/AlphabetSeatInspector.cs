namespace F13StandardUtils.CollectTicket.Seat.Script.Core
{
    public class AlphabetSeatInspector: BaseSeatModifierInspector
    {
        public override bool InGroup(BaseSeatModifier m)
        {
            return m is AlphabetSeatModifier;
        }
    }
}