using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider staminaSystem;
    [SerializeField] Slider manaSystem, defenseSystem;
    Character player;

    float highHp, medHP, lowHP;
    [SerializeField] Image screenEffect;
    [SerializeField] Animator screenEffectAnim;
    [SerializeField] GameObject pausedUI;
    [SerializeField] bool isPaused;
    [SerializeField] Image weaponIcon;
    [SerializeField] TextMeshProUGUI objectiveText;

    public static UIManager instance;
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

    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void SetWeaponIcon(Sprite sprite)
    {
        weaponIcon.sprite = sprite;
    }

    public void UpdateObjectiveText(string objective)
    {
        objectiveText.text = objective;
    }
}
