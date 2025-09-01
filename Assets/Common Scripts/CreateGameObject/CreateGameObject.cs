#nullable enable
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Common2D.CreateGameObject2D{
    public class CreateGameObject
    {
        public static GameObject CreateObjWithTypeComponent<T>
        (
            Vector3 position,
            Transform? parent
        ) where T : Component
        {
            GameObject obj;
            obj = new GameObject("New Game Object");
            obj.transform.position = position;
            obj.AddComponent<T>();
            if (parent != null)
                obj.transform.SetParent(parent);

            return obj;
        }

        public static TextMeshPro CreateTextMeshPro
        (
            string text,
            Vector3 position,
            Transform? parentForText,
            Color color,
            int sortingOrder
        )
        {
            Transform? parent = null;
            if (parentForText == null)
            {
                GameObject parentTextDebug = GameObject.Find("Text Game Object");
                if (parentTextDebug == null)
                {
                    parentTextDebug = new GameObject("Text Game Object");
                    parentTextDebug.transform.position = Vector3.zero;
                }
                parent = parentTextDebug.transform;
            }
            else
            {
                parent = parentForText;
            }

            TextMeshPro textMeshPro = CreateObjWithTypeComponent<TextMeshPro>
                                    (position, parent)
                                        .GetComponent<TextMeshPro>();

            textMeshPro.transform.position = position;
            textMeshPro.text = text;
            textMeshPro.fontSize = 1.88f;
            // Change color text to black
            textMeshPro.color = color;

            textMeshPro.alignment = TextAlignmentOptions.Center;
            textMeshPro.sortingOrder = sortingOrder;
            return textMeshPro;
        }

        public static SpriteRenderer CreateSpriteRenderer
        (
            Vector3 position,
            Color color,
            int sortingOrder,
            Vector3 localScale,
            Action<GameObject>? func
        )
        {
            GameObject parentSpriteDebug = GameObject.Find("Sprite Game Object");
            if (parentSpriteDebug == null)
            {
                parentSpriteDebug = new GameObject("Sprite Game Object");
                parentSpriteDebug.transform.position = Vector3.zero;
            }
            ;

            SpriteRenderer spriteRenderer = CreateObjWithTypeComponent<SpriteRenderer>
                                    (position, parentSpriteDebug.transform)
                                        .GetComponent<SpriteRenderer>();
            spriteRenderer.gameObject.transform.localScale = localScale;
            spriteRenderer.color = color;
            spriteRenderer.sortingOrder = sortingOrder;

            spriteRenderer.sprite = ResourcesManager.GetSpriteDefault();
            func?.Invoke(spriteRenderer.gameObject);
            return spriteRenderer;
        }

        public static void CreateCountdown(
            float countdownDuration,
            Vector3 position,
            CountdownOptions countdownOptions,
            Action<TextMeshPro>? onFinish = null,
            Transform? transformParent = null,
            string stringDefaultWhenFinish = "<Finish>",
            Action<TextMeshPro>? onCustomTextMeshPro = null           
        )
        {
            TextMeshPro countdownText = CreateObjWithTypeComponent<TextMeshPro>
                                        (position, transformParent)
                                            .GetComponent<TextMeshPro>();
            countdownText.color = Color.white;
            countdownText.fontSize = 1.88f;
            countdownText.alignment = TextAlignmentOptions.Center;
            countdownText.gameObject.name = "Countdown Timer";

            MonoBehaviour monoBehaviour = MonoBehaviour.FindAnyObjectByType<MonoBehaviour>();
            if (monoBehaviour != null)
            {
                Coroutine countdownCoroutine = monoBehaviour.StartCoroutine(
                    ShowCountdown(
                        countdownDuration,
                        countdownText,
                        stringDefaultWhenFinish,
                        countdownOptions,
                        onFinish
                ));
                onCustomTextMeshPro?.Invoke(countdownText);
            }
        }

        private static IEnumerator ShowCountdown(
            float countdownDuration,
            TextMeshPro countdownText,
            string defaultWhenFinish,
            CountdownOptions countdownOptions,
            Action<TextMeshPro>? onFinish
        )
        {
            float startTime = Time.time;
            float endTime = startTime + countdownDuration;

            while (Time.time < endTime)
            {
                float remainingTime = endTime - Time.time;
                countdownText.text = TimeFormatter.FormatCountdown(remainingTime, countdownOptions);
                yield return null;
            }
            countdownText.text = defaultWhenFinish;
            yield return new WaitForSeconds(0.25f);

            if (onFinish != null)
            {
                onFinish(countdownText);
            }
        }


        public static TextMeshPro CreateTextPopup(string text, Vector2 position, Color color, Transform? prefabPopupText = null)
        {
            if (ObjectPooler.IsObjectPoolerExist(KeyOfObjPooler.PopupText.ToString()))
            {
                TextMeshPro textPopup = ObjectPooler.GetObject<PopupText>(KeyOfObjPooler.PopupText.ToString(), false).GetComponent<TextMeshPro>();
                Transform transformPopup = textPopup.transform;
                transformPopup.position = new Vector3(position.x, position.y, 0);
                textPopup.text = text;
                textPopup.color = color;
                transformPopup.gameObject.SetActive(true);
                return textPopup;
            }

            TextMeshPro newTextPopup = MonoBehaviour
                .Instantiate(ResourcesManager.GetPopupTextPrefab(), position, Quaternion.identity)
                .GetComponent<TextMeshPro>();

            newTextPopup.text = text;
            newTextPopup.color = color;
            newTextPopup.gameObject.SetActive(true);
            return newTextPopup;
        }
    }
}