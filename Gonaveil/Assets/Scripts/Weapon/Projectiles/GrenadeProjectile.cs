﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : Projectile
{
    public float fuseTime = 3f;
    public float bounceMultipler = 0.95f;
    public float friction = 0.1f;
    public float velocityInheritance = 0.65f;
    public GameObject explosionParticle;

    public float explosionRadius = 3f;
    public float explosionForce = 500f;

    private Vector3 normal;
    private bool hasDownContact;
    private Vector3 rollAxis;

    public override void OnStart() {
        velocity = transform.forward * startVelocity;

        velocity += instigator.GetComponent<CharacterController>().velocity * velocityInheritance;

        rollAxis = Random.onUnitSphere;

        var time = fuseTime - weapon.chargeProgress * fuseTime;

        print(weapon.chargeProgress);

        StartCoroutine(Fuse(time));
    }

    public override void OnUpdate() {
        effect.Rotate(rollAxis, (velocity.magnitude / radius) * Mathf.Rad2Deg * Time.fixedDeltaTime * timeScale, Space.World);
    }

    public override void OnHit(ref Vector3 position, float deltaTime, RaycastHit hit) {
        var rollVelocity = Vector3.ProjectOnPlane(velocity, hit.normal);
        var bounceVelocity = Vector3.Reflect(velocity.normalized, hit.normal) * velocity.magnitude * bounceMultipler;
        var lerp = Vector3.Dot(velocity.normalized,-hit.normal);

        velocity = Vector3.Lerp(rollVelocity, bounceVelocity, lerp);

        position = hit.point + hit.normal * radius;

        normal = hit.normal;

        rollAxis = -Vector3.Cross(rollVelocity.normalized, hit.normal);

        Debug.DrawLine(hit.point, hit.point + rollAxis, Color.blue, 10f);
    }

    public override void OnSimulate(ref Vector3 position, float deltaTime) {
        hasDownContact = Physics.Raycast(position, Vector3.down, out RaycastHit hit, radius, mask);

        if (hasDownContact) {
            position += hit.normal * (radius - hit.distance);

            velocity *= 1 - (friction * deltaTime);
        }
    }

    public override void OnSimulateGravity(ref Vector3 position, float deltaTime) {
        var gravityDirection = Physics.gravity;

        if (normal.y > 0 && (hasContact || hasDownContact)) {
            gravityDirection = Vector3.ProjectOnPlane(Physics.gravity, normal);
        }

        velocity += gravityDirection * gravityScale * deltaTime;
    }

    private IEnumerator Fuse (float time) {
        yield return new WaitForSeconds(time);

        GamePlayPhysics.DoExplosion(transform.position, explosionRadius, explosionForce);
        Instantiate(explosionParticle,transform.position,Quaternion.Euler(0,0,0));

        Destroy(gameObject);
    }
}
