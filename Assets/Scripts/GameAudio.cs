using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameAudio : MonoBehaviour
{
    public static GameAudio Instance { get; private set; }

    [Header("Combat")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip enemyDeathClip;

    [Header("Progress")]
    [SerializeField] private AudioClip gemPickupClip;
    [SerializeField] private AudioClip levelUpClip;
    [SerializeField] private AudioClip uiConfirmClip;

    [Header("Game Result")]
    [SerializeField] private AudioClip victoryClip;
    [SerializeField] private AudioClip gameOverClip;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
        audioSource.ignoreListenerPause = true;
    }

    private void PlaySound(
        AudioClip clip,
        float volume = 1f)
    {
        if (clip == null)
            return;

        audioSource.PlayOneShot(clip, volume);
    }

    public void PlayShoot()
    {
        PlaySound(shootClip, 0.35f);
    }

    public void PlayEnemyDeath()
    {
        PlaySound(enemyDeathClip, 0.45f);
    }

    public void PlayGemPickup()
    {
        PlaySound(gemPickupClip, 0.6f);
    }

    public void PlayLevelUp()
    {
        PlaySound(levelUpClip, 0.8f);
    }

    public void PlayUIConfirm()
    {
        PlaySound(uiConfirmClip, 0.7f);
    }

    public void PlayVictory()
    {
        PlaySound(victoryClip, 1f);
    }

    public void PlayGameOver()
    {
        PlaySound(gameOverClip, 1f);
    }
}