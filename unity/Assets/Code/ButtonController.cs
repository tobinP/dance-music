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

    [Tooltip("Input device role (left or right controller)")]
    public InputDeviceRole deviceRole;

    public bool IsPrimaryPressed = false;
    public bool IsSecondaryPressed = false;
    public bool secondaryOn = false;

    public bool needsUpdate = false;
    public bool isReleased = true;
    public GameObject sphere;
    public TextMeshProUGUI textMeshPro;

    private Vector3 scaleUp = new Vector3(50,50,50);
    private SocketMan socketMan;
    private Transform transform;

    // to obtain input devices
    List<InputDevice> inputDevices;
    private bool inputValue = false;

    void Awake()
    {
        inputDevices = new List<InputDevice>();
    }

    void Start()
    {
        socketMan = SingleMan.Instance.SocketMan;
        transform = gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        InputDevices.GetDevicesWithRole(deviceRole, inputDevices);

        foreach (var device in inputDevices)
        {
            if (device.TryGetFeatureValue(CommonUsages.primaryButton, out inputValue) && inputValue)
            {
                if (!IsPrimaryPressed)
                {
                    IsPrimaryPressed = true;
                    sphere.transform.localScale += scaleUp;
                    var data = new Data { name = $"{device.role}-primaryDown", xVal = 0 };
                    textMeshPro.text = data.name;
                    socketMan.Send(data);
                }
            }
            else if (IsPrimaryPressed)
            {
                IsPrimaryPressed = false;
                sphere.transform.localScale -= scaleUp;
                var data = new Data { name = "primaryUp", xVal = 0 };
                socketMan.Send(data);
            }

            if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out inputValue) && inputValue)
            {
                if (!IsSecondaryPressed)
                {
                    IsSecondaryPressed = true;
                    secondaryOn = true;
                    sphere.transform.localScale += scaleUp;
                    var data = new Data { name = "secondaryDown", xVal = 0 };
                    socketMan.Send(data);
                }
            }
            else if (IsSecondaryPressed)
            {
                IsSecondaryPressed = false;
                secondaryOn = false;
                sphere.transform.localScale -= scaleUp;
                var data = new Data { name = "secondaryUp", xVal = 0 };
                socketMan.Send(data);
            }
        }

        if (secondaryOn)
        {
            var data = new Data { name = "secondaryDown", xVal = transform.position.y };
            socketMan.Send(data);
        }
    }
}
