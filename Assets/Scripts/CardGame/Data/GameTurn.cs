using CardGameShared.Data;

namespace CardGame.Data
{
    public struct GameTurn
    {
        public int damageDealt { get; set; }
        public int damageReceived { get; set; }
        public ActionType action { get; set; }
        public ActionType actionRecieved { get; set; }
    }
}