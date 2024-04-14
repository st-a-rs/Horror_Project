using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimatorManager : MonoBehaviour
{
    Animator weaponAnimator;


    [Header("Weapon FX")]
    public GameObject weaponMuzzleFlashFX; // Flash FX when weapon is fired
    //public GameObject weaponBulletCaseFX; // Bullet case FX when weapon is fired

    [Header("Weapon FX Transforms")] 
    public Transform weaponMuzzleFlashTransform; // Location of Muzzle Flash FX
   // public Transform weaponBulletCaseTransform; // Location of bullet case
    private void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();
    }

    public void ShootWeapon(PlayerCamera playerCamera)
    {
        // Animate the weapon
        weaponAnimator.Play("Shoot");
        

        //SMOKE FX
        GameObject muzzleFlash = Instantiate(weaponMuzzleFlashFX, weaponMuzzleFlashTransform);
        muzzleFlash.transform.parent = null;

        //BULLET CASE
        //GameObject bulletCase = Instantiate(weaponBulletCaseFX, weaponBulletCaseTransform);
        //bulletCase.transform.parent = null;

        //SHOOT SOMETHING
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.cameraObject.transform.position, playerCamera.cameraObject.transform.forward, out hit))
        {
            Debug.Log(hit.transform.gameObject.name);
        }
    }
}