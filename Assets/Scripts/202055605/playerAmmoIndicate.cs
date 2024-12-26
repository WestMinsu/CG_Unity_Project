using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class playerAmmoIndicate : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text myText;

    public Player target;
    public int ammo; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ammo = target.ammo;
        myText.text = "X" + ammo.ToString();
    }
}
