using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellUIManager : MonoBehaviour
{
    [System.Serializable]
    public class SpellSlot
    {
        public Image background;
        public Image icon;
        public TextMeshProUGUI keyText;
        public TextMeshProUGUI spellName;
        public GameObject selectedIndicator;
    }

    [Header("Spell Slots")]
    public SpellSlot[] spellSlots = new SpellSlot[2];

    [Header("Visual Settings")]
    public Color selectedColor = Color.white;
    public Color unselectedColor = new Color(0.5f, 0.5f, 0.5f, 0.7f);
    public Color selectedBackgroundColor = new Color(0.2f, 0.6f, 1f, 0.8f);
    public Color unselectedBackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);

    [Header("Spell Icons")]
    public Sprite fireballIcon;
    public Sprite flamethrowerIcon;

    [Header("Spell Names")]
    public string[] spellNames = { "Fireball", "Flamethrower" };

    private SpellCaster spellCaster;
    private int currentSpellIndex = 0;

    void Start()
    {
        spellCaster = FindFirstObjectByType<SpellCaster>();
        if (spellCaster == null)
        {
            Debug.LogError("SpellCaster not found!");
            return;
        }

        InitializeUI();
    }

    void InitializeUI()
    {
        // Set up spell slots
        for (int i = 0; i < spellSlots.Length; i++)
        {
            if (spellSlots[i] != null)
            {
                // Set spell icons
                if (i == 0 && fireballIcon != null)
                    spellSlots[i].icon.sprite = fireballIcon;
                else if (i == 1 && flamethrowerIcon != null)
                    spellSlots[i].icon.sprite = flamethrowerIcon;

                // Set key text
                if (spellSlots[i].keyText != null)
                    spellSlots[i].keyText.text = (i + 1).ToString();

                // Set spell names
                if (spellSlots[i].spellName != null && i < spellNames.Length)
                    spellSlots[i].spellName.text = spellNames[i];
            }
        }

        UpdateSpellUI();
    }

    void Update()
    {
        if (spellCaster != null)
        {
            int newSpellIndex = spellCaster.GetCurrentSpellIndex();
            
            if (newSpellIndex != currentSpellIndex)
            {
                currentSpellIndex = newSpellIndex;
                UpdateSpellUI();
            }
        }
    }

    void UpdateSpellUI()
    {
        if (spellCaster == null || spellSlots == null) return;

        int currentSpell = spellCaster.GetCurrentSpellIndex();
        
        for (int i = 0; i < spellSlots.Length; i++)
        {
            if (spellSlots[i] != null)
            {
                bool isSelected = (i == currentSpell);
                
                // Update background color
                if (spellSlots[i].background != null)
                    spellSlots[i].background.color = isSelected ? selectedBackgroundColor : unselectedBackgroundColor;
                
                // Update icon color
                if (spellSlots[i].icon != null)
                    spellSlots[i].icon.color = isSelected ? selectedColor : unselectedColor;
                
                // Update text color
                if (spellSlots[i].spellName != null)
                    spellSlots[i].spellName.color = isSelected ? selectedColor : unselectedColor;
                
                if (spellSlots[i].keyText != null)
                    spellSlots[i].keyText.color = isSelected ? selectedColor : unselectedColor;
                
                // Show/hide selection indicator
                if (spellSlots[i].selectedIndicator != null)
                    spellSlots[i].selectedIndicator.SetActive(isSelected);
            }
        }
    }
} 