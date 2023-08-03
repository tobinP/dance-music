using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using TMPro;

public class ButtonController : MonoBehaviour
{
    static readonly Dictionary<string, InputFeatureUsage<bool>> availableButtons = new Dictionary<string, InputFeatureUsage<bool>>
    {
        {"triggerButton", CommonUsages.triggerButton },
        {"thumbrest", CommonUsages.thumbrest },
        {"primary2DAxisClick", CommonUsages.primary2DAxisClick },
        {"primary2DAxisTouch", CommonUsages.primary2DAxisTouch },
        {"menuButton", CommonUsages.menuButton },
        {"gripButton", CommonUsages.gripButton },
        {"secondaryButton", CommonUsages.secondaryButton },
        {"secondaryTouch", CommonUsages.secondaryTouch },
        {"primaryButton", CommonUsages.primaryButton },
        {"primaryTouch", CommonUsages.primaryTouch },
    };

    public enum ButtonOption
    {
        triggerButton,
        thumbrest,
        primary2DAxisClick,
        primary2DAxisTouch,
        menuButton,
        gripButton,
        secondaryButton,
        secondaryTouch,
        primaryButton,
        primaryTouch
    };

    [Tooltip("Input device role (left or right controller)")]
    public InputDeviceRole deviceRole;
    
    [Tooltip("Select the button")]
    public ButtonOption button;

    [Tooltip("Event when the button starts being pressed")]
    public UnityEvent OnPress;

    [Tooltip("Event when the button is released")]
    public UnityEvent OnRelease;

    public bool IsPrimaryPressed = false;
    public bool IsSecondaryPressed = false;
    public bool secondaryOn = false;

    public bool needsUpdate = false;
    public bool isReleased = true;
    public GameObject testObject;
    public GameObject actualController;
    public TextMeshProUGUI textMeshPro;

    private Vector3 scaleUp = new Vector3(50,50,50);
    private TestObj obj = new TestObj { name = "triggerDown", xVal = 0 };
    private SocketMan socketMan;
    private Transform transform;

    // to obtain input devices
    List<InputDevice> inputDevices;
    bool inputValue;

    InputFeatureUsage<bool> inputFeature;

    void Awake()
    {
        // get label selected by the user
        string featureLabel = Enum.GetName(typeof(ButtonOption), button);

        // find dictionary entry
        availableButtons.TryGetValue(featureLabel, out inputFeature);
        
        // init list
        inputDevices = new List<InputDevice>();
    }

    void Start()
    {
        socketMan = SingleMan.Instance.SocketMan;
        transform = gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        TestObj actualObj = new TestObj { name = "actualController", xVal = actualController.transform.position.y };
        textMeshPro.text = $"val: {actualController.transform.position.y}";
        socketMan.Send(actualObj);

        if (needsUpdate)
        {
            needsUpdate = false;
            if (isReleased)
            {
                testObject.transform.localScale -= scaleUp;
                obj.name = "triggerUp";
                socketMan.Send(obj);
            }
            else
            {
                testObject.transform.localScale += scaleUp;
                obj.name = "triggerDown";
                socketMan.Send(obj);
            }
        }

        InputDevices.GetDevicesWithRole(deviceRole, inputDevices);

        for (int i = 0; i < inputDevices.Count; i++)
        {
            if (inputDevices[i].TryGetFeatureValue(CommonUsages.primaryButton, out inputValue) && inputValue)
            {
                if (!IsPrimaryPressed)
                {
                    IsPrimaryPressed = true;
                    testObject.transform.localScale += scaleUp;
                    obj.name = "primaryDown";
                    socketMan.Send(obj);
                }
            }
            else if (IsPrimaryPressed)
            {
                IsPrimaryPressed = false;
                testObject.transform.localScale -= scaleUp;
                obj.name = "primaryUp";
                socketMan.Send(obj);
            }

            if (inputDevices[i].TryGetFeatureValue(CommonUsages.secondaryButton, out inputValue) && inputValue)
            {
                if (!IsSecondaryPressed)
                {
                    IsSecondaryPressed = true;
                    secondaryOn = true;
                    testObject.transform.localScale += scaleUp;
                    obj.name = "secondaryDown";
                    socketMan.Send(obj);
                }
            }
            else if (IsSecondaryPressed)
            {
                IsSecondaryPressed = false;
                secondaryOn = false;
                testObject.transform.localScale -= scaleUp;
                obj.name = "secondaryUp";
                socketMan.Send(obj);
            }
        }

        // if (IsPrimaryPressed)
        // {
        //     TestObj ob = new TestObj { name = "primaryDown", xVal = transform.position.y };
        //     socketMan.Send(ob);
        // }

        if (secondaryOn)
        {
            Debug.Log($"&&& onSecondary: {transform.position.y}");
            TestObj ob = new TestObj { name = "secondaryDown", xVal = actualController.transform.position.y };
            socketMan.Send(ob);
        }
    }
}
