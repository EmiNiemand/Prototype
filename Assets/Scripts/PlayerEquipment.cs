using Assets.Scripts;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PlayerEquipment : MonoBehaviour
{
    public bool isActive = false;
    public static PlayerEquipment instance;
    UIDocument uiDocument;
    VisualElement visualElement;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        uiDocument = GameObject.Find("Inventory").GetComponent<UIDocument>();
        uiDocument.rootVisualElement.Q("Container").visible = false;
    }

    public void ShowEquipment()
    {
        if (isActive)
        {
            isActive = false;
            uiDocument.rootVisualElement.Q("Container").visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            isActive = true;
            uiDocument.rootVisualElement.Q("Container").visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}