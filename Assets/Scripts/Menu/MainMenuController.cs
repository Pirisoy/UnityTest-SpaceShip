using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Text txtTopScore;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip sfxClick;

    private void Awake()
    {
        txtTopScore.text = Tools.GetPlayerTopScore().ToString();
    }
    private void Start()
    {
        SoundManager.Singelton.PlayMusic(menuMusic);
    }
    public void BtnOpenGamePlay()
    {
        SoundManager.Singelton.PlayActionClip(sfxClick);

        Invoke(nameof(LoadGamePlayScene), 0.5f);
    }

    private void LoadGamePlayScene()
    {
        SceneManager.LoadScene("GamePlay");
    }
}