﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public int primaryWeaponID;
    public int secondaryWeaponID;
    public WeaponParameters[] allWeapons;
    public Weapon weaponMaster;
    public int selectedWeaponID;

    private WeaponMovement weaponMovement;
    private int lastSelectedWeaponID = -1;
    private WeaponParameters current;

    void Start()
    {
        weaponMovement = GetComponent<WeaponMovement>();

        SetWeapon();
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            if (++selectedWeaponID > allWeapons.Length - 1) selectedWeaponID = 0;

            SetWeapon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            if (--selectedWeaponID < 0) selectedWeaponID = 1;

            SetWeapon();
        }
    }

    void SetWeapon() {
        //if (lastSelectedWeaponID > -1) allWeapons[lastSelectedWeaponID].weaponStats.modelObject.SetActive(false);

        current = allWeapons[selectedWeaponID];

        weaponMaster.SetParameters(current);
        weaponMovement.Profile = current.weaponMovementProfile;
        weaponMovement.offset = current.offset;

        lastSelectedWeaponID = selectedWeaponID;
    }
}
