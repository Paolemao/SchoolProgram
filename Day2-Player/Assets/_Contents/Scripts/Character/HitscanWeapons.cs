using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanWeapons : Wepon {

    public enum FireMode
    {
        SemiAuto,

        FullAuto
    }

    [SerializeField]
    private RayImpact rayImpact;

    [SerializeField]
    private FireMode fireMode;

    [SerializeField]
    private SoundsPlayer fireAudio;

    [SerializeField]
    private ParticleSystem muzzleFlash;

    [SerializeField]
    private GameObject tracer;

    [Range(0f, 30f)]
    [SerializeField]
    private float spreadNoraml = 0.8f;

    [SerializeField]
    [Range(0f, 30f)]
    private float spreadAim = 0.95f;

    [SerializeField]
    [Range(1, 20)]
    private int rayCount = 1;

    [SerializeField]
    private float distanceMax = 150f;

    [SerializeField]
    private LayerMask damageMask;

    [SerializeField]
    private float shotDuration = 0.22f;

    [SerializeField]
    private float shotsPerMinute = 450;

    private float timeBetweenShotsMin;
    private float nextTimeCanFire;
    private AudioSource audioSource;

    public override bool AttackOnceHandle()
    {
        if (Time.time<nextTimeCanFire||!IsEquiped)
        {
            return false;
        }
        nextTimeCanFire = Time.time + timeBetweenShotsMin;
        Shoot();
        return base.AttackOnceHandle();
    }

    public override bool AttackContinuouslyHandle()
    {
        if (fireMode == FireMode.SemiAuto)
            return false;

        return AttackOnceHandle();
    }

    protected void Shoot()
    {
        fireAudio.Play(SoundsPlayer.Selection.Randomly, audioSource, 1f);
        if (muzzleFlash)
        {
            muzzleFlash.Play(true);
        }

        for (int i=0;i<rayCount;i++)
        {
            DoHitsCan();
        }
        onAttack.Invoke();
    }

    protected void DoHitsCan()
    {
        float spread = spreadAim;//Player.aim.Active ? spreadAim : spreadNormal;
        RaycastHit hitInfo;

        var firePos = new Vector3(User.transform.position.x,
            User.transform.position.y + User.GetComponent<CapsuleCollider>().height / 2,
            User.transform.position.z
            );

        Ray ray = new Ray(firePos, User.transform.forward);
        //Vector3 spreadVector = character.transform.TransformVector(new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0f));
        //ray.direction = Quaternion.Euler(spreadVector) * ray.direction;

        Debug.DrawLine(firePos, firePos + User.transform.forward * distanceMax, Color.red, 1100);

        if (Physics.Raycast(ray,out hitInfo,distanceMax,damageMask,QueryTriggerInteraction.Ignore))
        {
            if (hitInfo.collider.gameObject==User)
            {
                return;
            }

            float impulse = rayImpact.GetImpulseAtDistance(hitInfo.distance,distanceMax);
            float damage = rayImpact.GetDamageAtDistance(hitInfo.distance,distanceMax);

            var damageable = hitInfo.collider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                var damageData = new DamageEventData(-damage, User, hitInfo.point, ray.direction, impulse);
            }
            else if (hitInfo.rigidbody)
            {
                hitInfo.rigidbody.AddForceAtPosition(ray.direction*impulse,hitInfo.point);
            }

            if (tracer)
            {
                var temp = Instantiate(tracer,transform.position,Quaternion.LookRotation(ray.direction));
                Destroy(temp, 1);
            }
        }
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (fireMode == FireMode.SemiAuto)
            timeBetweenShotsMin = shotDuration;
        else
            timeBetweenShotsMin = 60f / shotsPerMinute;
    }

}
