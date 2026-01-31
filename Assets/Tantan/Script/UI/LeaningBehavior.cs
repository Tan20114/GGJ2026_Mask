using UnityEngine;

public class LeaningBehavior : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] float minAngle = 20f;
    [SerializeField] float maxAngle = 40f;
    [SerializeField] float maxMove = 40f;
    [SerializeField] float maxDistance = 5f;
    [SerializeField] float smooth = 10f;
    [SerializeField] bool isRotateX = true;
    float currentY;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }


    void Update()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector3 posOffset = PositionCalculator(mouseWorld);
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            startPos + posOffset,
            Time.deltaTime * smooth
        );

        if (!isRotateX) return;

        float t = TCalculatorX(mouseWorld.x);
        Debug.Log(t);

        // Lerp ANGLE, not quaternion
        currentY = Mathf.Lerp(minAngle, maxAngle, t);

        transform.rotation = Quaternion.Euler(0f, currentY, 0f);
    }

    Vector3 PositionCalculator(Vector3 mouseWorld)
    {
        float x = Mathf.Clamp(mouseWorld.x / maxDistance, -1f, 1f);
        float y = Mathf.Clamp(mouseWorld.y / maxDistance, -1f, 1f);

        return new Vector3(x, y, 0f) * maxMove;
    }


    float TCalculatorX(float mouseX)
    {
        float normalized = Mathf.Clamp(mouseX / maxDistance, -1f, 1f);
        return normalized * 0.5f + 0.5f;
    }
}
