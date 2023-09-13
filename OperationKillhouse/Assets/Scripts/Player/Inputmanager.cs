using UnityEngine;

public class Inputmanager : MonoBehaviour
{
    public InputMaster inputMaster;

    private void Awake()
    {
        inputMaster = new InputMaster();
    }
    private void OnEnable()
    {
        inputMaster.Enable();
    }

    public void OnDisable()
    {
        inputMaster.Disable();
    }
}
