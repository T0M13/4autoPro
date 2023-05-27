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
    [SerializeField] private bool gameOver;
    //[Header("Item Settings")]
    //[SerializeField] private int itemsSpeed = 1;
    [Header("Player Stats")]
    [SerializeField] private int coins;
    [SerializeField] private float timeScore;
    [Header("Save/Load")]
    [SerializeField] private SaveComponent saveBehaviour;
    [SerializeField] private LoadComponent loadBehaviour;
    [Header("Saved Stats")]
    [SerializeField] private int playerProfileCoins;
    [SerializeField] private float playerProfileScore;

    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }
    public bool GameOver { get => gameOver; set => gameOver = value; }

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
        OnGameOver += CallGameOver;
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
        OnGameOver -= CallGameOver;
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

        GameOver = false;

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

    private void CallGameOver()
    {
        SaveData.PlayerProfile.coins += coins;
        if (SaveData.PlayerProfile.timeScore < timeScore)
            SaveData.PlayerProfile.timeScore = timeScore;
        Save();
        GameOver = true;
        Debug.Log("Game Over");
    }

    private void Update()
    {
        if (playerReferences.PlayerStats.Exploded) return;
        timeScore += Time.deltaTime;

        if (uIManager != null)
            uIManager.TimeScoreUI.text = (Mathf.RoundToInt(timeScore)).ToString();
    }

    private void RestartGame()
    {
        timeScore = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restarting");

    }


    public void GetDamage(int damageValue)
    {
        playerReferences.PlayerStats.Health -= damageValue;
        if (playerReferences.PlayerStats.Health <= 0)
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
        playerProfileScore = SaveData.PlayerProfile.timeScore;
    }
}
