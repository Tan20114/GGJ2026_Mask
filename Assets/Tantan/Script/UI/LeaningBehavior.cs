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

        float dir = Mathf.Sign(mouseWorld.x);

        // No rotation near center
        if (dir == 0f)
            return;

        float t = TCalculator(mouseWorld.x);

        float targetAngle = Mathf.Lerp(minAngle, maxAngle, t) * dir;

        // Lerp ANGLE, not quaternion
        currentY = Mathf.Lerp(currentY, targetAngle, Time.deltaTime * smooth);

        transform.rotation = Quaternion.Euler(0f, currentY, 0f);
    }

    float TCalculator(float mouseX)
    {
        float distance = Mathf.Abs(mouseX);
        return Mathf.Clamp01(distance / maxDistance);
    }

}
