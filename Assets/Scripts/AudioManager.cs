using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] AudioClip _digEffect;
    [SerializeField] AudioClip _winningEffect;
    [SerializeField] AudioClip _explosionEffect;
    private AudioSource _audioSource;

    private void Start()
    {
        EventManager.OnTileJumped += TileJumpSound;
        _audioSource = GetComponent<AudioSource>();
    }

    private void TileJumpSound(Tile obj)
    {
        if (!obj.Data.IsRevealed)
        {
            switch (obj.Data.TiType)
            {
                case TileData.TileType.Mine:
                    _audioSource?.PlayOneShot(_explosionEffect);
                    break;
                default:
                    _audioSource?.PlayOneShot(_digEffect);
                    break;
            }
        }
    }

    public void PlayExplosionSFX()
    {
        _audioSource?.PlayOneShot(_explosionEffect);
    }
}
