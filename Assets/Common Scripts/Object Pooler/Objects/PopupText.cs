using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PopupText : CommonPoolObject
{
    public float moveUp = 1f;
    public float duration = 0.5f;
    public float timeExistWhenEndPos = 0.001f;
    Coroutine moveCoroutine;
    public override string PoolName { get => KeyOfObjPooler.PopupText.ToString(); set => throw new NotImplementedException(); }

    void Awake()
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

    override protected void OnDisable()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        base.OnDisable();
    }
}