using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun
{
    public partial class GameNotifications
    {
        public GameNotificationsType Type { get; set; }
        public ItemId Id { get; set; }
        public int Val { get; set; }

    }
}