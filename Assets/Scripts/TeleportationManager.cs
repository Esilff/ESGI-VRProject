using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private TeleportationProvider provider;
    private InputAction _thumbstick;
    private bool _isActive;


    // Start is called before the first frame update
    void Start()
    {
        rayInteractor.enabled = false;
        var activate = actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Mode Activate");
        activate.Enable();
        activate.performed += OnTeleportActivate;

        var cancel = actionAsset.FindActionMap("XRI LeftHand Interaction").FindAction("Teleport Mode Cancel");
        cancel.Enable();
        cancel.performed += OnTeleportCancelled;

        _thumbstick = actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Move");
        _thumbstick.Enable();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive) return;
        switch (_thumbstick.triggered)
        {
            case true:
                return;
            case false:
                _isActive = false;
                rayInteractor.enabled = false;
                if (!rayInteractor.TryGetCurrent3DRaycastHit(out var hit))
                {
                    rayInteractor.enabled = false;
                    _isActive = false;
                    return;
                }

                var request = new TeleportRequest()
                {
                    destinationPosition = hit.point
                };
                provider.QueueTeleportRequest(request);
                rayInteractor.enabled = false;
                _isActive = false;
                break;
        }
       
    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = true;
        _isActive = true;

    }

    private void OnTeleportCancelled(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = false;
        _isActive = false;
    }
}
