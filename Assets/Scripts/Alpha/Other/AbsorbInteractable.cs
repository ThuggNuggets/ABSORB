using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbInteractable : MonoBehaviour
{
    [Header("Refereneces")]
    public BoxCollider boxCollider;
    public Animator animator;
    public Light lightComponent;
    public LightFlicker flicker;
    public GameObject particleEffect;


    [Header("Properties")]
    public float particleEffectTime = 3.0f;

    public void Activate()
    {
        particleEffect.SetActive(true);
        lightComponent.intensity = 0;
        animator.SetBool("DoorTriggered", true);
        boxCollider.enabled = false;
        flicker.Disable();
        StartCoroutine(ParticleEffectTime());
    }

    private IEnumerator ParticleEffectTime()
    {
        yield return new WaitForSeconds(particleEffectTime);
        particleEffect.SetActive(false);
    }
}
