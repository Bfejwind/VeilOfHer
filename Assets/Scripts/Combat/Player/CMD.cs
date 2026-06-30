using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CMD : MonoBehaviour
{
    private PlayerInput playerInput;
    public GameObject cmdKeyboard;
    [SerializeField] private TMPro.TMP_InputField cmdInputField; 
    private string cmdInput;
    [SerializeField] private float slowTimeScale = 0.1f; // The time scale to set when CMD is active
    //public ControlAbility1 controlAbility;
    public CommandCaster commandCast;
    void Start()
    {
        cmdKeyboard.SetActive(false);
        playerInput = GetComponent<PlayerInput>();
        commandCast = GetComponent<CommandCaster>();
    }
    public void OnCmd()
    {
        //Slow down time
        Time.timeScale = slowTimeScale;
        //Switch control map to CMD action map
        playerInput.SwitchCurrentActionMap("Keyboard");
        cmdKeyboard.SetActive(true);
        cmdInputField.Select();
        cmdInputField.ActivateInputField();
    }
    public void OnPlayer()
    {
        cmdInput = cmdInputField.text;
        Debug.Log($"{cmdInput}");
        //Reset time scale
        Time.timeScale = 1.0f;
        //Switch control map to Player action map
        playerInput.SwitchCurrentActionMap("Player");
        cmdKeyboard.SetActive(false);
        //Call the command
        commandCast.ExecuteCommand(cmdInput);
    }

}
