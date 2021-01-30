using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( AudioSource ) )]
[RequireComponent( typeof( AudioSource ) )]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    AudioClip[] footstepAudio;
    [SerializeField]
    AudioClip[] swordwoosh;
    [SerializeField]
    float pitchRange = 0.1f;

    AudioSource feetstepAudioSource;
    AudioSource swordwooshAudioSource;

    private void Start() {
        feetstepAudioSource = GetComponents<AudioSource>( )[0];
        swordwooshAudioSource = GetComponents<AudioSource>( )[1];

        feetstepAudioSource.volume = 0.05f;
        swordwooshAudioSource.volume = 0.2f;
    }

    /// <summary>
    /// ランダムにオーディオを再生する
    /// </summary>
    public void PlayFootstepAudio() {
        feetstepAudioSource.pitch = 1.0f + Random.Range( -pitchRange, pitchRange );
        feetstepAudioSource.PlayOneShot( footstepAudio[Random.Range( 0, footstepAudio.Length )] );
    }

    public void PlaySwordwooshAudio() {
        swordwooshAudioSource.pitch = 1.0f + Random.Range( -pitchRange, pitchRange );
        swordwooshAudioSource.PlayOneShot( swordwoosh[Random.Range( 0, swordwoosh.Length )] );
    }
}
