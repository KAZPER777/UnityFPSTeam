using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    public GameObject model;
    [Range(1,10)] public int shootDamage;
    [Range(5, 1000)] public int shootDist;
    [Range(.1f, 2)] public float shootRate;
    [HideInInspector] public int ammoCur;
    [Range(5, 50)] public int ammoMax;

    public ParticleSystem hittEffect;
    public AudioClip[] shootSound;
    [Range(0, 1)] public float shootVol;
}
