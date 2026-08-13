using UnityEngine;

public class ExperienceGem : MonoBehaviour
{
    [SerializeField] private int experienceAmount = 1;

    private bool collected;

    public void Initialize(int newAmount)
    {
        experienceAmount = Mathf.Max(1, newAmount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
            return;

        if (!other.TryGetComponent(
                out PlayerExperience playerExperience))
        {
            return;
        }

        collected = true;

        GameAudio.Instance?.PlayGemPickup();

        playerExperience.AddExperience(experienceAmount);
        Destroy(gameObject);
    }
}