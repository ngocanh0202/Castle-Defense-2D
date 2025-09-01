using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void EnterState();
    void UpdateState();
    void FixUpdateState();
    void OnCollision2D(Collision2D collision);
    void OnTriggerEnter2D(Collider2D collision);
}

public interface IDrawGizmos
{
    void OnDrawGizmos();
}