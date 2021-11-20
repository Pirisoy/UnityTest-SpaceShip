using UnityEngine;

[CreateAssetMenu(fileName = "New Asteroid", menuName = "Objects/Asteroid")]
public class AsteroidData : ScriptableObject
{
    [Header("Type of Asteroid")]
    public AsteroidType type;
    [Header("Type of Asteroid that spawns after destroy")]
    public AsteroidType child;
    [Header("Hit Score")]
    public int hitScore;
    [Header("Asteroids Move Speed")]
    public float minMoveSpeed = 1f;
    public float maxMoveSpeed = 5f;
}