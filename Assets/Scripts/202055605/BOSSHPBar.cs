using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class BOSSHPBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider HP;
    public TMP_Text myText;

    public Boss target;
    public float maxHP;
    public float currentHP;
    public RectTransform uiGroup;
    void Start()
    {
        maxHP = target.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        if (target != null)
        {
            uiGroup.anchoredPosition = Vector3.zero;
        }

        else
        {
            uiGroup.anchoredPosition = Vector3.up * 2000;
        }
        currentHP = target.curHealth;
        HP.value = currentHP / maxHP;
        //myText.text = currentHP.ToString() + "/" + maxHP.ToString();
    }
}
