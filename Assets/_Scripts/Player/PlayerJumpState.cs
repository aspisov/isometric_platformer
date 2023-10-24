using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : BaseState
{
    public override void EnterState(PlayerController player) {
        player.animator.Play("jump");
    }
    
    public override void UpdateState(PlayerController player) {
        player.rb.position += player.moveDirection * player.velocity * Time.fixedDeltaTime;
    }
}