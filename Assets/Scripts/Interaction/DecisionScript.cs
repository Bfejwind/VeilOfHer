using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Video;
public class DecisionScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputChoice;
    [SerializeField] private GameObject errorMsg;
    [SerializeField] private InputActionReference cmd;
    [SerializeField] private GameObject idealObj;
    [SerializeField] private GameObject realityObj;
    [SerializeField] private VideoPlayer idealVid;
    [SerializeField] private VideoPlayer realityVid;
    private void Start()
    {
        errorMsg.SetActive(false);
        idealVid.loopPointReached += VideoFinished;
        realityVid.loopPointReached += VideoFinished;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (inputChoice.text == "Idealism")
            {
                idealObj.SetActive(true);
                idealVid.Play();
            }
            else if (inputChoice.text == "Reality")
            {
                realityObj.SetActive(true);
                realityVid.Play();

            }
            else
            {
                ErrorMsg();
            }
            
        }
    }
    private IEnumerator ErrorMsg()
    {
        errorMsg.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        errorMsg.SetActive(false);
    }
    private void VideoFinished(VideoPlayer vp)
    {
        GameManager.Instance.LoadScene("Main Menu");
    }
}
