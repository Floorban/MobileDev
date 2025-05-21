using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public float radius = 2f;
    public float speed = 90f;
    [SerializeField] private List<GameObject> weapons = new();

    void Update()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (!weapons[i]) continue;

            float angle = (360f / weapons.Count) * i + Time.time * speed;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;
            weapons[i].transform.position = transform.position + offset;
        }
    }

    public GameObject AddWeapon(GameObject weaponPrefab)
    {
        GameObject w = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        w.transform.SetParent(gameObject.transform);
        weapons.Add(w);
        return w;
    }
    public void RemoveWeapon(GameObject target)
    {
        Destroy(target);
        weapons.Remove(target);
    }
}
