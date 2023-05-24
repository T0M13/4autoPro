using System;
using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("References")]
    [SerializeField] private ChunkManager chunkManager;
    [SerializeField] private PlayerReferences playerReferences;
    [SerializeField] private UIManager uIManager;
    [Header("Game Positions")]
    [SerializeField] private Vector3 startPosition;
    //[Header("Item Settings")]
    //[SerializeField] private int itemsSpeed = 1;
    [Header("Player Stats")]
    [SerializeField] private int health = 1;
    [SerializeField] private int coins;
    [SerializeField] private float score;
    [Header("Save/Load")]
    [SerializeField] private SaveComponent saveBehaviour;
    [SerializeField] private LoadComponent loadBehaviour;
    [Header("Saved Stats")]
    [SerializeField] private int playerProfileCoins;
    [SerializeField] private float playerProfileScore;

    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }

    public Action OnAddCoin;
    public Action<int> OnGetDamage;
    public Action OnSave;
    public Action OnLoad;
    public Action OnGameOver;

    public Action OnMagnetPowerUp;
    public Action OnSpeedPowerUp;

    private void OnEnable()
    {
        OnAddCoin += AddCoin;
        OnSave += Save;
        OnLoad += Load;
        OnGameOver += GameOver;
        OnGetDamage += GetDamage;
        InputManager.instance.swipeDetector.OnSwipeUp += RestartGame;


        OnMagnetPowerUp += MagnetPowerUp;
        OnSpeedPowerUp += SpeedPowerUp;

    }

    private void OnDisable()
    {
        OnAddCoin -= AddCoin;
        OnSave -= Save;
        OnLoad -= Load;
        OnGameOver -= GameOver;
        OnGetDamage -= GetDamage;
        InputManager.instance.swipeDetector.OnSwipeUp -= RestartGame;

        OnMagnetPowerUp -= MagnetPowerUp;
        OnSpeedPowerUp -= SpeedPowerUp;

    }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        Load();
        Save();


        if (chunkManager == null)
            chunkManager = FindObjectOfType<ChunkManager>();

        if (playerReferences == null)
            playerReferences = FindObjectOfType<PlayerReferences>();

        if (uIManager == null)
            uIManager = FindObjectOfType<UIManager>();
    }

    private void AddCoin()
    {
        coins++;
        Debug.Log("Current Coins: " + coins);
    }

    private void GameOver()
    {
        SaveData.PlayerProfile.coins += coins;
        if (SaveData.PlayerProfile.score < score)
            SaveData.PlayerProfile.score = score;
        Save();
        Debug.Log("Game Over");
    }

    private void Update()
    {
        if (playerReferences.PlayerStats.Exploded) return;
        score += Time.deltaTime;
        score = score % 60;

        if (uIManager != null)
            uIManager.ScoreUI.text = (Mathf.RoundToInt(score)).ToString();
    }

    private void RestartGame()
    {
        score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restarting");

    }


    public void GetDamage(int damageValue)
    {
        health -= damageValue;
        if (health <= 0)
        {
            playerReferences.PlayerStats.OnExplode?.Invoke();
        }
    }

    private void MagnetPowerUp()
    {
        if (playerReferences == null)
            playerReferences = FindObjectOfType<PlayerReferences>();
        if (playerReferences == null) return;

        //playerReferences.PlayerInteractor.OnMagnetPowerUp?.Invoke();
    }

    private void SpeedPowerUp()
    {
        if (playerReferences == null)
            playerReferences = FindObjectOfType<PlayerReferences>();
        if (playerReferences == null) return;

        //playerReferences.PlayerController.OnSpeedPowerUp?.Invoke();
    }

    private void Save()
    {
        saveBehaviour.Save();
    }

    private void Load()
    {
        loadBehaviour.Load();

        playerProfileCoins = SaveData.PlayerProfile.coins;
        playerProfileScore = SaveData.PlayerProfile.score;
    }
}
