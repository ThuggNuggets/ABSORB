using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPot : Ability
{
    [Header("References")]
    public GameObject potAim;
    public GameObject potOrbObject;
    public ParticleSystem waterHit;
    public Animator playerAnimator;
    public Camera mainCamera;

    private AudioSource _waterHitAudio;
    private Animator _orbAnimator;
    private bool _aimActive = false;
    private Trigger _impactTrigger;

    private void Awake()
    {
        _impactTrigger = potAim.GetComponent<Trigger>();
        _orbAnimator = potAim.GetComponent<Animator>();
        _waterHitAudio = waterHit.GetComponent<AudioSource>();
    }

    public override void OnEnter()
    {
        _aimActive = true;
        potAim.SetActive(true);
    }

    public override void OnExit()
    {
        _aimActive = false;
    }

    public override void Activate()
    {
        _aimActive = false;
        potOrbObject.SetActive(true);
        playerAnimator.SetBool("Pot", true);
        _orbAnimator.SetBool("Attack", true);
    }

    public void Key_ActivateOrbHitVFX()
    {
        waterHit.Play();
        _waterHitAudio.Play();
        potOrbObject.SetActive(false);
        StartCoroutine(DeactivateAim());
    }

    public void Key_DeactivatePotAbility()
    {
        playerAnimator.SetBool("Pot", false);
        _orbAnimator.SetBool("Attack", false);
        abilityHandler.SetAbility(AbilityHandler.AbilityType.NONE);
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

    private IEnumerator DeactivateAim()
    {
        yield return new WaitForSeconds(1.0F);
        potAim.SetActive(false);
    }
}
