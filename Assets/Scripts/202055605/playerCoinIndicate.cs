using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class playerCoinIndicate : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text myText;

    public Player target;
    public int coin;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        coin = target.coin;
        myText.text = "X" + coin.ToString();
    }
}
