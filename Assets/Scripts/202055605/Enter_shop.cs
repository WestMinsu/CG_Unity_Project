using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enter_shop : MonoBehaviour
{
    public GameObject enterPoint;
    public GameManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.gameObject.transform.position = enterPoint.transform.position;

    }
}
