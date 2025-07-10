using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private SpriteRenderer spriteRenderer;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private float moveRange;
    public EnemyIdleState(EnemyStateMachine stateMachine, Animator animator) : base(stateMachine, animator)
    {
        spriteRenderer = stateMachine.GetComponent<SpriteRenderer>();
        originalPosition = stateMachine.transform.position;
        moveRange = 1.25f; 
    }

    public override void EnterState()
    {

        Debug.Log("Entering Idle State");
        StateMachine.StartCoroutine(IdleColorChangeLoop());
        targetPosition = originalPosition;
    }

    public override void FixUpdateState()
    {

    }

    public override void OnCollision2D(Collision2D collision)
    {

    }

    public override void UpdateState()
    {
        // make this enemy move randomly from original position + 5f
        if (StateMachine.CurrentState != this)
        {
            return;
        }
        if (targetPosition != StateMachine.transform.position)
        {

            StateMachine.transform.position = Vector2.MoveTowards(
                StateMachine.transform.position,
                targetPosition,
                Time.deltaTime * 1f 
            );
        }
        else
        {
            Vector2 randomPosition = new Vector2(
                originalPosition.x + Random.Range(-moveRange, moveRange),
                originalPosition.y + Random.Range(-moveRange, moveRange)
            );
            targetPosition = randomPosition;
        }

    }

    IEnumerator IdleColorChangeLoop()
    {
        while (StateMachine.CurrentState == this)
        {
            Color originalColor = spriteRenderer.color;
            Color targetColor = GetRandomBlueGreenWhiteColor();
            float duration = 1f;
            yield return StateMachine.StartCoroutine(StateMachine.ChangeColorCoroutine(originalColor, targetColor, duration));
            yield return new WaitForSeconds(0.25f);
        }
    }

    Color GetRandomBlueGreenWhiteColor()
    {
        int colorChoice = Random.Range(0, 3);
        
        switch (colorChoice)
        {
            case 0: 
                return new Color(0f, Random.Range(0.2f, 0.8f), Random.Range(0.7f, 1f), 1f);
            case 1: 
                return new Color(0f, Random.Range(0.7f, 1f), Random.Range(0.2f, 0.8f), 1f);
            case 2:
                float whiteIntensity = Random.Range(0.8f, 1f);
                return new Color(whiteIntensity, whiteIntensity, whiteIntensity, 1f);
            default:
                return Color.white;
        }
    }
}
