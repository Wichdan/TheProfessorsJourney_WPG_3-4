using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] Slider staminaSystem;
    [SerializeField] Slider manaSystem, defenseSystem;
    Character player;

    float highHp, medHP, lowHP;
    [Header("Screen Effect")]
    [SerializeField] Image screenEffect;
    [SerializeField] Animator screenEffectAnim;

    [Header("Other UI")]
    [SerializeField] GameObject pausedUI, gameOverUI;
    [SerializeField] bool isPaused, isGameOver;
    [SerializeField] Image weaponIcon;
    [SerializeField] TextMeshProUGUI objectiveText, gameOverMsg;
    [SerializeField] private TMP_Text m_textMeshPro;
    [SerializeField] float textSpeed = 0.03f;
    [SerializeField] Button resetGame, exitGame, gameOverReset;

    public static UIManager instance;

    public bool IsGameOver { get => isGameOver; set => isGameOver = value; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();

        staminaSystem.maxValue = player.MaxStamina;
        manaSystem.maxValue = player.MaxMana;
        defenseSystem.maxValue = player.Defense;

        highHp = player.HealthPoint * 90 / 100;
        medHP = player.HealthPoint * 50 / 100;
        lowHP = player.HealthPoint * 30 / 100;
        //Debug.Log("High HP: " + highHp + " Med HP: " + medHP + " Low HP: " + lowHP);

        if (SceneChanger.instance != null)
        {
            resetGame.onClick.AddListener(() =>
            {
                SceneChanger.instance.ResetScene();
            });

            exitGame.onClick.AddListener(() =>
            {
                SceneChanger.instance.ChangeScene(0);
            });

            gameOverReset.onClick.AddListener(() =>
            {
                SceneChanger.instance.ResetScene();
                Time.timeScale = 1;
            });
        }
    }

    private void Update()
    {
        staminaSystem.value = player.Stamina;
        manaSystem.value = player.Mana;
        defenseSystem.value = player.Defense;
        ChangeScreenEffect();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            flipIsPaused();
        }

        if (isPaused)
            PausedGame();
        else
            ContinueGame();
    }

    void ChangeScreenEffect()
    {
        var hp = (float)player.HealthPoint / 100;
        if (hp > highHp)
            screenEffect.enabled = false;
        screenEffectAnim.SetFloat("HP", hp);

    }

    void flipIsPaused() => isPaused = !isPaused;

    void PausedGame()
    {
        Time.timeScale = 0;
        pausedUI.SetActive(true);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        pausedUI.SetActive(false);
        isPaused = false;
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverUI.SetActive(isGameOver);
        PlayTextAni();
        //Time.timeScale = 0;
    }

    public void SetWeaponIcon(Sprite sprite)
    {
        weaponIcon.sprite = sprite;
    }

    public void UpdateObjectiveText(string objective)
    {
        objectiveText.text = objective;
    }

    void PlayTextAni()
    {
        StopAllCoroutines();
        gameOverMsg.text = "Jika kamu menyerah disini, mungkin dunia (ini) akan hancur...";
        StartCoroutine(TextAnimation());
    }

    IEnumerator TextAnimation()
    {
        // Force and update of the mesh to get valid information.
        m_textMeshPro.ForceMeshUpdate();

        int totalVisibleCharacters = m_textMeshPro.textInfo.characterCount; // Get # of Visible Character in text object
        int counter = 0;
        int visibleCount = 0;

        while (counter <= totalVisibleCharacters)
        {
            visibleCount = counter % (totalVisibleCharacters + 1);

            m_textMeshPro.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            // Once the last character has been revealed, wait 1.0 second and start over.
            if (visibleCount >= totalVisibleCharacters)
            {
                yield return new WaitForSeconds(1.0f);
                m_textMeshPro.text = gameOverMsg.text;
                //yield return new WaitForSeconds(3.0f);
            }

            counter += 1;

            yield return new WaitForSeconds(textSpeed);
        }
        //Debug.Log("Done revealing the text.");
    }
}
