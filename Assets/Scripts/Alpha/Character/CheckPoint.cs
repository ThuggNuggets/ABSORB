﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Trigger triggerBox;
    public Transform spawnPosition;

    private PlayerHandler _playerHandler;

    // Start is called before the first frame update
    void Start()
    {
        _playerHandler = FindObjectOfType<PlayerHandler>();
    }

    void Update()
    {
        if (triggerBox.Enabled && triggerBox.Collider.CompareTag("Player"))
        {
            Debug.Log("Player walked through trigger");
            _playerHandler.SetRespawnPosition(this.spawnPosition.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(this.spawnPosition.position, 1f);
    }
}
