using UnityEngine;

[CreateAssetMenu(fileName = "SoundsCollectionSO", menuName = "Scriptable Objects/SoundsCollectionSO")]
public class SoundsCollectionSO : ScriptableObject
{
    public SoundSO[] GunShoot;
    public SoundSO[] Jump;
    public SoundSO[] Splat;
}
