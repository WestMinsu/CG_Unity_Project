using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Nav ���� Ŭ������ UnityEngine.AI ���ӽ����̽� ���

public class TutorialEnemy : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
    public int score;
    public GameManager manager; 
    public Transform target;
    public BoxCollider meleeArea;
    public GameObject bullet; 
    public GameObject[] coins;
    public bool isAttack;
    public bool isDead; 

    public Rigidbody rigid;
    public BoxCollider boxCollider;
    public MeshRenderer[] meshs; 
    public NavMeshAgent nav;
    public Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;

            StartCoroutine(OnDamage(reactVec, false));
        }

        else if (other.tag == "Bullet")
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

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade) // ����ź���� ���׼��� ���� bool �Ű����� �߰�
    {
        if (!isDead)
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            if (curHealth > 0) // �����ִ� ü���� �������� �ǰ� ��� ���� �ۼ�
            {
                foreach (MeshRenderer mesh in meshs)
                    mesh.material.color = Color.white;

                yield return new WaitForSeconds(0.1f);
            }

            else
            {
                if (isDead)
                    yield break;

                isDead = true;
                foreach (MeshRenderer mesh in meshs)
                    mesh.material.color = Color.gray;
                gameObject.layer = 12; // EnemyDead ���̾� ��ȣ = 12

                Debug.Log("���� ���");

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