using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace RSG
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "RSG/Input/Input Reader")]
    public class InputReader : ScriptableObject, GameInput.IPlayerActions, GameInput.IUIActions
    {
        private GameInput m_gameInput;

        #region Player Action Events
        // --- Player ---
        public event UnityAction<Vector2> MoveEvent;
        public event UnityAction<Vector2> LookEvent;
        public event UnityAction AttackEvent;
        public event UnityAction InteractEvent;
        public event UnityAction<bool> CrouchEvent; // (bool) for hold actions
        public event UnityAction JumpEvent;
        public event UnityAction PreviousEvent;
        public event UnityAction NextEvent;
        public event UnityAction<bool> SprintEvent; // (bool) for hold actions
        #endregion

        #region UI Action Events
        // --- UI ---
        public event UnityAction<Vector2> NavigateEvent;
        public event UnityAction SubmitEvent;
        public event UnityAction CancelEvent;
        public event UnityAction ClickEvent;
        public event UnityAction<Vector2> PointEvent; 
        public event UnityAction MouseDownEvent;
        public event UnityAction MouseUpEvent;
        public event UnityAction RightClickEvent;
        public event UnityAction MiddleClickEvent;
        public event UnityAction<Vector2> ScrollWheelEvent;
        public event UnityAction<Vector3> TrackedDevicePositionEvent;
        public event UnityAction<Quaternion> TrackedDeviceOrientationEvent;
        #endregion

        private void OnEnable()
        {
            if (m_gameInput == null)
            {
                m_gameInput = new GameInput();
                // Set callbacks for BOTH action maps
                m_gameInput.Player.SetCallbacks(this);
                m_gameInput.UI.SetCallbacks(this);
            }
            
            // Start with UI input enabled (e.g., for a main menu)
            EnableUIInput();
        }

        private void OnDisable()
        {
            // Disable all action maps
            m_gameInput.Player.Disable();
            m_gameInput.UI.Disable();
        }

        #region Action Map Switchers
        
        public void EnablePlayerInput()
        {
            m_gameInput.UI.Disable();
            m_gameInput.Player.Enable();
        }

        public void EnableUIInput()
        {
            m_gameInput.Player.Disable();
            m_gameInput.UI.Enable();
        }

        public bool IsPlayerInputEnabled()
        {
            return m_gameInput.Player.enabled;
        }

        public bool IsUIInputEnabled()
        {
            return m_gameInput.UI.enabled;
        }
        
        #endregion

        #region Player Action Implementations
        
        public void OnMove(InputAction.CallbackContext context)
        {
            // This is a continuous action, fires every frame value changes
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            // This is a button press, only fire on 'performed' (on press)
            if (context.performed)
                AttackEvent?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
                InteractEvent?.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            // This is a hold action
            if (context.performed) // Button pressed
                CrouchEvent?.Invoke(true);
            else if (context.canceled) // Button released
                CrouchEvent?.Invoke(false);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
                JumpEvent?.Invoke();
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            if (context.performed)
                PreviousEvent?.Invoke();
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            if (context.performed)
                NextEvent?.Invoke();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            // This is a hold action
            if (context.performed) // Button pressed
                SprintEvent?.Invoke(true);
            else if (context.canceled) // Button released
                SprintEvent?.Invoke(false);
        }
        
        #endregion

        #region UI Action Implementations
        
        public void OnNavigate(InputAction.CallbackContext context)
        {
            NavigateEvent?.Invoke(context.ReadValue<Vector2>());
        }
        
        public void OnMouseDown( InputAction.CallbackContext context )
        {
            if (context.performed)
                MouseDownEvent?.Invoke();
        }
        
        public void OnMouseUp( InputAction.CallbackContext context )
        {
            if (context.performed)
                MouseUpEvent?.Invoke();
        }
        
        public void OnPoint( InputAction.CallbackContext context )
        {
            PointEvent?.Invoke( context.ReadValue<Vector2>() );
        }
        
        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.performed)
                SubmitEvent?.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed)
                CancelEvent?.Invoke();
        }
        
        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.performed)
                ClickEvent?.Invoke();
        }
        
        public void OnRightClick(InputAction.CallbackContext context)
        {
            if (context.performed)
                RightClickEvent?.Invoke();
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            if (context.performed)
                MiddleClickEvent?.Invoke();
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            ScrollWheelEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
            TrackedDevicePositionEvent?.Invoke(context.ReadValue<Vector3>());
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
            TrackedDeviceOrientationEvent?.Invoke(context.ReadValue<Quaternion>());
        }
        #endregion
    }
}