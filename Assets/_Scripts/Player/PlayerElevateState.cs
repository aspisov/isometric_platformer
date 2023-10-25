using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElevateState : BaseState
{
    private float timer = 0;

    public override void EnterState(PlayerController player) {
        player.animator.Play("idle");
    }

    public override void UpdateState(PlayerController player) {
    }
}
