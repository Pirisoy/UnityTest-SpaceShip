using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Objects/GameData")]
public class GameData : ScriptableObject
{
    [Header("Player Data")]
    public int maxLife = 3;
    [Header("Spawn Times")]
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 5f;
}