using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbInteractable : MonoBehaviour
{
    public BoxCollider boxCollider;
    public Animator animator;
    public Light lightComponent;
    public LightFlicker flicker;

    public void Activate()
    {
        lightComponent.intensity = 0;
        animator.SetBool("DoorTriggered", true);
        boxCollider.enabled = false;
        flicker.Disable();
    }
}
