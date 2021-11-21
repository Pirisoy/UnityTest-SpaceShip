using UnityEngine;

[CreateAssetMenu(fileName = "New Ship", menuName = "Objects/Weapon")]
public class WeaponData : ScriptableObject
{
    public float fireRate = 1;
    public float bulletForce = 10;
}