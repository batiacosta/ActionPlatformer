using UnityEngine;

[CreateAssetMenu(fileName = "SoundsCollectionSO", menuName = "Scriptable Objects/SoundsCollectionSO")]
public class SoundsCollectionSO : ScriptableObject
{
    [Header("Music")]
    public SoundSO[] FightMusic;
    public SoundSO[] DiscoBallMusic;
    
    [Header("SFX")]
    public SoundSO[] GunShoot;
    public SoundSO[] Jump;
    public SoundSO[] Splat;
}
