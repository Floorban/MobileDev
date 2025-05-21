using System.Linq;
using Unity.VisualScripting;
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
            //hasActivated = true;

            GameObject weapon = wm.AddWeapon(weaponPrefab);
            spawnedWeapons.Add(weapon);

            var controller = weapon.GetComponent<SwingAttack>();
            if (controller != null)
            {
                controller.Setup(this);
            }
        }
        else
        {
            foreach (var weapon in spawnedWeapons)
            {
                if (!weapon) continue;

                var swing = weapon.GetComponent<SwingAttack>();
                if (swing != null)
                {
                    swing.DoSwing();
                }
            }
        }
    }

    public override void Deactivate(WeaponManager wm)
    {
        foreach (var weapon in spawnedWeapons)
        {
            if (weapon != null)
                wm.RemoveWeapon(weapon);
        }

        spawnedWeapons.Clear();
        hasActivated = false;
    }
}
