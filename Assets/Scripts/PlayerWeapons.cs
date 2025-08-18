using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons;   // assign weapon prefabs here
    private int currentIndex = 0;
    private Weapon currentWeapon;

    [SerializeField] private Transform weaponHolder; // empty GameObject as parent for weapons

    private void Start()
    {
        // Spawn the first weapon
        EquipWeapon(currentIndex);
    }

    private void Update()
    {
        HandleWeaponSwitch();

        if (Input.GetButton("Fire1"))
        {
            currentWeapon.TryAttack();
        }
    }

    private void HandleWeaponSwitch()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            currentIndex++;
            if (currentIndex >= weapons.Length)
                currentIndex = 0;

            EquipWeapon(currentIndex);
        }
        else if (scroll < 0f)
        {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = weapons.Length - 1;

            EquipWeapon(currentIndex);
        }
    }

    private void EquipWeapon(int index)
    {
        // Destroy old weapon if one exists
        if (currentWeapon != null)
            Destroy(currentWeapon.gameObject);

        // Instantiate new weapon as child of weaponHolder
        currentWeapon = Instantiate(weapons[index], weaponHolder);
        currentWeapon.gameObject.SetActive(true);
    }
}