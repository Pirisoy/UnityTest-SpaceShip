using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerShip))]
public class PlayerWeapon : MonoBehaviour
{
    //SerializeFields
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private AudioClip bulletSfx;

    // private data
    private float lastFireTime;
    private ObjectPool<Bullet> bulletPool;

    private PlayerShip playerShip;

    private void Awake()
    {
        playerShip = GetComponent<PlayerShip>();
        bulletPool = new ObjectPool<Bullet>();
    }

    public void StartFire()
    {
        StartCoroutine("CorFire");
    }
    public void StopFire()
    {
        StopCoroutine("CorFire");
    }
    private void OnDisable()
    {
        StopFire();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator CorFire()
    {
        if (Time.realtimeSinceStartup - lastFireTime < 1f / playerShip.GetShipData().fireRate)
            yield return new WaitForSeconds((1f / playerShip.GetShipData().fireRate) - (Time.realtimeSinceStartup - lastFireTime));
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(1f / playerShip.GetShipData().fireRate);
        }
    }
    private void Fire()
    {
        lastFireTime = Time.realtimeSinceStartup;

        Bullet bullet;
        if (bulletPool.HasItem())
        {
            bullet = bulletPool.Get();
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet.gameObject.SetActive(true);
        }
        else
            bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        bullet.SetWeapon(this);
        bullet.Shoot(firePoint.up , playerShip.GetShipData().bulletForce);


        SoundManager.Singelton.PlayActionClip(bulletSfx);
    }
    public void BackBulletToPull(Bullet bullet)
    {
        bulletPool.Add(bullet);
    }
}