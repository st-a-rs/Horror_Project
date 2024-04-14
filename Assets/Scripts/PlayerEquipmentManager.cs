using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerEquipmentManager : MonoBehaviour
{
    AnimatorManager animatorManager;
    WeaponLoaderSlot weaponLoaderSlot;

    [Header("Current Equipment")]
    public WeaponItem weapon;
    public WeaponAnimatorManager weaponAnimator;
    RightHandIKTarget rightHandIK;
    LeftHandIKTarget leftHandIK;
    //public SubWeaponItem subWeapon; //knife, stun grenade, etc

    private void Awake()
    {
        animatorManager =GetComponent<AnimatorManager>();
        LoadWeaponLoaderSlots();
    }

    private void Start()
    {
        LoadCurrentWeapon();
    }

    private void LoadWeaponLoaderSlots()
    {
        //back slot
        //hip slot
        weaponLoaderSlot = GetComponentInChildren<WeaponLoaderSlot>();
    }
    private void LoadCurrentWeapon()
    {
        weaponLoaderSlot.LoadWeaponModel(weapon);
        animatorManager.animator.runtimeAnimatorController = weapon.weaponAnimator;
        weaponAnimator = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<WeaponAnimatorManager>();
        rightHandIK = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();
        leftHandIK = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
        animatorManager.AssignHandIK(rightHandIK, leftHandIK);
    }
}