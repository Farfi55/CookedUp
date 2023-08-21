using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "new AudioClipRef", menuName = "CookedUp/AudioClipRefsSO", order = 20)]
public class AudioClipRefsSO : ScriptableObject {
    [SerializeField] private AudioClip[] chop;
    [SerializeField] private AudioClip[] deliveryFail;
    [SerializeField] private AudioClip[] deliverySuccess;
    [SerializeField] private AudioClip[] footstep;
    [SerializeField] private AudioClip[] objectDrop;
    [SerializeField] private AudioClip[] objectPickup;
    [SerializeField] private AudioClip[] objectDestroy;
    [SerializeField] private AudioClip[] stoveSizzle;
    [SerializeField] private AudioClip[] trash;
    [SerializeField] private AudioClip[] warning;
    [SerializeField] private AudioClip[] clockTick;
    [SerializeField] private AudioClip[] countdown;
    [SerializeField] private AudioClip[] countdownEnd;

    public AudioClip[] Chop => chop;
    public AudioClip[] DeliveryFail => deliveryFail;
    public AudioClip[] DeliverySuccess => deliverySuccess;
    public AudioClip[] Footstep => footstep;
    public AudioClip[] ObjectDrop => objectDrop;
    public AudioClip[] ObjectPickup => objectPickup;
    public AudioClip[] StoveSizzle => stoveSizzle;
    public AudioClip[] Trash => trash;
    public AudioClip[] Warning => warning;
    public AudioClip[] ClockTick => clockTick;
    public AudioClip[] Countdown => countdown;
    public AudioClip[] CountdownEnd => countdownEnd;
    
    public AudioClip[] ObjectDestroy => objectDestroy;
    
}
