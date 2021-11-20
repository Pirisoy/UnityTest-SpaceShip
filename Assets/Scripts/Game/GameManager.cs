using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerShip playerShip;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameData gameData;
    [SerializeField] private Text txtCurrentScore;
    [SerializeField] private Text txtCurrentScoreText;
    [SerializeField] private Image lifePrefab;
    [SerializeField] private Transform lifeParent;
    [SerializeField] private DeathScreen deathScreen;
    [SerializeField] private MyVfx vfxAsteroidDestroy;
    [SerializeField] private GameObject vfxPlayerShipDestoryed;
    [SerializeField] private AudioClip gamePlayMusic;
    [SerializeField] private AudioClip sfxClick;
    [SerializeField] private AudioClip sfxSpawn;
    [SerializeField] private AudioClip sfsLoose;
    public Camera MainCamera => mainCamera;
    public GameData Data => gameData;
    public static GameManager Singelton { get; private set; }

    private int currentLife;
    private int currentScore;
    private PlayerInputActions inputActions;
    private bool isDead = true;
    private ObjectPool<MyVfx> asteroidDestroyVfxPool;

    private void Awake()
    {
        asteroidDestroyVfxPool = new ObjectPool<MyVfx>();

        deathScreen.gameObject.SetActive(false);
        Singelton = this;
    }
    private void OnDestroy()
    {
        ReleaseInputs();
        Singelton = null;
        CancelInvoke();
    }
    private void Start()
    {
        CreateInputs();
        SetInitData();
        ResetPlayerShip();
        Invoke("StartSpawn", 0.5f);


        SoundManager.Singelton.PlayMusic(gamePlayMusic);
    }
    private void SetInitData()
    {
        isDead = false;
        currentLife = gameData.maxLife;
        currentScore = 0;

        CreateLifeGrid();

        txtCurrentScore.text = currentScore.ToString();

        txtCurrentScore.gameObject.SetActive(true);
        txtCurrentScoreText.gameObject.SetActive(true);
    }
    private void CreateLifeGrid()
    {
        for (int i = 0; i < gameData.maxLife; i++)
        {
            Instantiate(lifePrefab, lifeParent);
        }
    }
    private void CreateInputs()
    {
        inputActions = new PlayerInputActions();
        inputActions.GamePlay.Move.performed += context => MoveInput(context);
        inputActions.GamePlay.Move.canceled += context => MoveInputCanceled(context);

        inputActions.GamePlay.Fire.performed += context => StartFireInput(context);
        inputActions.GamePlay.Fire.canceled += context => StopFireInput(context);

        inputActions.Enable();
    }
    private void ReleaseInputs()
    {
        inputActions.Dispose();
    }
    private void MoveInput(InputAction.CallbackContext context)
    {
        if (isDead)
            return;
        playerShip.SetInput(context.ReadValue<Vector2>());
    }
    private void MoveInputCanceled(InputAction.CallbackContext context)
    {
        if (isDead)
            return;
        playerShip.SetInput(Vector3.zero);
    }

    private void StartFireInput(InputAction.CallbackContext context)
    {
        if (isDead)
            return;
        playerShip.Weapon.StartFire();
    }
    private void StopFireInput(InputAction.CallbackContext context)
    {
        if (isDead)
            return;
        playerShip.Weapon.StopFire();
    }

    private void StartSpawn()
    {
        EnemySpawner.Singelton.StartSpawn();
    }
    private void StopSpawn()
    {
        EnemySpawner.Singelton.StopSpawn();
    }

    public void PlayerGotHit()
    {
        if (isDead)
            return;

        isDead = true;
        playerShip.gameObject.SetActive(false);
        EnemySpawner.Singelton.StopSpawn();
        EnemySpawner.Singelton.ClearAll();

        ShowPlayerDestroyedVfx();

        Destroy(lifeParent.GetChild(0).gameObject);

        currentLife--;

        if (currentLife > 0)
        {
            Invoke(nameof(InvRestartLevel), 2);
        }
        else
            Invoke(nameof(FinishGame), .5f);
    }

    public void AsteroidDestroyed(int score, Transform transform)
    {
        ShowAsteroidDestroyVfx(transform.position);

        currentScore += score;

        txtCurrentScore.text = currentScore.ToString();

        if (currentScore > Tools.GetPlayerTopScore())
        {
            Tools.SetPlayerTopScore(currentScore);
        }
    }

    private void InvRestartLevel()
    {
        ResetPlayerShip();

        EnemySpawner.Singelton.StartSpawn();

        isDead = false;
    }
    private void FinishGame()
    {
        SoundManager.Singelton.PlayActionClip(sfsLoose);

        txtCurrentScore.gameObject.SetActive(false);
        txtCurrentScoreText.gameObject.SetActive(false);

        deathScreen.Show(Tools.GetPlayerTopScore(), currentScore);
    }
    public void BtnRestart()
    {
        SoundManager.Singelton.PlayActionClip(sfxClick);

        FullRestart();
    }
    public void BtnBack()
    {
        SoundManager.Singelton.PlayActionClip(sfxClick);

        Invoke(nameof(LoadMainMenuScene), 0.5f);
    }

    private void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void FullRestart()
    {
        deathScreen.gameObject.SetActive(false);
        SetInitData();
        ResetPlayerShip();
        Invoke("StartSpawn", 0.5f);
    }
    private void ResetPlayerShip()
    {
        playerShip.transform.position = Vector2.zero;
        playerShip.transform.rotation = Quaternion.identity;
        playerShip.ResetVelocity();
        playerShip.gameObject.SetActive(true);

        SoundManager.Singelton.PlayActionClip(sfxSpawn);
    }
    private void ShowAsteroidDestroyVfx(Vector2 position)
    {
        GameObject vfx;
        if (asteroidDestroyVfxPool.HasItem())
        {
            vfx = asteroidDestroyVfxPool.Get().gameObject;
            vfx.transform.position = position;
            vfx.gameObject.SetActive(true);
        }
        else
            vfx = Instantiate(vfxAsteroidDestroy, position, Quaternion.identity).gameObject;
    }
    public void BackVfxToPool(MyVfx vfx)
    {
        asteroidDestroyVfxPool.Add(vfx);
    }
    private void ShowPlayerDestroyedVfx()
    {
        vfxPlayerShipDestoryed.transform.position = playerShip.transform.position;
        vfxPlayerShipDestoryed.gameObject.SetActive(true);

        Invoke(nameof(DisableVFXPlayerShipDestroy), 1);
    }
    private void DisableVFXPlayerShipDestroy()
    {
        vfxPlayerShipDestoryed.gameObject.SetActive(false);
    }
}