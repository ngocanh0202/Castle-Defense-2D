using System;
using System.Collections;
using Common2D.Singleton.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common2D.Singleton
{
    public class ConfirmModalSystem : Singleton<ConfirmModalSystem>
    {
        [Header("Confirm Modal Settings")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject confirmModalPrefab;
        [SerializeField] private RectTransform confirmModalParent;
        [SerializeField] private float displayDuration = 0.5f;
        [SerializeField] private float fadeInDuration = 0.30f;
        [SerializeField] private float fadeOutDuration = 0.25f;
        [SerializeField] private Vector2 startPosition = Vector2.zero;
        [SerializeField] private Vector2 endPosition = Vector2.zero;
        [SerializeField] private bool isShowingConfirmModal;
        protected override void Awake()
        {
            base.Awake();
            if (confirmModalParent == null)
            {
                canvas = GameObject.Find(StringDefault.Canvas.ToString()).GetComponent<Canvas>();
                if (canvas == null)
                {
                    GameObject canvasObj = new GameObject(StringDefault.Canvas.ToString());
                    canvas = canvasObj.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvas.sortingOrder = 100;
                    canvasObj.AddComponent<CanvasScaler>();
                    canvasObj.AddComponent<GraphicRaycaster>();
                }

                GameObject parentObj = new GameObject("ConfirmModalParent");
                float widthCanvas = canvas.GetComponent<RectTransform>().rect.width;
                float heighCanvas = canvas.GetComponent<RectTransform>().rect.height;
                parentObj.transform.SetParent(canvas.transform, false);
                confirmModalParent = parentObj.AddComponent<RectTransform>();
                confirmModalParent.anchoredPosition = Vector2.zero;
                confirmModalParent.sizeDelta = new Vector2(widthCanvas, heighCanvas);
                startPosition = new Vector2((widthCanvas / 2) + 100f, (heighCanvas / 2) - 200f);
                endPosition = new Vector2((widthCanvas / 2) - 375f, (heighCanvas / 2) - 200f);
                parentObj.AddComponent<CanvasGroup>().blocksRaycasts = true;
                parentObj.GetComponent<CanvasGroup>().interactable  = true;
            }
        }

        public void ShowConfirmModal(string message,
            float duration = -1,
            Action OnListenerClickAccept = null)
        {
            if (string.IsNullOrEmpty(message))
                return;

            if (duration < 0)
                duration = displayDuration;

            if (!isShowingConfirmModal)
            {
                isShowingConfirmModal = true;
                ConfirmModalInfor newConfirmModalInfor = new ConfirmModalInfor(message, duration, OnListenerClickAccept);
                StartCoroutine(HandleShowModalConfirm(newConfirmModalInfor));
            }

        }

        private IEnumerator HandleShowModalConfirm(ConfirmModalInfor confirmModalInfor)
        {
            GameObject confirmModalObj = null;

            if (confirmModalPrefab != null)
            {
                confirmModalObj = Instantiate(confirmModalPrefab, confirmModalParent);
            }
            else
            {
                confirmModalObj = Instantiate(GetDefaultConfirmModal(), confirmModalParent);
            }

            RectTransform rectTransform = confirmModalObj.GetComponent<RectTransform>();
            CanvasGroup canvasGroup = confirmModalObj.GetComponent<CanvasGroup>();

            TextMeshProUGUI messageText = confirmModalObj.transform.Find("Message").GetComponent<TextMeshProUGUI>();

            Button acceptButton = confirmModalObj.transform.Find("AcceptButton").GetComponent<Button>();
            Button cancelButton = confirmModalObj.transform.Find("CancelButton").GetComponent<Button>();

            rectTransform.anchoredPosition = startPosition;
            canvasGroup.alpha = 0;

            if (messageText != null)
            {
                messageText.text = confirmModalInfor.message;
            }

            bool userResponded = false;
            bool userAccepted = false;

            if (acceptButton != null)
            {
                acceptButton.onClick.AddListener(() =>
                {
                    userResponded = true;
                    userAccepted = true;
                });
            }

            if (cancelButton != null)
            {
                cancelButton.onClick.AddListener(() =>
                {
                    userResponded = true;
                    userAccepted = false;
                });
            }

            // Fade in animation
            float fadeInTimer = 0;
            while (fadeInTimer < fadeInDuration)
            {
                fadeInTimer += Time.deltaTime;
                float t = fadeInTimer / fadeInDuration;
                canvasGroup.alpha = Mathf.Lerp(0, 1, t);
                rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            float waitTimer = 0;
            while (!userResponded && (confirmModalInfor.duration < 0 || waitTimer < confirmModalInfor.duration))
            {
                waitTimer += Time.deltaTime;
                yield return null;
            }

            isShowingConfirmModal = false;
            if (userAccepted && confirmModalInfor.onAccept != null)
            {
                confirmModalInfor.onAccept();
            }

            float fadeOutTimer = 0;
            while (fadeOutTimer < fadeOutDuration)
            {
                fadeOutTimer += Time.deltaTime;
                float t = fadeOutTimer / fadeOutDuration;
                canvasGroup.alpha = Mathf.Lerp(1, 0, t);
                yield return null;
            }
            Destroy(confirmModalObj);
        }

        private GameObject GetDefaultConfirmModal()
        {
            return ResourcesManager.GetDefaultConfirmModal();
        }
    }
}