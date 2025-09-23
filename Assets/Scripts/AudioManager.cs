using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] AudioClip _digEffect;
    [SerializeField] AudioClip _winningEffect;
    [SerializeField] AudioClip _explosionEffect;
    private AudioSource _audioSource;

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayExplosionSFX()
    {
        _audioSource?.PlayOneShot(_explosionEffect);
    }

    public void PlayDigSFX()
    {
        _audioSource?.PlayOneShot(_digEffect);
    }

    public void PlayWinningSFX()
    {
        _audioSource?.PlayOneShot(_winningEffect);
    }
}
