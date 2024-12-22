using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rigid;

    void Start()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effectObj.SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 15, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
        foreach(RaycastHit hitObj in rayHits) 
        {
            if (hitObj.transform.GetComponent<Enemy>() == null) // TutorialEnemy와 본 게임의 Enemy를 구별
                hitObj.transform.GetComponent<TutorialEnemy>().HitByGrenade(transform.position);
            else
                hitObj.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        }

        Destroy(gameObject, 5); 
    }
}
