using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceUI : MonoBehaviour
{
    [SerializeField] private PlayerExperience playerExperience;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Slider experienceBar;

    private void Update()
    {
        levelText.text = $"LV {playerExperience.Level}";

        experienceBar.maxValue =
            playerExperience.ExperienceNeeded;

        experienceBar.value =
            playerExperience.CurrentExperience;
    }
}