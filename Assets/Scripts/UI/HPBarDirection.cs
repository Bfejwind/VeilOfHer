using UnityEngine;

public class HPBarDirection : MonoBehaviour
{
    [SerializeField] private Transform playerCam;
    private void Start()
    {
        if (playerCam == null)
        {
            playerCam = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = playerCam.rotation;
    }
}
