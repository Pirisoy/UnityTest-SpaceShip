using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Text txtMaxScore;
    [SerializeField] private Text txtCurrentScore;

    public void Show(int maxScore, int currentScore)
    {
        txtMaxScore.text = maxScore.ToString();
        txtCurrentScore.text = currentScore.ToString();

        gameObject.SetActive(true);
    }
}