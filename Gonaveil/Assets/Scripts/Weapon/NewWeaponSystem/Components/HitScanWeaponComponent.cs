﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewHitScanComponent", menuName = "Weapons/New Hit Scan Component")]
public class HitScanWeaponComponent : WeaponComponent {
    public GameObject impact;

    public override void OnFireStart() {
        var cast = Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, Mathf.Infinity);

        Debug.DrawLine(camera.position, hit.point, Color.red, 10f);

        if (cast) {
            var impactObject = Instantiate(impact, hit.point, Quaternion.LookRotation(Vector3.up, hit.normal));
        }
    }
}
