using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private PlayerExperience playerExperience;
    [SerializeField] private PlayerAutoAttack playerAutoAttack;
    // [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject upgradePanel;

    private void Awake()
    {
        upgradePanel.SetActive(false);
    }

    private void OnEnable()
    {
        playerExperience.LeveledUp += ShowUpgradePanel;
    }

    private void OnDisable()
    {
        playerExperience.LeveledUp -= ShowUpgradePanel;
    }

    private void ShowUpgradePanel(int newLevel)
    {
        upgradePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ChooseDamage()
    {
        playerAutoAttack.UpgradeDamage();
        ClosePanel();
    }

    public void ChooseAttackSpeed()
    {
        playerAutoAttack.UpgradeAttackSpeed();
        ClosePanel();
    }

    // public void ChooseMoveSpeed()
    // {
    //     playerMovement.UpgradeMoveSpeed();
    //     ClosePanel();
    // }

    public void ChooseMultishot()
    {
        playerAutoAttack.UpgradeProjectileCount();
        ClosePanel();
    }

    private void ClosePanel()
    {
        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}