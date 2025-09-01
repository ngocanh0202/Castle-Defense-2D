#nullable enable
using System;
using Common2D.CreateGameObject2D;
using UnityEngine;

namespace Common2D{
    namespace EventMouse2D{
        public class EventMouse2D : MonoBehaviour
        {
            public static Vector3 GetPositionOnMouse()
            {
                Vector3 mouseScreenPosition = Input.mousePosition;
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.nearClipPlane));
                return mouseWorldPosition;
            }

            public static Vector2 GetPositionOnMouse2D()
            {
                Vector3 mouseScreenPosition = Input.mousePosition;
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.nearClipPlane));
                return new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);
            }

            public static GameObject CreateGameObjectOnMouse(GameObject? gameObject)
            {
                if (gameObject == null)
                {
                    gameObject = CreateGameObject.CreateSpriteRenderer(GetPositionOnMouse(), Color.white, 3, new Vector3(0.1f, 0.1f, 0.1f), null).gameObject;
                }
                else
                {
                    gameObject.transform.position = GetPositionOnMouse();
                }
                return gameObject;
            }

            private static bool IsEventMouseOccurred(EventMouseType eventType)
            {
                switch (eventType)
                {
                    case EventMouseType.LeftClick:
                        return Input.GetMouseButtonDown(0);
                    case EventMouseType.RightClick:
                        return Input.GetMouseButtonDown(1);
                    case EventMouseType.MiddleClick:
                        return Input.GetMouseButtonDown(2);
                    case EventMouseType.LeftHold:
                        return Input.GetMouseButton(0);
                    case EventMouseType.RightHold:
                        return Input.GetMouseButton(1);
                    case EventMouseType.MiddleHold:
                        return Input.GetMouseButton(2);
                    case EventMouseType.LeftRelease:
                        return Input.GetMouseButtonUp(0);
                    case EventMouseType.RightRelease:
                        return Input.GetMouseButtonUp(1);
                    case EventMouseType.MiddleRelease:
                        return Input.GetMouseButtonUp(2);
                    default:
                        return false;
                }
            }

            public static GameObject? CreateGameObjectOnMouseWithBehivor
            (
                GameObject? gameObjectOnMoues,
                GameObject prefab,
                Transform? transformParent,
                EventMouseType eventTypeToCreate,
                EventMouseType eventTypeToRelease
            )
            {
                if (gameObjectOnMoues != null)
                {
                    gameObjectOnMoues.transform.position = GetPositionOnMouse();
                    if (transformParent != null)
                        if (!transformParent.Find(gameObjectOnMoues.name))
                        {
                            gameObjectOnMoues.transform.SetParent(transformParent);
                        }
                    if (IsEventMouseOccurred(eventTypeToRelease))
                    {
                        GameObject objSetPosition = Instantiate(gameObjectOnMoues, gameObjectOnMoues.transform.position, Quaternion.identity);
                        objSetPosition.transform.SetParent(transformParent);
                        Destroy(gameObjectOnMoues);
                        return null;
                    }
                }
                else
                {
                    if (IsEventMouseOccurred(eventTypeToCreate))
                    {
                        if (prefab == null)
                        {
                            gameObjectOnMoues = CreateGameObject.CreateSpriteRenderer(GetPositionOnMouse(), Color.white, 3, new Vector3(1f, 1f, 1f), null).gameObject;
                            gameObjectOnMoues.transform.position = GetPositionOnMouse();
                        }
                        else
                        {
                            gameObjectOnMoues = Instantiate(prefab, GetPositionOnMouse(), Quaternion.identity);
                        }
                        if (transformParent != null)
                            gameObjectOnMoues.transform.SetParent(transformParent);
                    }
                }
                return gameObjectOnMoues;
            }

            public static void LookAtMouse2D(Transform transform, float speed = 1f)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = mouseWorldPos - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            }
        }
    }
 
}