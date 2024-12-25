using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A, B, C, D };

    public Type enemyType;
    public int maxHealth; 
    public int curHealth;
    public int score;
    public GameManager manager;
    public Transform target;
    public BoxCollider meleeArea;
    public BoxCollider weaponArea;
    public GameObject bullet; 
    public GameObject[] coins;
    public bool isChase;
    public bool isAttack;
    public bool isDead; 


    public Rigidbody rigid;
    public BoxCollider boxCollider;
    // public MeshRenderer[] meshs;
    public SkinnedMeshRenderer[] skinnedMeshs;
    public NavMeshAgent nav;
    public Animator anim;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        // meshs = GetComponentsInChildren<MeshRenderer>();
        skinnedMeshs = GetComponentsInChildren<SkinnedMeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        //if(enemyType != Type.D)
        Invoke("ChaseStart", 2);
    }

    public void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    void Update()
    {
        if (nav.enabled) //if (nav.enabled && enemyType != Type.D)
        {
            nav.SetDestination(target.position); 
            nav.isStopped = !isChase; 
        }
    }

    public void FreezeVelocity()
    {
        if(isChase)
        {
            rigid.velocity = Vector3.zero; 
            rigid.angularVelocity = Vector3.zero;
        }
    }


    public void Targeting()
    {
        if (!isDead) //if(!isDead && enemyType != Type.D)
        {
            float targetRadius = 0; 
            float targetRange = 0;
            switch (enemyType)
            {
                case Type.A:
                    targetRadius = 1.5f;
                    targetRange = 5f;
                    break;
                case Type.B:
                    targetRadius = 1.5f; 
                    targetRange = 8.5f;
                    break;
                case Type.C:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;
                case Type.D:
                    targetRadius = 1.5f;
                    targetRange = 15f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack) 
            {
                StartCoroutine(Attack());
            }
        }
        
    }

    IEnumerator Attack()
    {
        isChase = false; 
        isAttack = true;
        anim.SetBool("isAttack", true);

        switch(enemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(0.5f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Type.B:
                yield return new WaitForSeconds(0.5f);
                weaponArea.enabled = true;

                yield return new WaitForSeconds(1f);
                weaponArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Type.C:
                yield return new WaitForSeconds(0.5f);
                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                Rigidbody rigidbullet = instantBullet.GetComponent<Rigidbody>();
                rigidbullet.velocity = transform.forward * 20;

                yield return new WaitForSeconds(1.5f);
                break;
            case Type.D:
                yield return new WaitForSeconds(0.75f);
                weaponArea.enabled = true;

                yield return new WaitForSeconds(0.75f);
                weaponArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
        }
        
        isChase = true; 
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;

            StartCoroutine(OnDamage(reactVec, false)); 
        }

        else if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject); 

            StartCoroutine(OnDamage(reactVec, false));
        }
    }

    public void HitByGrenade(Vector3 explosionPos) 
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));
    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        if(!isDead)
        {
            //foreach (MeshRenderer mesh in meshs)
            //    mesh.material.color = Color.red;
            foreach (SkinnedMeshRenderer mesh in skinnedMeshs)
                mesh.material.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            if (curHealth > 0) 
            {
                //foreach (MeshRenderer mesh in meshs)
                //    mesh.material.color = Color.white

         
                foreach (SkinnedMeshRenderer mesh in skinnedMeshs)
                    if (mesh.gameObject.CompareTag("BlackMesh"))
                        mesh.material.color = Color.black;
                    else
                        mesh.material.color = Color.white;

                yield return new WaitForSeconds(0.1f);
            }

            else
            {
                if (isDead)
                    yield break;

                isDead = true;
                foreach (SkinnedMeshRenderer mesh in skinnedMeshs)
                    mesh.material.color = Color.gray;
                gameObject.layer = 12; // EnemyDead 레이어 번호 = 12
                                      
                isChase = false;
                nav.enabled = false; 
                anim.SetTrigger("doDie");
                //Debug.Log("몬스터 사망");

                Player player = target.GetComponent<Player>();
                player.score += score;
                int ranCoin = Random.Range(0, 3);
                Instantiate(coins[ranCoin], transform.position, Quaternion.identity);

                switch (enemyType)
                {
                    case Type.A:
                        manager.enemyCntA--;
                        break;
                    case Type.B:
                        manager.enemyCntB--;
                        break;
                    case Type.C:
                        manager.enemyCntC--;
                        break;
                    case Type.D:
                        manager.enemyCntD--;
                        break;
                }

                if (isGrenade)
                {
                    reactVec = reactVec.normalized;
                    reactVec += Vector3.up * 3;
                    rigid.freezeRotation = false; 
                    rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                    rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
                }

                else
                {
                    reactVec = reactVec.normalized;
                    reactVec += Vector3.up;
                    rigid.AddForce(reactVec * 5, ForceMode.Impulse); 
                }

                Destroy(gameObject, 4); 
            }

        }
    }
        
}
