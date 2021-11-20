using UnityEngine;

public static class Tools
{
    public static void SetPlayerTopScore(int score)
    {
        PlayerPrefs.SetInt("TopScore", score);
    }
    public static int GetPlayerTopScore()
    {
        return PlayerPrefs.GetInt("TopScore", 0);
    }
}