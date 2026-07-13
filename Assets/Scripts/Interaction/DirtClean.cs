using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Rendering.HighDefinition;

public class DirtClean : MonoBehaviour
{
    [Header("Scrubbing Settings")]
    [Tooltip("The speed at which the dirt is scrubbed away.")]
    [SerializeField]
    public float scrubSpeed = 0.000000001f;


    [Header("Particle System")]
    [Tooltip("The particle system that plays when scrubbing dirt.")]
    [SerializeField]
    public ParticleSystem scrubParticles;

    private ParticleSystem activeParticles;

    [Header("UI Elements")]
    [Tooltip("The text element that displays status messages.")]
    [SerializeField]
    public TMP_Text statusText;

    [Header("References")]
    [Tooltip("Reference to the daily tasks script.")]
    public DailyTasks dirtCount;
    public ActivateCleaning mopStatus;


    void Start()
    {
        mopStatus = GetComponent<ActivateCleaning>();
        dirtCount = GetComponent<DailyTasks>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button is held down
        if (Input.GetMouseButton(0))
        {
            // Handle scrubbing logic
            HandleScrubbing();
        }
        else
        {
            // Stop the particle system if the mouse button is not held down
            StopParticles();
        }

        // Stop the particle system when the mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            StopParticles();
        }
    }

    public void HandleScrubbing()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Dirt"))
            {
                // Check if the player has taken the mop
                if (ActivateCleaning.mopTaken)
                {
                   // Manage the particle system's playback and position
                    ManageParticles(hit.point);

                    // Get the transform of the dirt object
                    DecalProjector decal = hit.collider.GetComponent<DecalProjector>();

                    if (decal != null)
                    {
                        // Reduce the scale of the dirt object overtime
                        decal.fadeFactor -= scrubSpeed * Time.deltaTime;

                        if (decal.fadeFactor <= 0.2)
                        {
                            StopParticles();
                            Destroy(hit.collider.gameObject);
                            dirtCount.UpdateDirtCount();
                        }
                    } 
                }
                else
                {
                    statusText.text = "You need a mop to clean the dirt!";
                    StartCoroutine(WaitForSeconds(2));
                    return;
                }
            }
            else
            {
                StopParticles();
            }
        }
    }

    void ManageParticles(Vector3 hitpoint)
    {
        if (activeParticles == null && scrubParticles != null)
        {
            activeParticles = Instantiate(scrubParticles, hitpoint, Quaternion.identity);
        }

        if (activeParticles != null)
        {
            activeParticles.transform.position = hitpoint;

            if (!activeParticles.isPlaying)
            {
                activeParticles.Play();
            }
        }
    }

    void StopParticles()
    {
        activeParticles.Stop();
        Destroy(activeParticles.gameObject, 1f); // Destroy the particle system after 1 second to allow it to finish playing
    }

    IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        statusText.text = "";
    }
}