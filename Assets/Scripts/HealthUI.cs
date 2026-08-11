using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TMP_Text healthText;

    private void Update()
    {
        healthText.text =
            $"HP: {playerHealth.CurrentHealth}/{playerHealth.MaxHealth}";
    }
}