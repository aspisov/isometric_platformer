using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : BaseState
{
    public override void EnterState(PlayerController player) {
        player.animator.Play("idle");
    }
    
    public override void UpdateState(PlayerController player) {
        if (!player.isColliding) {
            player.SwitchState(player.fallState);
        }

        else if (player.jumpIsPressed) {
            player.SwitchState(player.jumpState);
            player.jumpIsPressed = false;
        }

        else if (player.moveDirection != Vector2.zero) {
            player.SwitchState(player.runState);
        }

        player.MovementLogic();
    }
}