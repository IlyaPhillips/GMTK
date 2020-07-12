using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{
    private float speedIncrease = 5.0F;
    private float duration = 2.0F;

    public override IEnumerator usePowerUp(PlayerMovement pm)
    {
        pm.speed += speedIncrease;
        Debug.Log("speed increased");
        yield return new WaitForSeconds(duration);
        pm.speed -= speedIncrease;
        Debug.Log("speed decreased");
    }

}
