using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    public float floatSpeed = 50f;
    public float duration = 1f;

    private TextMeshProUGUI text;
    private float timer;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string value)
    {
        if (text == null) text = GetComponent<TextMeshProUGUI>();
        text.text = value;
        // Optional: Debug log to confirm it's being set
        // Debug.Log("DamageNumber set to: " + value);
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer > duration)
        {
            Destroy(gameObject);
        }
    }
}