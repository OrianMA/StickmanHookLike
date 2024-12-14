using Unity.Cinemachine;
using UnityEngine;

public class BackgroundFitToCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    void Update()
    {
        // Get the orthographic size and aspect ratio
        float aspectRatio = (float)Screen.width / Screen.height;

        // Calculate the size of the square
        float height = _camera.Lens.OrthographicSize * 2f; // Total height of camera view
        float width = height * aspectRatio;  // Total width of camera view
        float size = Mathf.Min(width, height); // Ensure it fits within both dimensions

        // Adjust the scale of the square
        transform.localScale = new Vector3(width * 1.25f, height * 1.25f, 1f);
    }
}
