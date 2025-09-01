using System.Collections;
using System.Collections.Generic;
using Common2D.Singleton.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common2D.Singleton
{
    public class NotificationSystem : Singleton<NotificationSystem>
    {
        [Header("Notification Settings")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject notificationPrefab;
        [SerializeField] private RectTransform notificationParent;
        [SerializeField] private float displayDuration = 1.5f;
        [SerializeField] private float fadeInDuration = 0.1f;
        [SerializeField] private float fadeOutDuration = 0.1f;
        [SerializeField] private float messageInterval = 0f;
        [SerializeField] private Vector2 startPosition = Vector2.zero;
        [SerializeField] private Vector2 endPosition = Vector2.zero;
        [SerializeField] private int maxQueueSize = 3;
        private Queue<NotificationInfo> notificationQueue = new Queue<NotificationInfo>();
        private bool isShowingNotification = false;
        [Header("Set color background and color text for notification")]
        [SerializeField] Color colorPrimary = new Color(0.2f, 0.2f, 0.8f, 0.8f);
        [SerializeField] Color colorSuccess = new Color(0.2f, 0.8f, 0.2f, 0.8f);
        [SerializeField] Color colorWarning = new Color(0.8f, 0.8f, 0.2f, 0.8f);
        [SerializeField] Color colorError = new Color(0.8f, 0.2f, 0.2f, 0.8f);
        protected override void Awake()
        {
            base.Awake();
            if (notificationParent == null)
            {
                // Find Canvas have render mode screen
                canvas = GameObject.Find(StringDefault.Canvas.ToString()).GetComponent<Canvas>();
                if (canvas == null)
                {
                    GameObject canvasObj = new GameObject(StringDefault.Canvas.ToString());
                    canvas = canvasObj.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvas.sortingOrder = 0;
                    canvasObj.AddComponent<CanvasScaler>();
                    canvasObj.AddComponent<GraphicRaycaster>();
                }

                GameObject parentObj = new GameObject("NotificationParent");
                float widthCanvas = canvas.GetComponent<RectTransform>().rect.width;
                float heighCanvas = canvas.GetComponent<RectTransform>().rect.height;
                parentObj.transform.SetParent(canvas.transform, false);
                notificationParent = parentObj.AddComponent<RectTransform>();
                notificationParent.anchoredPosition = Vector2.zero;
                notificationParent.sizeDelta = new Vector2(widthCanvas, heighCanvas);
                startPosition = new Vector2((widthCanvas / 2) + 100f, (heighCanvas / 2) - 100f);
                endPosition = new Vector2((widthCanvas / 2) - 275f, (heighCanvas / 2) - 100f);
            }
        }

        public void ShowNotification(string message,
            NotificationType type = NotificationType.Info,
            float duration = -1)
        {
            if (string.IsNullOrEmpty(message))
                return;

            if (notificationQueue.Count >= maxQueueSize)
            {
                notificationQueue.Dequeue();
            }

            if (duration < 0)
                duration = displayDuration;

            notificationQueue.Enqueue(new NotificationInfo(message, type, duration));

            if (!isShowingNotification)
            {
                StartCoroutine(ProcessNotificationQueue());
            }
        }

        private IEnumerator ProcessNotificationQueue()
        {
            isShowingNotification = true;

            while (notificationQueue.Count > 0)
            {
                NotificationInfo notificationInfo = notificationQueue.Dequeue();
                yield return StartCoroutine(ShowSingleNotification(notificationInfo));

                yield return new WaitForSeconds(messageInterval);
            }

            isShowingNotification = false;
        }

        private IEnumerator ShowSingleNotification(NotificationInfo notificationInfo)
        {
            GameObject notificationObj = null;

            if (notificationPrefab != null)
            {
                notificationObj = Instantiate(notificationPrefab, notificationParent);
            }
            else
            {
                notificationObj = Instantiate(CreateDefaultNotification(), notificationParent);
            }

            RectTransform rectTransform = notificationObj.GetComponent<RectTransform>();
            CanvasGroup canvasGroup = notificationObj.GetComponent<CanvasGroup>();
            TextMeshProUGUI messageText = notificationObj.GetComponentInChildren<TextMeshProUGUI>();
            Image backgroundImage = notificationObj.GetComponent<Image>();
            rectTransform.anchoredPosition = startPosition;
            canvasGroup.alpha = 0;

            if (messageText != null)
            {
                messageText.text = notificationInfo.message;
            }

            if (backgroundImage != null)
            {
                switch (notificationInfo.type)
                {
                    case NotificationType.Info:
                        backgroundImage.color = colorPrimary;
                        break;
                    case NotificationType.Success:
                        backgroundImage.color = colorSuccess;
                        break;
                    case NotificationType.Warning:
                        backgroundImage.color = colorWarning;
                        break;
                    case NotificationType.Error:
                        backgroundImage.color = colorError;
                        break;
                }
            }

            float fadeInTimer = 0;
            while (fadeInTimer < fadeInDuration)
            {
                fadeInTimer += Time.deltaTime;
                float t = fadeInTimer / fadeInDuration;
                canvasGroup.alpha = Mathf.Lerp(0, 1, t);
                rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            yield return new WaitForSeconds(notificationInfo.duration);

            float fadeOutTimer = 0;
            while (fadeOutTimer < fadeOutDuration)
            {
                fadeOutTimer += Time.deltaTime;
                float t = fadeOutTimer / fadeOutDuration;
                canvasGroup.alpha = Mathf.Lerp(1, 0, t);
                yield return null;
            }

            Destroy(notificationObj);
        }


        private GameObject CreateDefaultNotification()
        {
            return ResourcesManager.GetDefaultNotification();
        }

        public void ShowInfo(string message, float duration = -1)
        {
            ShowNotification(message, NotificationType.Info, duration);
        }

        public void ShowSuccess(string message, float duration = -1)
        {
            ShowNotification(message, NotificationType.Success, duration);
        }

        public void ShowWarning(string message, float duration = -1)
        {
            ShowNotification(message, NotificationType.Warning, duration);
        }

        public void ShowError(string message, float duration = -1)
        {
            ShowNotification(message, NotificationType.Error, duration);
        }

        public void ClearNotifications()
        {
            notificationQueue.Clear();
        }
    }
}