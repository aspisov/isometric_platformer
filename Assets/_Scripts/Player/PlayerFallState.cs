using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : BaseState
{
    private float timer = 0;

    public override void EnterState(PlayerController player) {
        player.animator.Play("idle");
        player.sprite.sortingOrder = -2;
    }

    public override void UpdateState(PlayerController player) {
        timer += Time.fixedDeltaTime;
        player.transform.position -= new Vector3(0, 3f, 0) * Time.deltaTime;
        if (timer >= 0.5)
        {
            player.Reset();
            timer = 0;
        }
    }
}