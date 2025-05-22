using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/SwingMeleeStats")]
public class SwingMeleeWeapon : AttackStats
{
    public float swingAngle = 90f;
    public float swingDuration = 0.3f;
    public float windupAngle = 20f;
    public float windupDuration = 0.3f;
    public float returnDuration = 0.15f;

    public override void Activate(WeaponManager p)
    {
        GameObject weapon = p.AddWeapon(weaponPrefab);
        spawnedWeapons.Add(weapon);

        var controller = weapon.GetComponent<SwingAttack>();
        if (controller != null)
            controller.Setup(this);
    }
}
