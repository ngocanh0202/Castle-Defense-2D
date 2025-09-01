namespace Common2D.Singleton.Models
{
    public class NotificationInfo
    {
        public string message;
        public NotificationType type;
        public float duration;

        public NotificationInfo(string message, NotificationType type, float duration = -1)
        {
            this.message = message;
            this.type = type;
            this.duration = duration;
        }
    }
}

