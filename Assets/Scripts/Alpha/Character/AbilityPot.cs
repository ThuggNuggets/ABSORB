using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPot : Ability
{
    [Header("References")]
    public GameObject potAim;
    public GameObject potOrbObject;
    public ParticleSystem waterHit;
    public AudioSource waterHitAudio;
    public Animator orbAnimator;
    public Animator playerAnimator;
    public Camera mainCamera;
    private bool _aimActive = false;

    public override void OnEnter()
    {
        _aimActive = true;
        potAim.SetActive(true);
    }

    public override void OnExit()
    {
        _aimActive = false;
        potAim.SetActive(false);
    }

    public override void Activate()
    {
        _aimActive = false;
        potOrbObject.SetActive(true);
        playerAnimator.SetBool("Pot", true);
        orbAnimator.SetBool("Attack", true);
    }

    public void Key_ActivateOrbHitVFX()
    {
        waterHit.Play();

    }

    public void Key_DeactivatePotAbility()
    {
        playerAnimator.SetBool("Pot", false);
    }

    public void Update()
    {
        if (_aimActive)
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f)), out hit, 1000.0f))
            {
                potAim.transform.position = hit.point;
                potAim.transform.up = hit.normal;
            }
        }
    }
}
