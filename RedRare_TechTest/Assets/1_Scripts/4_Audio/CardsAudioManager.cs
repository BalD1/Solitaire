using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CardsAudioManager : EventHandlerMono
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] cardGrabAudioClips;
    [SerializeField] private AudioClip cardsSlideAudio;
    [SerializeField] private AudioClip winSoundClip;

    private void Reset()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    protected override void EventRegister()
    {
        SolitaireManagerEventsHandler.OnStartGame += PlayCardsSlideAudio;
        SolitaireManagerEventsHandler.OnWin += PlayWinSound;
        CardGrabManagerEventsHandler.OnGrabCard += PlayGrabAudio;
        CardGrabManagerEventsHandler.OnPlacedCard += PlayGrabAudio;
    }

    protected override void EventUnRegister()
    {
        SolitaireManagerEventsHandler.OnStartGame -= PlayCardsSlideAudio;
        SolitaireManagerEventsHandler.OnWin -= PlayWinSound;
        CardGrabManagerEventsHandler.OnGrabCard -= PlayGrabAudio;
        CardGrabManagerEventsHandler.OnPlacedCard -= PlayGrabAudio;
    }

    private void PlayCardsSlideAudio()
    {
        audioSource.PlayOneShot(cardsSlideAudio);
    }

    private void PlayGrabAudio(List<Card> card)
    {
        audioSource.PlayOneShot(cardGrabAudioClips.RandomElement());
    }
    private void PlayGrabAudio(Card card, CardReceiver receiver) => PlayGrabAudio(new List<Card>());

    private void PlayWinSound()
    {
        audioSource.PlayOneShot(winSoundClip);
    }
}
