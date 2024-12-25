using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    //public GameObject missile;
    //public Transform missilePortA;
    //public Transform missilePortB;
    public bool isLook;

    Vector3 lookVec;
    Vector3 chargeVec;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        //meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        nav.isStopped = true;
        StartCoroutine(Think());
    }

    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }

        //if (isLook)
        //{
        //    float h = Input.GetAxisRaw("Horizontal");
        //    float v = Input.GetAxisRaw("Vertical");
        //    lookVec = new Vector3(h, 0, v) * 5f;
        //    transform.LookAt(target.position + lookVec);
        //}
        //else
        //    nav.SetDestination(chargeVec);

        if (nav.enabled) //if (nav.enabled && enemyType != Type.D)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }

    void FixedUpdate()
    {
        FreezeVelocity();
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);
        int ranAction = Random.Range(0, 8);
        switch (ranAction)
        {
            case 0:
            case 1:
                StartCoroutine(Walk());
                break;
            case 2:
            case 3:
            case 4:
                StartCoroutine(Throw());
                break;
            case 5:
            case 6:
            case 7:
                StartCoroutine(Charge());
                break;
                //case 0:
                //case 1:
                //case 2:
                //case 3:
                //case 4:
                //case 5:
                //case 6:
                //case 7:
                //    StartCoroutine(Throw());
                //    break;
        }
    }

    IEnumerator Walk()
    {
        isChase = true;
        anim.SetTrigger("doWalk");
        StartCoroutine("Attack"); 

        yield return new WaitForSeconds(5f); 

        isChase = false;
        StartCoroutine(Think());
    }

    IEnumerator Charge()
    {
        isChase = true;
        anim.SetTrigger("doWalk");
        StartCoroutine("Attack");

        yield return new WaitForSeconds(3f);

        isChase = false;

        chargeVec = target.position + lookVec;

        isLook = false;
        nav.isStopped = false;
        boxCollider.enabled = false;
        anim.SetTrigger("doCharge");

        yield return new WaitForSeconds(2f); // 돌진 준비 
        meleeArea.enabled = true;

        yield return new WaitForSeconds(2f); // 돌진
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);
        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;
        StartCoroutine(Think());
    }

    IEnumerator Throw()
    {
        isChase = true;
        anim.SetTrigger("doWalk");
        StartCoroutine("Attack");

        yield return new WaitForSeconds(3f);

        isChase = false;

        //boxCollider.enabled = false;
        anim.SetTrigger("doThrow");

        yield return new WaitForSeconds(1.4f);
        weaponArea.enabled = true;

        yield return new WaitForSeconds(0.3f);
        weaponArea.enabled = false;

        yield return new WaitForSeconds(1f);
        isLook = true;
        nav.isStopped = true;
        //boxCollider.enabled = true;
        StartCoroutine(Think());
    }

    //IEnumerator MissileShot()
    //{
    //    anim.SetTrigger("doShot");
    //    yield return new WaitForSeconds(0.2f);
    //    GameObject instantMissleA = Instantiate(missile, missilePortA.position, missilePortA.rotation); 
    //    BossMissile bossMissileA = instantMissleA.GetComponent<BossMissile>(); 
    //    bossMissileA.target = target;

    //    yield return new WaitForSeconds(0.3f);
    //    GameObject instantMissleB = Instantiate(missile, missilePortB.position, missilePortB.rotation);
    //    BossMissile bossMissileB = instantMissleB.GetComponent<BossMissile>();
    //    bossMissileB.target = target;

    //    yield return new WaitForSeconds(2f);
    //    StartCoroutine(Think());
    //}

    //IEnumerator RockShot()
    //{
    //    isLook = false; 
    //    anim.SetTrigger("doBigShot");
    //    Instantiate(bullet, transform.position, transform.rotation);
    //    yield return new WaitForSeconds(3f);

    //    isLook = true; 
    //    StartCoroutine(Think());
    //}
}