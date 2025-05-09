using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    public GameObject spellPrefab;
    public Transform castPoint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CastSpell();
        }
    }

    void CastSpell()
    {
        Instantiate(spellPrefab, castPoint.position, castPoint.rotation);
    }
}
