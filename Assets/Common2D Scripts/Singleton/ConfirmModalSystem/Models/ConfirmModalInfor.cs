using System;

namespace Common2D.Singleton.Models
{
    public class ConfirmModalInfor
    {
        public string message;
        public float duration;
        public Action onAccept;
        public ConfirmModalInfor(string message, float duration = -1, Action onAccept = null)
        {
            this.message = message;
            this.duration = duration;
            this.onAccept = onAccept;
        }
    }
}
