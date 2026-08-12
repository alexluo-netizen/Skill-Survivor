using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerExperience playerExperience;
    [SerializeField] private PlayerAutoAttack playerAutoAttack;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject upgradePanel;

    [Header("Buttons")]
    [SerializeField] private Button[] upgradeButtons =
        new Button[3];

    [Header("Selection")]
    [SerializeField] private float selectedScale = 1.08f;

    private Vector3[] normalScales;
    private int selectedIndex = 1;
    private bool panelIsOpen;

    private sealed class UpgradeOption
    {
        public string Label { get; }
        public Action Apply { get; }

        public UpgradeOption(string label, Action apply)
        {
            Label = label;
            Apply = apply;
        }
    }

    private void Awake()
    {
        normalScales =
            new Vector3[upgradeButtons.Length];

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            normalScales[i] =
                upgradeButtons[i].transform.localScale;
        }

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

    private void Update()
    {
        if (!panelIsOpen || Keyboard.current == null)
            return;

        Keyboard keyboard = Keyboard.current;

        bool tabPressed =
            keyboard.tabKey.wasPressedThisFrame;

        bool shiftPressed =
            keyboard.leftShiftKey.isPressed ||
            keyboard.rightShiftKey.isPressed;

        bool previousPressed =
            keyboard.wKey.wasPressedThisFrame ||
            keyboard.upArrowKey.wasPressedThisFrame ||
            keyboard.aKey.wasPressedThisFrame ||
            keyboard.leftArrowKey.wasPressedThisFrame ||
            (tabPressed && shiftPressed);

        bool nextPressed =
            keyboard.sKey.wasPressedThisFrame ||
            keyboard.downArrowKey.wasPressedThisFrame ||
            keyboard.dKey.wasPressedThisFrame ||
            keyboard.rightArrowKey.wasPressedThisFrame ||
            (tabPressed && !shiftPressed);

        if (previousPressed)
        {
            SelectButton(selectedIndex - 1);
        }
        else if (nextPressed)
        {
            SelectButton(selectedIndex + 1);
        }

        bool confirmPressed =
            keyboard.enterKey.wasPressedThisFrame ||
            keyboard.numpadEnterKey.wasPressedThisFrame ||
            keyboard.spaceKey.wasPressedThisFrame;

        if (confirmPressed)
        {
            upgradeButtons[selectedIndex]
                .onClick.Invoke();
        }
    }

    private void ShowUpgradePanel(int newLevel)
    {
        ConfigureRandomButtons();

        upgradePanel.SetActive(true);
        panelIsOpen = true;

        // 防止角色移动脚本同时读取菜单按键
        playerMovement.enabled = false;

        Time.timeScale = 0f;

        // 默认选择中间按钮
        SelectButton(1);
    }

    private void ConfigureRandomButtons()
    {
        List<UpgradeOption> options =
            CreateUpgradePool();

        Shuffle(options);

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            UpgradeOption selectedOption = options[i];
            Button button = upgradeButtons[i];

            TMP_Text buttonText =
                button.GetComponentInChildren<TMP_Text>(true);

            if (buttonText != null)
            {
                buttonText.text = selectedOption.Label;
            }

            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(
                () => ChooseUpgrade(selectedOption)
            );
        }
    }

    private List<UpgradeOption> CreateUpgradePool()
    {
        return new List<UpgradeOption>
        {
            new UpgradeOption(
                "DAMAGE +1",
                playerAutoAttack.UpgradeDamage
            ),

            new UpgradeOption(
                "FIRE RATE x1.25",
                playerAutoAttack.UpgradeAttackSpeed
            ),

            new UpgradeOption(
                "MULTISHOT +2",
                playerAutoAttack.UpgradeProjectileCount
            ),

            new UpgradeOption(
                "PIERCING +1",
                playerAutoAttack.UpgradePiercing
            ),

            new UpgradeOption(
                "PROJECTILE SIZE x1.2 ",
                playerAutoAttack.UpgradeProjectileSize
            ),

            new UpgradeOption(
                "MOVE SPEED +1",
                playerMovement.UpgradeMoveSpeed
            )
        };
    }

    private void Shuffle(List<UpgradeOption> options)
    {
        for (int i = options.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(
                0,
                i + 1
            );

            UpgradeOption temporary = options[i];
            options[i] = options[randomIndex];
            options[randomIndex] = temporary;
        }
    }

    private void ChooseUpgrade(UpgradeOption option)
    {
        // 防止同一帧重复选择
        if (!panelIsOpen)
            return;

        panelIsOpen = false;

        option.Apply();
        ClosePanel();
    }

    public void SelectButton(int newIndex)
    {
        if (!panelIsOpen)
            return;

        int buttonCount = upgradeButtons.Length;

        selectedIndex =
            (newIndex + buttonCount) % buttonCount;

        for (int i = 0; i < buttonCount; i++)
        {
            Vector3 targetScale = normalScales[i];

            if (i == selectedIndex)
            {
                targetScale *= selectedScale;
            }

            upgradeButtons[i].transform.localScale =
                targetScale;
        }
    }

    private void ClosePanel()
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].transform.localScale =
                normalScales[i];
        }

        upgradePanel.SetActive(false);
        playerMovement.enabled = true;
        Time.timeScale = 1f;
    }
}