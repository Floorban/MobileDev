using UnityEngine;

public class SwingMeleeWeapon : AttackBehavior
{
    public float swingAngle = 90f;
    public float swingDuration = 0.3f;
    public float windupAngle = 20f;
    public float windupDuration = 0.3f;
    public float returnDuration = 0.15f;

    public override void Activate(WeaponManager wm)
    {
        if (!hasActivated)
        {
            hasActivated = true;

            spawnedWeapon = wm.AddWeapon(weaponPrefab);
            var controller = spawnedWeapon.GetComponent<SwingAttack>();
            if (controller != null)
                controller.Setup(this);
        }
        else if (spawnedWeapon != null)
        {
            var swing = spawnedWeapon.GetComponent<SwingAttack>();
            if (swing != null)
            {
                swing.DoSwing();
            }
        }
    }

    public override void Deactivate(WeaponManager wm)
    {
        if (!spawnedWeapon) return;

        wm.RemoveWeapon(spawnedWeapon);
        spawnedWeapon = null;
        hasActivated = false;
    }
}
