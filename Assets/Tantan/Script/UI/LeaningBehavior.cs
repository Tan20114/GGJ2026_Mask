using UnityEngine;

public class LeaningBehavior : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] float minAngle = 20f;
    [SerializeField] float maxAngle = 40f;
    [SerializeField] float maxDistance = 5f;
    [SerializeField] float smooth = 10f;
    float currentY;

    void Update()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        float t = TCalculator(mouseWorld.x);
        Debug.Log(t);

        // Lerp ANGLE, not quaternion
        currentY = Mathf.Lerp(minAngle, maxAngle, t);

        transform.rotation = Quaternion.Euler(0f, currentY, 0f);
    }

    float TCalculator(float mouseX)
    {
        float normalized = Mathf.Clamp(mouseX / maxDistance, -1f, 1f);
        return normalized * 0.5f + 0.5f;
    }

}
