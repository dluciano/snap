using Snap.Entities.Enums;

namespace Snap.Entities
{
    public static class EntitiesExtensions
    {
        public static byte GetCardValue(this Card card) =>
            (byte)((byte)((byte)card << 4) >> 4);

        public static byte GetCardType(this Card card) =>
            (byte)((byte)card >> 4);
    }
}
