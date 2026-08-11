using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Events;


public class DirtClean : MonoBehaviour
{
    [Header("Scrubbing Settings")]
    [Tooltip("The speed at which the dirt is scrubbed away.")]
    [SerializeField]
    public float scrubSpeed = 0.00000000001f;

    [Header("Particle System")]
    [Tooltip("The particle system that plays when scrubbing dirt.")]
    [SerializeField]
    public ParticleSystem scrubParticles;

    private ParticleSystem activeParticles;

    [Header("UI Elements")]
    [Tooltip("The text element that displays status messages.")]
    [SerializeField]
    public TMP_Text Objectives;

    [Header("References")]
    [Tooltip("Reference to the daily tasks script.")]
    public ActivateCleaning mopStatus;
    public DailyTasks dailyTasks;

    [Header("Dirt Cleaning State")]
    [Tooltip("Indicates whether the dirt is cleaned.")]
    public bool isCleaned = false;


    void Start()
    {
        mopStatus = GetComponent<ActivateCleaning>();
        dailyTasks = Object.FindAnyObjectByType<DailyTasks>();

        if (dailyTasks == null)
        {
            Debug.LogError("DailyTasks script is not assigned in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button is held down
        if (Input.GetMouseButton(0) && ActivateCleaning.mopTaken)
        {
            // Handle scrubbing logic
            HandleScrubbing();
        }
        else
        {
            // Stop the particle system if it's playing and the mouse button is not held down
            if (Input.GetMouseButton(0) && activeParticles != null && activeParticles.isPlaying)
            {
                StopParticles();
            }
        }

        // Stop the particle system when the mouse button is released
        if (Input.GetMouseButtonUp(0) && ActivateCleaning.mopTaken)
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

                        if (decal.fadeFactor <= 0.2f)
                        {
                            if (isCleaned)
                            {
                                return; // Skip the rest of the update if cleaning is already completed  
                            }
                            isCleaned = true; // Mark the dirt as cleaned
                            Destroy(hit.collider.gameObject);
                            dailyTasks.UpdateDirtCount(1); // Update the dirt count in the DailyTasks script
                            StopParticles();  
                        }
                    } 
                }
                else
                {
                    Objectives.text = "You need a mop to clean the dirt!";
                    StartCoroutine(WaitForSeconds(2));
                    return;
                }
            }
            else
            {
                if (activeParticles != null && activeParticles.isPlaying)
                {
                    StopParticles();
                }
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
        if (activeParticles == null)
        {
            return;
        }
        activeParticles.Stop();
        Destroy(activeParticles.gameObject); // Destroy the particle system
    }

    IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Objectives.text = "";
    }
}