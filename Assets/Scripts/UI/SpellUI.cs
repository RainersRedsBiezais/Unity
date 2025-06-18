using UnityEngine;
using UnityEngine.UI;

public class SpellUI : MonoBehaviour
{
    [Header("Spell Slot UI")]
    public Image[] spellSlotImages; // Array of UI images for spell slots
    public Color selectedColor = Color.white; // Color for selected spell
    public Color unselectedColor = new Color(0.5f, 0.5f, 0.5f, 0.7f); // Color for unselected spells
    
    [Header("Spell Icons")]
    public Sprite fireballIcon; // Assign fireball icon sprite
    public Sprite flamethrowerIcon; // Assign flamethrower icon sprite
    
    private SpellCaster spellCaster;
    private int currentSpellIndex = 0;

    void Start()
    {
        // Find the SpellCaster in the scene
        spellCaster = FindObjectOfType<SpellCaster>();
        
        if (spellCaster == null)
        {
            Debug.LogError("SpellCaster not found in scene!");
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
        // Check if spell selection changed
        if (spellCaster != null)
        {
            // Get current spell index from SpellCaster (you'll need to make this public)
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
        for (int i = 0; i < spellSlotImages.Length; i++)
        {
            if (spellSlotImages[i] != null)
            {
                // Highlight selected spell, dim others
                spellSlotImages[i].color = (i == currentSpellIndex) ? selectedColor : unselectedColor;
            }
        }
    }
} 