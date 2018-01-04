using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : Entity
{
    public DamageSource CollisionDamageSource;

    public float DocileDuration = 2f;

    [Header("Difficulty (X) is Ranged from 0 to 1")]
    public AnimationCurve SpeedBasedOnDifficulty;
    public AnimationCurve AccuracyBasedOnDifficulty;
    [Header("Y is Distance")]
    public AnimationCurve AggroBasedOnDifficulty;

    public float StateUpdateInterval = 0.5f;

    public float MinFireInterval = 0.5f;
    private float lastFire = 0f;
    public float ProjectileSpeed = 5f;

    private Collider2D myCollision;
    private Coroutine stateMachine;

    private Vector2 roamDir = Vector2.zero;
    private Vector2 curRoamDir = Vector2.zero;
    private float roamSpeed = 0f;
    private float nextRoamUpdate = 0f;
    private bool leaving = false;

    public float FireDistance = 12f;
    public float AccuracyDistance = 2.5f;

    private Animator myAnimator;

    public ParticleSystem ParticleSystem;

    protected override void Awake()
    {
        base.Awake();

        myCollision = GetComponent<Collider2D>();
        myAnimator  = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();

        if(CollisionDamageSource == null)
            CollisionDamageSource = GetComponent<DamageSource>();
    }

    public override void ResetObject()
    {
        base.ResetObject();

        this.ParticleSystem.transform.SetParent(transform, false);
        this.ParticleSystem.transform.position = Vector3.zero;
        leaving = false;
        myCollision.enabled = false;

        InvaderManager.RegisterNewInvader(this);

        if (stateMachine != null)
            StopCoroutine(stateMachine);

        stateMachine = StartCoroutine(stateUpdate());
    }

    public override void Destroyed()
    {
        this.ParticleSystem.transform.SetParent(null, false);
        base.Destroyed();
    }

    private void OnDisable()
    {
        InvaderManager.RemoveInvader(this);
    }

    private void Update()
    {
        if(!leaving && !InvaderManager.IsInsideBounds(new Vector2(transform.position.x, transform.position.y)))
        {
            if (stateMachine != null)
                StopCoroutine(stateMachine);
            StartCoroutine(LeaveZone());
        }
    }

    protected bool GoHostile(out float dist)
    {
        float difficulty = InvaderManager.GetDifficulty();
        dist = Vector2.Distance(InvaderManager.I.PlayerObject.transform.position, transform.position);
        float aggroDist = AggroBasedOnDifficulty.Evaluate(difficulty);

        if (InvaderManager.I.PlayerObject.GetCurrentHealth <= 0)
            return false;

        return dist < aggroDist;
    }

    protected IEnumerator LeaveZone()
    {
        leaving = true;
        myCollision.enabled = false;
        myAnimator.SetTrigger("Leave");
        InvaderManager.I.SpawnInvader();

        yield return new WaitForSeconds(DocileDuration);
        DisableAndReuse();
    }

    protected IEnumerator stateUpdate()
    {
        yield return new WaitForSeconds(DocileDuration);
        myCollision.enabled = true;

        while(Health > 0)
        {
            float dist = 0f;
            if(GoHostile(out dist))
            {
                while (Aggro())
                    yield return null;
            }

            float roamFor = Time.time + StateUpdateInterval;

            while (roamFor > Time.time)
            {
                Roam();
                yield return null;
            }
        }
    }

    protected bool Aggro()
    {
        float dist = 0f;
        if (!GoHostile(out dist))
            return false;
        else
        {
            Vector2 tVel = InvaderManager.I.PlayerObject.GetVelocity / Time.deltaTime;

            float mySpeed = SpeedBasedOnDifficulty.Evaluate(InvaderManager.GetDifficulty());
            float tMagn = tVel.magnitude;
            float delta = tMagn / mySpeed;
            Vector2 desPoint = InvaderManager.I.PlayerObject.transform.position;
            desPoint += (tVel * delta);

            transform.position = Vector2.MoveTowards(transform.position, desPoint, mySpeed * Time.deltaTime);

            FireAtPlayer();

            return true;
        }
    }

    protected void FireAtPlayer()
    {
        if(InvaderManager.I.PlayerObject.GetCurrentHealth > 0 && lastFire < Time.time)
        {
            ParticleSystem.EmitParams eparam = new ParticleSystem.EmitParams();

            float accuracy = AccuracyBasedOnDifficulty.Evaluate(InvaderManager.GetDifficulty());

            Vector2 tVel = InvaderManager.I.PlayerObject.GetVelocity / Time.deltaTime;

            float tMagn = tVel.magnitude;
            float delta = tMagn / ProjectileSpeed;

            Vector2 desPoint = InvaderManager.I.PlayerObject.transform.position;
            desPoint += (tVel * delta);
            desPoint += Random.insideUnitCircle * (1f - accuracy) * AccuracyDistance;

            Vector2 dir = desPoint - new Vector2(transform.position.x, transform.position.y);
            Debug.DrawRay(desPoint, dir, Color.green, 1f);

            eparam.velocity = dir.normalized * ProjectileSpeed;
            eparam.position = transform.position;

            ParticleSystem.Emit(eparam, 1);
            lastFire = Time.time + MinFireInterval + Random.Range(0f, 2f);
        }
    }

    protected void Roam()
    {
        if (Time.time > nextRoamUpdate)
        {
            float xBias = InvaderManager.I.PlayerObject.transform.position.x - transform.position.x;

            xBias = Mathf.Clamp(xBias, -1f, 1f) * 0.4f;

            Vector2 dir = (Vector2.right * xBias) + Random.insideUnitCircle;
            dir.y *= 0.3f; // More horizontal movement
            roamDir = dir.normalized;
            roamSpeed = SpeedBasedOnDifficulty.Evaluate(InvaderManager.GetDifficulty()) * Random.Range(0.5f, 1f);
            nextRoamUpdate = Time.time + Random.Range(0.5f, 4f);
        }

        if(FireDistance > Vector3.Distance(InvaderManager.I.PlayerObject.transform.position, transform.position))
        {
            FireAtPlayer();
        }

        curRoamDir = Vector3.RotateTowards(curRoamDir, roamDir, 2f * Time.deltaTime, 4f * Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y) + curRoamDir, roamSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Defender dfndr = null;
        if(dfndr = collision.GetComponent<Defender>())
        {
            if(CollisionDamageSource)
            {
                dfndr.DamageTaken(CollisionDamageSource.Damage);

                if (CollisionDamageSource.OnDamageDone != null)
                    CollisionDamageSource.OnDamageDone.Invoke();
            }
        }
    }
}
