using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Shared;


/**
 * Player status overlay controller. Shows the remaining health, shield
 * and munitions of the player.
 */
public class PlayerStatusView : MonoBehaviour {

    /** Container for health and shield indicators */
    [SerializeField] private Transform healthContainer = null;

    /** Container for munition indicators */
    [SerializeField] private Transform munitionContainer = null;

    /** Health indicator template */
    [SerializeField] private GameObject healthPrefab = null;

    /** Shield indicator template */
    [SerializeField] private GameObject shieldPrefab = null;

    /** Munition indicator template */
    [SerializeField] private GameObject munitionPrefab = null;

    /** Ordered health indicator controllers */
    private List<StatusIndicator> healthIcons = new List<StatusIndicator>();

    /** Ordered shield indicator controllers */
    private List<StatusIndicator> shieldIcons = new List<StatusIndicator>();

    /** Ordered munition indicator controllers */
    private List<StatusIndicator> munitionIcons = new List<StatusIndicator>();

    /** Player controller */
    private PlayerController player = null;


    /**
     * Initialize this overlay.
     */
    public void Awake() {
        GameObject o = GameObject.FindWithTag("Player");
        player = o.GetComponentInChildren<PlayerController>();
    }


    /**
     * Attach the events.
     */
    public void OnEnable() {
        player.status.statusChanged += OnPlayerStatusChanged;
    }


    /**
     * Detach the events.
     */
    public void OnDisable() {
        player.status.statusChanged -= OnPlayerStatusChanged;
    }


    /**
     * Update the view when the player status changes.
     */
    private void OnPlayerStatusChanged(PlayerStatus status) {
        CreateMunitionIndicators(status);
        CreateHealthIndicators(status);
        CreateShieldIndicators(status);
        UpdateMunitionIndicators(status);
        UpdateShieldIndicators(status);
        UpdateHealthIndicators(status);
    }


    /**
     * Create the munition indicators and add them to the container.
     */
    private void CreateMunitionIndicators(PlayerStatus status) {
        while (munitionIcons.Count < status.munitionSlots) {
            GameObject instance = Instantiate(munitionPrefab, munitionContainer);
            munitionIcons.Add(instance.GetComponent<StatusIndicator>());
        }
    }


    /**
     * Create the shield indicators and add them to the container.
     */
    private void CreateShieldIndicators(PlayerStatus status) {
        while (shieldIcons.Count < status.shieldSlots) {
            GameObject instance = Instantiate(shieldPrefab, healthContainer);
            shieldIcons.Add(instance.GetComponent<StatusIndicator>());
            instance.transform.SetAsFirstSibling();
        }
    }


    /**
     * Create the health indicators and add them to the container.
     */
    private void CreateHealthIndicators(PlayerStatus status) {
        while (healthIcons.Count < status.healthSlots / 2) {
            GameObject instance = Instantiate(healthPrefab, healthContainer);
            healthIcons.Add(instance.GetComponent<StatusIndicator>());
            instance.transform.SetAsFirstSibling();
        }
    }


    /**
     * Update the status of all the munition indicators.
     */
    private void UpdateMunitionIndicators(PlayerStatus status) {
        for (int i = 0; i < munitionIcons.Count; i++) {
            if (i < status.munitionLeft) {
                munitionIcons[i].ChangeToFull();
            } else {
                munitionIcons[i].ChangeToEmpty();
            }
        }
    }


    /**
     * Update the status of all the shield indicators.
     */
    private void UpdateShieldIndicators(PlayerStatus status) {
        for (int i = 0; i < shieldIcons.Count; i++) {
            if (i < status.shieldPoints) {
                shieldIcons[i].ChangeToFull();
            } else {
                shieldIcons[i].ChangeToEmpty();
            }
        }
    }


    /**
     * Update the status of all the health indicators.
     */
    private void UpdateHealthIndicators(PlayerStatus status) {
        for (int i = 0; i < healthIcons.Count; i++) {
            if (i < status.healthPoints / 2) {
                healthIcons[i].ChangeToFull();
            } else if (2 * i < status.healthPoints) {
                healthIcons[i].ChangeToHalf();
            } else {
                healthIcons[i].ChangeToEmpty();
            }
        }
    }
}
