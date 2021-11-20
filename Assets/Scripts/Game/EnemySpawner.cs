//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Asteroid bigAsteroid;
    [SerializeField] private Asteroid mediumAsteroid;
    [SerializeField] private Asteroid smallAsteroid;

    private Dictionary<AsteroidType, ObjectPool<Asteroid>> asteroidPool;
    private List<Asteroid> allAsteroids;
    private bool isSpawnEnabled = false;

    public static EnemySpawner Singelton { get; private set; }
    private void Awake()
    {
        Singelton = this;

        asteroidPool = new Dictionary<AsteroidType, ObjectPool<Asteroid>>();
        allAsteroids = new List<Asteroid>();
    }
    private void OnDestroy()
    {
        Singelton = null;
    }

    public void StartSpawn()
    {
        isSpawnEnabled = true;

        StopCoroutine("CorSpawn");
        StartCoroutine("CorSpawn");
    }
    public void StopSpawn()
    {
        isSpawnEnabled = false;

        StopCoroutine("CorSpawn");
    }
    public void ClearAll()
    {
        asteroidPool = new Dictionary<AsteroidType, ObjectPool<Asteroid>>();

        foreach (var a in allAsteroids)
        {
            a.gameObject.SetActive(false);
            BackToPool(a.Type, a);
        }
    }
    private IEnumerator CorSpawn()
    {
        // randomly spawn asteroid in a random time
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(GameManager.Singelton.Data.minSpawnTime, GameManager.Singelton.Data.maxSpawnTime));
            SpawnRandom();
        }
    }
    public void BackToPool(AsteroidType type, Asteroid asteroid)
    {
        if (!asteroidPool.ContainsKey(type))
            asteroidPool.Add(type, new ObjectPool<Asteroid>());

        asteroidPool[type].Add(asteroid);
    }
    private void SpawnRandom()
    {
        float rand = UnityEngine.Random.Range(0, 1f);
        if (rand <= 1f / 3f)
            Spawn(AsteroidType.Small);
        else if (rand <= 2f / 3f)
            Spawn(AsteroidType.Medium);
        else if (rand <= 1f)
            Spawn(AsteroidType.Big);
    }
    public void Spawn(AsteroidType type, Transform target = null)
    {
        if (!isSpawnEnabled)
            return;

        // if target is null , create a random start point , if not , create on target position

        Vector2 pos = (target == null) ? CreatePositionOutside() : (Vector2)target.position;

        Asteroid asteroid = null;
        if (asteroidPool.ContainsKey(type) && asteroidPool[type].HasItem())
        {
            asteroid = asteroidPool[type].Get();
            asteroid.transform.position = pos;
            asteroid.transform.rotation = Quaternion.identity;
            asteroid.gameObject.SetActive(true);
            asteroid.Rb.velocity = Vector2.zero;
        }
        else
        {
            switch (type)
            {
                case AsteroidType.Small:
                    asteroid = Instantiate(smallAsteroid, pos, Quaternion.identity);
                    break;
                case AsteroidType.Medium:
                    asteroid = Instantiate(mediumAsteroid, pos, Quaternion.identity);
                    break;
                case AsteroidType.Big:
                    asteroid = Instantiate(bigAsteroid, pos, Quaternion.identity);
                    break;
                default:
                    throw new System.ArgumentException("Wrong asteroid type.");
            }

            if (asteroid != null)
                allAsteroids.Add(asteroid);
        }

        asteroid.SetMoveTarget(CreatePositionInside());
    }
    private Vector2 CreatePositionOutside()
    {
        int area = Random.Range(0, 2);
        Vector2 pos;
        if (area == 0)
            pos = new Vector2(Random.Range(-.1f, 1.1f), (Random.Range(0f, 1f) < 0.5f) ? 1.1f : -.1f);
        else
            pos = new Vector2((Random.Range(0f, 1f) < 0.5f) ? 1.1f : -.1f, Random.Range(-.1f, 1.1f));

        return GameManager.Singelton.MainCamera.ViewportToWorldPoint(pos);
    }
    private Vector2 CreatePositionInside()
    {
        Vector2 pos = new Vector2(Random.Range(0, 1f), Random.Range(0, 1f));
        return GameManager.Singelton.MainCamera.ViewportToWorldPoint(pos);
    }
}
public enum AsteroidType
{
    None, Big, Medium, Small
}