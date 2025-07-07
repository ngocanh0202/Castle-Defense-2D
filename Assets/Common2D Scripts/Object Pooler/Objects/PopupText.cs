using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PopupText : MonoBehaviour, IPooler
{
    public event EventHandler<PoolerEventArgs> OnSetInactive;
    public float moveUp = 1f;
    public float duration = 0.5f;
    public float timeExistWhenEndPos = 0.001f;

    Coroutine moveCoroutine;

    void Start()
    {
        MoveToUpPosition2();
    }

    void OnEnable()
    {
        MoveToUpPosition2();
    }

    void MoveToUpPosition2()
    {
        transform.DOKill();
        transform.DOLocalMoveY(
            transform.localPosition.y + 0.5f,
            0.5f
        ).OnComplete(() =>
        {
            if (ObjectPooler.IsObjectPoolerExist(KeyOfObjPooler.PopupText.ToString()))
            {
                gameObject.SetActive(false);
            } 
            else
            {
                Destroy(gameObject);
            }
            
        });
    }


    void MoveToUpPosition()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveUpCoroutine());
    }



    IEnumerator MoveUpCoroutine()
    {
        Vector3 startPos = transform.localPosition;
        Vector3 endPos = startPos + Vector3.up * moveUp;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        transform.localPosition = endPos;
        yield return null;
        if (ObjectPooler.IsObjectPoolerExist(KeyOfObjPooler.PopupText.ToString()))
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDisable()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        OnSetInactive?.Invoke(this, new PoolerEventArgs { key = KeyOfObjPooler.PopupText.ToString() });
    }
}