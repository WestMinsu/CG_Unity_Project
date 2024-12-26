using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class playerAmmoBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider MP;
    public TMP_Text myText;

    public Player target;
    public float maxMP;
    public float currentMP;
    void Start()
    {
        maxMP = target.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currentMP = target.health;
        MP.value = currentMP / maxMP;
        myText.text = currentMP.ToString() + "/" + maxMP.ToString();
    }
}
