using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public Boss boss;
    public GameObject itemShop;
    public GameObject weaponShop;
    //public GameObject startZone;
    public int stage;
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;
    public int enemyCntD;

    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject overPanel;
    public GameObject shopPotal;

    public Text maxScoreTxt;
    public TMP_Text scoreTxt;
    public TMP_Text stageTxt;
    //public Text playTimeTxt;
    //public TMP_Text playerHealthTxt;
    //public TMP_Text playerAmmoTxt;
    //public TMP_Text playerCoinTxt;
    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weaponRImg;
    //public Text enemyATxt;
    //public Text enemyBTxt;
    //public Text enemyCTxt;
    public RectTransform bossHealthGroup;
    public Slider bossHealthBar;
    public TMP_Text curScoreText;
    public Text resultScoreText;
    public GameObject highScore;

    void Awake()
    {
        enemyList = new List<int>();
        maxScoreTxt.text = string.Format("High SCORE: {0:n0}", PlayerPrefs.GetInt("MaxScore"));

        if (PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetInt("MaxScore", 0);

        stage = 1;
    }
    public void Practice()
    {
        SceneManager.LoadScene("Practice");
    }

    public void TermProject()
    {
        SceneManager.LoadScene("Map");
    }

    public void GameStart()
    {
        player.gameObject.SetActive(true);
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
        curScoreText.text = scoreTxt.text;

        int maxScore = PlayerPrefs.GetInt("MaxScore");
        Debug.Log(maxScore);
        string result = scoreTxt.text;
        //resultScoreText.text = scoreTxt.text;
        if (player.score > maxScore)
        {
            //highScore.SetActive(true);
            Debug.Log("GameOver, Best Score");
            PlayerPrefs.SetInt("MaxScore", player.score);
            result = result + " High Score!";
        }
        resultScoreText.text = result;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Map");
    }


    public void StageStart()
    {

        shopPotal.gameObject.SetActive(false);

        isBattle = true;
        StartCoroutine(InBattle());
    }

    public void StageEnd()
    {
        shopPotal.gameObject.SetActive(true);
        isBattle = false;
        stage++;
    }


    IEnumerator InBattle()
    {

        if (stage % 5 == 0)
        {
            enemyCntD++;
            GameObject instantEnemy = Instantiate(enemies[3], enemyZones[0].position, enemyZones[0].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy.manager = this;
            enemy.target = player.transform;
            boss = instantEnemy.GetComponent<Boss>();
        }


        else
        {
            for (int index = 0; index < stage; index++)
            {
                int ran = Random.Range(0, 3);
                enemyList.Add(ran);

                switch (ran)
                {
                    case 0:
                        enemyCntA++;
                        break;
                    case 1:
                        enemyCntB++;
                        break;
                    case 2:
                        enemyCntC++;
                        break;
                }
            }

            while (enemyList.Count > 0)
            {
                int ranZone = Random.Range(0, 4);
                GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranZone].position, enemyZones[ranZone].rotation); // 생성 시, 소환 리스트의 첫번째 데이터를 사용
                Enemy enemy = instantEnemy.GetComponent<Enemy>();
                enemy.target = player.transform;
                enemy.manager = this;
                enemyList.RemoveAt(0);
                yield return new WaitForSeconds(4f);
            }
        }

        while (enemyCntA + enemyCntB + enemyCntC + enemyCntD > 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(4f);
        boss = null;
        StageEnd();
    }

    void Update()
    {
        if (isBattle)
            playTime += Time.deltaTime;
        if (boss != null)
        {
            float curhp = boss.curHealth;
            float maxhp = boss.maxHealth;
            bossHealthGroup.anchoredPosition = Vector3.zero;
            bossHealthBar.value = curhp / maxhp;

        }

        else
        {
            bossHealthGroup.anchoredPosition = Vector3.up * 2000;
        }
    }

    void LateUpdate()
    {
        scoreTxt.text = string.Format("SCORE: {0:n0}", player.score);
        stageTxt.text = "STAGE " + stage;

        // 무기 보유 여부 UI
        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
    }
}
