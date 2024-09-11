using UnityEngine;
using System.Collections;

[System.Serializable]
public class StationDoor
{
    public enum Direction { up, right, forward }
    public Transform doorTransform; // The transform of the door
    public DoorSlide slide; // The slide configuration
    public float openRatio = 0; // 0 = fully closed, 1 = fully open

    private Coroutine currentAction; // Reference to the current coroutine

    // Method to open the door
    public void OpenDoor(MonoBehaviour owner)
    {
        if (doorTransform != null)
        {
            if (currentAction != null)
            {
                owner.StopCoroutine(currentAction);
            }
            currentAction = owner.StartCoroutine(Open(owner));
        }
    }

    // Method to close the door
    public void CloseDoor(MonoBehaviour owner)
    {
        if (doorTransform != null)
        {            
            if (currentAction != null)
            {
                owner.StopCoroutine(currentAction);
            }
            currentAction = owner.StartCoroutine(Close(owner));
        }
    }

    // Coroutine to handle the door opening
    private IEnumerator Open(MonoBehaviour owner)
    {
        Vector3 startPosition = doorTransform.position;
        Vector3 endPosition = CalculateEndPosition(slide.direction, slide.distance);

        Debug.Log($"Opening Door from {startPosition} to {endPosition}");

        float elapsedTime = 0f;
        float totalDuration = slide.duration;

        while (elapsedTime < totalDuration)
        {
            float t = elapsedTime / totalDuration;
            openRatio = Mathf.Clamp01(t); // Normalize ratio

            doorTransform.position = Vector3.Lerp(startPosition, endPosition, openRatio);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Finalize position and ratio
        openRatio = 1f;
        doorTransform.position = endPosition;
        currentAction = null; // Clear the current action when done

        Debug.Log("Door opened");
    }

    // Coroutine to handle the door closing
    private IEnumerator Close(MonoBehaviour owner)
    {
        Vector3 startPosition = doorTransform.position;
        Vector3 endPosition = CalculateEndPosition(slide.direction, -slide.distance);

        Debug.Log($"Closing Door from {startPosition} to {endPosition}");

        float elapsedTime = 0f;
        float totalDuration = slide.duration;

        while (elapsedTime < totalDuration)
        {
            float t = elapsedTime / totalDuration;
            openRatio = Mathf.Clamp01(1 - t); // Inverse ratio for closing

            doorTransform.position = Vector3.Lerp(endPosition,startPosition, openRatio);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Finalize position and ratio
        openRatio = 0f;
        doorTransform.position = endPosition;
        currentAction = null; // Clear the current action when done

        Debug.Log("Door closed");
    }

    // Calculate the end position based on direction
    private Vector3 CalculateEndPosition(Direction direction, float distance)
    {
        Vector3 directionVector = direction switch
        {
            Direction.right => doorTransform.right,
            Direction.forward => doorTransform.forward,
            _ => doorTransform.up, // Default to up direction
        };

        // Log the direction and distance for debugging
        Debug.Log($"Direction Vector: {directionVector}, Distance: {distance}");

        return doorTransform.position + (directionVector * distance);
    }
}

// Define DoorSlide struct
[System.Serializable]
public struct DoorSlide
{
    public float distance; // Distance to move the door
    public float duration; // Duration for the door to move
    public StationDoor.Direction direction; // Direction of movement
}
