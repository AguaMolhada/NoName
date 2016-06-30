using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity {

    public enum State
    {
        Idle,
        Chasing,
        Attacking
    };
    State currentState;

    public float AnimationSpeedWalking;
    public ParticleSystem deathEffect;
    public int damage;

    NavMeshAgent pathFinder;
    Transform target;
    LivingEntity targetEntity;
    Animator animator;

    float attackDistance = 1.5f;
    float timeBetweenAttacks = 1f;
    float nextAttackTime;

    bool hastarget;

    void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            currentState = State.Chasing;
            hastarget = true;
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;
            StartCoroutine(UpdatePath());
            this.OnDeath += DropGold;
        }
    }

    protected override void Start()
    {
        base.Start();


    }

    public void SetCharacteristics(float moveSpeed, float hitsToKillPlayer, float enemyHealth, float animSpeed)
    {
        pathFinder.speed = moveSpeed;
        AnimationSpeedWalking = animSpeed;

        if (hastarget)
        {
            damage = (int)Mathf.Ceil(targetEntity.startingHealth / hitsToKillPlayer);
        }
        startingHealth = startingHealth+enemyHealth;

    }


    void DropGold()
    {
        GetComponent<ItemLoot>().SpawnLoot();
    }

    void OnTargetDeath()
    {
        hastarget = false;
        currentState = State.Idle;
    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (damage >= health)
        {

            Destroy(Instantiate(deathEffect, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)) as GameObject, deathEffect.startLifetime);
        }
        base.TakeHit(damage, hitPoint, hitDirection);
    }

    void Update()
    {
        if (hastarget)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDistanceToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDistanceToTarget < Mathf.Pow(attackDistance, 2))
                {
                    animator.SetBool("Idle", true);
                    currentState = State.Attacking;
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
                else
                {
                    animator.SetFloat("Speed", AnimationSpeedWalking);
                    animator.SetBool("Idle", false);
                    currentState = State.Chasing;
                }
            }
        }
        if (!hastarget)
        {
            animator.SetBool("Idle", false);
        }
    }

    IEnumerator Attack()
    {
        pathFinder.enabled = false;

        Vector3 originalPos = transform.position;
        Vector3 attackPos = target.position;

        float attackSpeed = 3;
        float percent = 0;

        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }
            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent,2) + percent)*4;
            transform.position = Vector3.Lerp (originalPos,attackPos,interpolation);

            yield return null;
        }

        pathFinder.enabled = true;
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = 0.2f;
        while (hastarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
                if (!dead)
                {
                    pathFinder.SetDestination(targetPosition);

                }
            }
            yield return new WaitForSeconds(refreshRate);    
        }
    }
}
