using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    public AudioSource ShootingChannel;

    public AudioSource radioTalk;

    public AudioSource footStep;

    public AudioSource lockedDoor;

    public AudioSource enemyAttack;
    
    public AudioSource Theme;

    public AudioSource shatterSound;

    public AudioSource hostageSound;
    public AudioClip mainGunShot;
    public AudioClip sniperShot;

    public AudioSource reloadingSoundMainGun;
    public AudioSource reloadingSoundSniper;

    public AudioSource emptyMagazineSoundMainGun;
    private void Awake()
    {
        DontDestroyOnLoad(Theme);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void PlayShootingSound(WeaponModel weapon )
    {
       switch(weapon)
        {
            case Weapon.WeaponModel.mainGun:
                ShootingChannel.PlayOneShot(mainGunShot);
                break;
            case Weapon.WeaponModel.Sniper:
                ShootingChannel.PlayOneShot(sniperShot);
                break;
        }
    }
    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.mainGun:
                reloadingSoundMainGun.Play();
                break;
            case Weapon.WeaponModel.Sniper:
                reloadingSoundSniper.Play();
                break;
        }
    }
}
