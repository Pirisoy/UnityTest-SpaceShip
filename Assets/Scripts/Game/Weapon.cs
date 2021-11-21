using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class Weapon : MonoBehaviour
{
    //SerializeFields
    [SerializeField] protected Bullet bulletPrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected AudioClip bulletSfx;
    [SerializeField] protected WeaponData weaponData;


    private float lastFireTime;
    protected ObjectPool<Bullet> bulletPool;
    private List<Bullet> allBullets;

    private void Awake()
    {
        bulletPool = new ObjectPool<Bullet>();
        allBullets = new List<Bullet>();
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
        if (Time.realtimeSinceStartup - lastFireTime < 1f / weaponData.fireRate)
            yield return new WaitForSeconds((1f / weaponData.fireRate) - (Time.realtimeSinceStartup - lastFireTime));
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(1f / weaponData.fireRate);
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
        {
            bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            allBullets.Add(bullet);
        }

        bullet.SetWeapon(this);
        bullet.Shoot(firePoint.up, weaponData.bulletForce);


        SoundManager.Singelton.PlayActionClip(bulletSfx);
    }
    public void BackBulletToPull(Bullet bullet)
    {
        bulletPool.Add(bullet);
    }
    private void OnDisable()
    {
        StopFire();
        ClearAllBullets();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void ClearAllBullets()
    {
        bulletPool = new ObjectPool<Bullet>();

        foreach (var b in allBullets)
        {
            if (b == null)
                continue;
            b.gameObject.SetActive(false);
            BackBulletToPull(b);
        }
    }
}