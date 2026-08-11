using UnityEngine;

public class ExperienceGem : MonoBehaviour
{
    [SerializeField] private int experienceAmount = 1;

    public void Initialize(int newAmount)
    {
        experienceAmount = Mathf.Max(1, newAmount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(
                out PlayerExperience playerExperience))
        {
            return;
        }

        playerExperience.AddExperience(experienceAmount);
        Destroy(gameObject);
    }
}