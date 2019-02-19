namespace Snap.Entities
{
    public enum Card : byte
    {
        //CLOVES 0b00_xxxx
        A_CLOVE = 0b00_0000,
        TWO_CLOVE = 0b00_0001,
        THREE_CLOVE = 0b00_0010,
        FOUR_CLOVE = 0b00_0011,
        FIVE_CLOVE = 0b00_0100,
        SIX_CLOVE = 0b00_0101,
        SEVEN_CLOVE = 0b00_0110,
        EIGHT_CLOVE = 0b00_0111,
        NINE_CLOVE = 0b00_1000,
        TEN_CLOVE = 0b00_1001,
        JOCKER_CLOVE = 0b00_1010,
        QUEEN_CLOVE = 0b00_1011,
        KING_CLOVE = 0b00_1100,

        //TILES 0b01_xxxx
        A_TILE = 0b01_0000,
        TWO_TILE = 0b01_0001,
        THREE_TILE = 0b01_0010,
        FOUR_TILE = 0b01_0011,
        FIVE_TILE = 0b01_0100,
        SIX_TILE = 0b01_0101,
        SEVEN_TILE = 0b01_0110,
        EIGHT_TILE = 0b01_0111,
        NINE_TILE = 0b01_1000,
        TEN_TILE = 0b01_1001,
        JOCKER_TILE = 0b01_1010,
        QUEEN_TILE = 0b01_1011,
        KING_TILE = 0b01_1100,

        //HEART  0b10_xxxx
        A_HEART = 0b10_0000,
        TWO_HEART = 0b10_0001,
        THREE_HEART = 0b10_0010,
        FOUR_HEART = 0b10_0011,
        FIVE_HEART = 0b10_0100,
        SIX_HEART = 0b10_0101,
        SEVEN_HEART = 0b10_0110,
        EIGHT_HEART = 0b10_0111,
        NINE_HEART = 0b10_1000,
        TEN_HEART = 0b10_1001,
        JOCKER_HEART = 0b10_1010,
        QUEEN_HEART = 0b10_1011,
        KING_HEART = 0b10_1100,

        //PIKES  0b11_xxxx
        A_PIKE = 0b11_0000,
        TWO_PIKE = 0b11_0001,
        THREE_PIKE = 0b11_0010,
        FOUR_PIKE = 0b11_0011,
        FIVE_PIKE = 0b11_0100,
        SIX_PIKE = 0b11_0101,
        SEVEN_PIKE = 0b11_0110,
        EIGHT_PIKE = 0b11_0111,
        NINE_PIKE = 0b11_1000,
        TEN_PIKE = 0b11_1001,
        JOCKER_PIKE = 0b11_1010,
        QUEEN_PIKE = 0b11_1011,
        KING_PIKE = 0b11_1100
    }
}