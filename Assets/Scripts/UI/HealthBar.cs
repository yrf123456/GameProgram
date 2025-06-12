using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform target; // The character or enemy to follow
    public Transform barFill; // Foreground sprite of the health bar
    public Vector3 offset = new Vector3(-4.6f, 1.5f, 0); // Position offset

    private float maxWidth;

    // Set the health bar based on current and maximum health
    public void SetHealth(float current, float max)
    {
        float ratio = Mathf.Clamp01(current / max);
        Vector3 scale = barFill.localScale;
        scale.x = 0.2f * ratio;
        barFill.localScale = scale;
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
