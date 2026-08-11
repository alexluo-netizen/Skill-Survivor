using System;
using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [SerializeField] private int startingExperienceNeeded = 5;
    [SerializeField] private int increasePerLevel = 3;

    public int Level { get; private set; } = 1;
    public int CurrentExperience { get; private set; }

    public int ExperienceNeeded =>
        startingExperienceNeeded + (Level - 1) * increasePerLevel;

    public event Action<int> LeveledUp;

    public void AddExperience(int amount)
    {
        CurrentExperience += amount;

        while (CurrentExperience >= ExperienceNeeded)
        {
            int requiredExperience = ExperienceNeeded;

            CurrentExperience -= requiredExperience;
            Level++;

            Debug.Log($"Level Up! Current Level: {Level}");

            LeveledUp?.Invoke(Level);
        }
    }
}