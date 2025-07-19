using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "PlayerBindings";
    public static GameInput Instance { get; private set; }
    
    public event EventHandler OnInteractAction; 
    public event EventHandler OnInteractAlternativeAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBidingRebind;

    private PlayerInputActions playerInputActions;
    
    public enum Biding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        Interact_Alternate,
        Pause,
        Gamepad_Interact,
        Gamepad_Interact_Alternate,
        Gamepad_Pause,
    }
    
    private void Awake()
    {
        Instance = this;
        
        playerInputActions = new PlayerInputActions();
        
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));;
        }
        
        playerInputActions.Player.Enable();
        
        playerInputActions.Player.Interact.performed += InteractOnperformed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternativeOnperformed;
        playerInputActions.Player.Pause.performed += PauseOnperformed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= InteractOnperformed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternativeOnperformed;
        playerInputActions.Player.Pause.performed -= PauseOnperformed;
        
        playerInputActions.Dispose();
    }

    private void PauseOnperformed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternativeOnperformed(InputAction.CallbackContext obj)
    {
        OnInteractAlternativeAction?.Invoke(this, EventArgs.Empty);
    }
    
    private void InteractOnperformed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        var input = playerInputActions.Player.Move.ReadValue<Vector2>();
        
        return input.normalized;
    }

    public string GetBidingText(Biding biding)
    {
        switch (biding)
        {
            default:
            case Biding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Biding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Biding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Biding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Biding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Biding.Interact_Alternate:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Biding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();  
            case Biding.Gamepad_Interact:
                return playerInputActions.Player.Interact.bindings[1].ToDisplayString();  
            case Biding.Gamepad_Interact_Alternate:
                return playerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Biding.Gamepad_Pause:
                return playerInputActions.Player.Pause.bindings[1].ToDisplayString();  
        }
    }
    
    public void RebindBiding(Biding biding, Action onActionRebound)
    {
        playerInputActions.Player.Disable();

        switch (biding)
        {
            case Biding.Move_Up:
                playerInputActions.Player.Move.PerformInteractiveRebinding(1)
                    .OnComplete(callback =>
                    {
                        callback.Dispose();
                        playerInputActions.Player.Enable();
                        onActionRebound();

                        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                        PlayerPrefs.Save();
                        OnBidingRebind?.Invoke(this, EventArgs.Empty);
                    }).Start();
                break;
            case Biding.Move_Down:
                playerInputActions.Player.Move.PerformInteractiveRebinding(2)
                    .OnComplete(callback =>
                    {
                        callback.Dispose();
                        playerInputActions.Player.Enable();
                        onActionRebound();
                        
                        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                        PlayerPrefs.Save();
                        OnBidingRebind?.Invoke(this, EventArgs.Empty);
                    }).Start();
                break;
            case Biding.Move_Left:
                playerInputActions.Player.Move.PerformInteractiveRebinding(3)
                    .OnComplete(callback =>
                    {
                        callback.Dispose();
                        playerInputActions.Player.Enable();
                        onActionRebound();
                        
                        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                        PlayerPrefs.Save();
                        OnBidingRebind?.Invoke(this, EventArgs.Empty);
                    }).Start();
                break;
            case Biding.Move_Right:
                playerInputActions.Player.Move.PerformInteractiveRebinding(4)
                    .OnComplete(callback =>
                    {
                        callback.Dispose();
                        playerInputActions.Player.Enable();
                        onActionRebound();
                        
                        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                        PlayerPrefs.Save();
                        OnBidingRebind?.Invoke(this, EventArgs.Empty);
                    }).Start();
                break;
            case Biding.Interact:
                playerInputActions.Player.Interact.PerformInteractiveRebinding(0)
                    .OnComplete(callback =>
                    {
                        callback.Dispose();
                        playerInputActions.Player.Enable();
                        onActionRebound();
                        
                        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                        PlayerPrefs.Save();
                        OnBidingRebind?.Invoke(this, EventArgs.Empty);
                    }).Start();
                break;
            case Biding.Interact_Alternate:
                playerInputActions.Player.InteractAlternate.PerformInteractiveRebinding(0)
                    .OnComplete(callback =>
                    {
                        callback.Dispose();
                        playerInputActions.Player.Enable();
                        onActionRebound();
                        
                        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                        PlayerPrefs.Save();
                        OnBidingRebind?.Invoke(this, EventArgs.Empty);
                    }).Start();
                break;
            case Biding.Pause:
                playerInputActions.Player.Pause.PerformInteractiveRebinding(0)
                    .OnComplete(callback =>
                    {
                        callback.Dispose();
                        playerInputActions.Player.Enable();
                        onActionRebound();
                        
                        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                        PlayerPrefs.Save();
                        OnBidingRebind?.Invoke(this, EventArgs.Empty);
                    }).Start();
                break;
            case Biding.Gamepad_Pause:
                playerInputActions.Player.Pause.PerformInteractiveRebinding(1)
                    .OnComplete(callback =>
                    {
                        callback.Dispose();
                        playerInputActions.Player.Enable();
                        onActionRebound();
                        
                        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                        PlayerPrefs.Save();
                        OnBidingRebind?.Invoke(this, EventArgs.Empty);
                    }).Start();
                break;
            case Biding.Gamepad_Interact:
                playerInputActions.Player.Interact.PerformInteractiveRebinding(1)
                    .OnComplete(callback =>
                    {
                        callback.Dispose();
                        playerInputActions.Player.Enable();
                        onActionRebound();
                        
                        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                        PlayerPrefs.Save();
                        OnBidingRebind?.Invoke(this, EventArgs.Empty);
                    }).Start();
                break;
            case Biding.Gamepad_Interact_Alternate:
                playerInputActions.Player.InteractAlternate.PerformInteractiveRebinding(1)
                    .OnComplete(callback =>
                    {
                        callback.Dispose();
                        playerInputActions.Player.Enable();
                        onActionRebound();
                        
                        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                        PlayerPrefs.Save();
                        OnBidingRebind?.Invoke(this, EventArgs.Empty);
                    }).Start();
                break;
        }
    }
}