using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class HudIntegration : MonoBehaviour {

    public Text gold;
    public Text hp;
    public Text ammo;
    public Text Reloading;
    public Image fadePlane;
    public GameObject gameOverUI;
    public GameObject gameUI;
    public GameObject PauseUI;
    GameObject target;

    public Text waveCountdownText;
    public Text waveNumber;

    public int countdown = 5;

    [SerializeField]
    Spawner spwner;
    [SerializeField]
    Animator waveAnimator;


    bool paused = false;

    void Start()
    {
        paused = false;
        PauseUI.SetActive(false);
        gameUI.SetActive(true);
        target = GameObject.FindGameObjectWithTag("Player");
        FindObjectOfType<Player>().OnDeath += OnGameOver;

    }

    public void NextWave()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (countdown > 0)
        {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountdown", true);
            countdown--;
            yield return new WaitForSeconds(1f);
        }
        if (countdown == 0)
        {
            spwner.NextWave();
            waveAnimator.SetBool("WaveIncoming", true);
            waveAnimator.SetBool("WaveCountdown", false);
        }
        yield break;
    }

    void OnGameOver()
    {

        StartCoroutine(Fade(Color.clear, Color.black, 1));
        gameOverUI.SetActive(true);
        gameUI.SetActive(false);

    }

    IEnumerator Fade(Color from, Color to, float time)
    {

        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }

    }

    void Update()
    {
        waveCountdownText.text = countdown.ToString();
        waveNumber.text = spwner.currentWaveNumber.ToString();
        gold.text = "$ " + target.GetComponent<Player>().goldAmount.ToString();
        hp.text = target.GetComponent<LivingEntity>().CurrentHealth.ToString();
        ammo.text = target.GetComponent<GunController>().equippedGun.GetComponent<Gun>().cartridgeAmmo.ToString() + " / " + target.GetComponent<GunController>().equippedGun.GetComponent<Gun>().ammo.ToString();
        Reloading.enabled = target.GetComponent<GunController>().equippedGun.GetComponent<Gun>().isReloading;

        if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.P))
        {
            paused = !paused;
        }

        if (paused)
        {
            PauseUI.SetActive(true);
             Time.timeScale = 0.3f;
        }
        else
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    //List IDS
    // 0 = ak
    // 1 = pistol
    //

    public void AddAmmo(string n ,int price)
    {
        if(target.GetComponent<Player>().goldAmount >= price)
        {
            foreach (var g in target.GetComponentsInChildren<Gun>())
            {
                if(g.weaponName == n)
                {
                    g.AddAmmo(g.cartidgeMaxAmmo);
                    target.GetComponent<Player>().addGold(-70);
                }
            }
        }
    }


    public void AddAmmoAk()
    {
        if (target.GetComponent<Player>().goldAmount >= 70)
        {
            target.GetComponentsInChildren<Gun>()[0].AddAmmo(30);
            target.GetComponent<Player>().addGold(-70);
        }
    }

    public void AddAmmoPistol()
    {
        if (target.GetComponent<Player>().goldAmount >= 50)
        {
            target.GetComponentsInChildren<Gun>()[1].AddAmmo(20);
            target.GetComponent<Player>().addGold(-50);
        }
    }

    public void UpgradeGunLevel()
    {
        if (target.GetComponent<Player>().goldAmount >= 150)
        {
            target.GetComponent<Player>().addGunLevel();
            target.GetComponent<Player>().addGold(-150);
        }
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("Game");

    }

}
