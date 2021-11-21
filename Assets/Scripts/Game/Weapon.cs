using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class Weapon : MonoBehaviour
{
    //SerializeFields
    [SerializeField] protected Bullet bulletPrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected AudioClip bulletSfx;


    private float lastFireTime;
    protected ObjectPool<Bullet> bulletPool;

    private Ship ship;

    private void Awake()
    {
        ship = GetComponent<PlayerShip>();
        bulletPool = new ObjectPool<Bullet>();
    }

    public void StartFire()
    {
        StartCoroutine(nameof(CorFire));
    }
    public void StopFire()
    {
        StopCoroutine(nameof(CorFire));
    }

    protected IEnumerator CorFire()
    {
        if (Time.realtimeSinceStartup - lastFireTime < 1f / ship.GetShipData().fireRate)
            yield return new WaitForSeconds((1f / ship.GetShipData().fireRate) - (Time.realtimeSinceStartup - lastFireTime));
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(1f / ship.GetShipData().fireRate);
        }
    }

    protected void Fire()
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
        bullet.Shoot(firePoint.up, ship.GetShipData().bulletForce);


        SoundManager.Singelton.PlayActionClip(bulletSfx);
    }
    public void BackBulletToPull(Bullet bullet)
    {
        bulletPool.Add(bullet);
    }
    private void OnDisable()
    {
        StopFire();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}