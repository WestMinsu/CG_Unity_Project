using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class playerHpBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider HP;
    public TMP_Text myText; 

    public Player target;
    public float maxHP;
    public float currentHP;
    void Start()
    {
        maxHP = target.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currentHP = target.health;
        HP.value = currentHP / maxHP;
        myText.text = currentHP.ToString() +"/"+ maxHP.ToString();
    }
}
