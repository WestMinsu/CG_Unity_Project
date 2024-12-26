using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Shop : MonoBehaviour
{
    public RectTransform uiGroup;
    //public Animator anim;

    public GameObject[] itemObj;
    public int[] itemPrice;
    public Transform[] itemPos;

    public TMP_Text[] itemNameText;
    public TMP_Text[] itemPriceText;
    // public string[] talkData;
    //public Text talkText; 

    Player enterPlayer;

    public GameObject LackOfMoneyNote;

    void Start()
    {
        int itemListCount = itemObj.Length;

        for (int i = 0; i < itemListCount; i++)
        {
            itemNameText[i].text = itemObj[i].name;
            itemPriceText[i].text = "X" + itemPrice[i].ToString();
        }
    }

    public void Enter(Player player)
    {
        enterPlayer = player;
        uiGroup.gameObject.SetActive(true);
        uiGroup.anchoredPosition = Vector3.zero;
    }

    public void Exit()
    {
        //Debug.Log("exit");
        uiGroup.anchoredPosition = Vector3.down * 2000;
        uiGroup.gameObject.SetActive(false);
    }

    public void Buy(int index)
    {
        int price = itemPrice[index];
        if (price > enterPlayer.coin)
        {
            StopCoroutine(LackOfMoneyAlarm());
            StartCoroutine(LackOfMoneyAlarm());
            return;
        }

        enterPlayer.coin -= price;
        Vector3 ranVec = Vector3.right * Random.Range(-3, 3)
                         + Vector3.forward * Random.Range(-3, 3);
        Instantiate(itemObj[index], itemPos[index].position + ranVec, itemPos[index].rotation);
    }

    IEnumerator LackOfMoneyAlarm()
    {
        LackOfMoneyNote.SetActive(true);
        yield return new WaitForSeconds(2f);
        LackOfMoneyNote.SetActive(false);
    }
}
