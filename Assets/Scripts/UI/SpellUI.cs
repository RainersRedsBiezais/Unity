using UnityEngine;
using UnityEngine.UI;

public class SpellUI : MonoBehaviour
{
    [Header("Spell Slot UI")]
    public Image[] spellSlotImages; // Array of UI images for spell slots
    public Color selectedColor = Color.white; // Color for selected spell
    public Color unselectedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Color for unselected spells
    
    [Header("Spell Icons")]
    public Sprite fireballIcon; // Assign fireball icon sprite
    public Sprite flamethrowerIcon; // Assign flamethrower icon sprite
    
    private SpellCaster spellCaster;

    void Start()
    {
        spellCaster = FindFirstObjectByType<SpellCaster>();
        if (spellCaster == null)
        {
            Debug.LogError("SpellCaster not found!");
            return;
        }

        // Set up spell icons
        if (spellSlotImages.Length >= 2)
        {
            if (fireballIcon != null) spellSlotImages[0].sprite = fireballIcon;
            if (flamethrowerIcon != null) spellSlotImages[1].sprite = flamethrowerIcon;
        }

        // Initialize UI
        UpdateSpellUI();
    }

    void Update()
    {
        UpdateSpellUI();
    }

    void UpdateSpellUI()
    {
        if (spellCaster == null || spellSlotImages == null) return;

        int currentSpell = spellCaster.GetCurrentSpellIndex();
        
        for (int i = 0; i < spellSlotImages.Length; i++)
        {
            if (spellSlotImages[i] != null)
            {
                spellSlotImages[i].color = i == currentSpell ? selectedColor : unselectedColor;
            }
        }
    }
} 