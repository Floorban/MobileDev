using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/SwingMeleeStats")]
public class RangedWeapon : AttackStats
{
    public GameObject projectilePrefab;

    public float recoilAngle = -15f;
    public float recoilDuration = 0.05f;
    public float returnDuration = 0.1f;
    public override void Activate(WeaponManager p)
    {
        GameObject weapon = p.AddWeapon(weaponPrefab);
        spawnedWeapons.Add(weapon);

        var controller = weapon.GetComponent<RangedAttack>();
        if (controller != null)
            controller.Setup(this);
    }
}
