using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PileAudio : EventHandlerMono
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip onDrawClip;
    [SerializeField] private AudioClip onResetClip;

    [SerializeField] private Pile owner;

    private void Reset()
    {
        audioSource = this.GetComponent<AudioSource>();
        owner = this.GetComponentInParent<Pile>();
    }

    protected override void EventRegister()
    {
        owner.onDraw += PlayDrawAudio;
        owner.onReset += PlayResetAudio;
    }

    protected override void EventUnRegister()
    {
        owner.onDraw -= PlayDrawAudio;
        owner.onReset -= PlayResetAudio;
    }

    private void PlayDrawAudio()
    {
        audioSource.PlayOneShot(onDrawClip);
    }

    private void PlayResetAudio()
    {
        audioSource.PlayOneShot(onResetClip);
    }
}
