using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class WeaponStats : ScriptableObject
{
    public GameObject Model;
    public int Damage;
    public int EffectiveRange;
    public float RateOfFire;
    public int CurrentAmmo;
    public int MaxAmmo;

    public ParticleSystem HitEffect;
    public AudioClip[] GunSound;
    public float GunVolume;
    
}
