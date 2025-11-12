using UnityEngine;
using UnityEngine.InputSystem;

namespace RSG
{
    [CreateAssetMenu( fileName = "InputReader", menuName = "RSG/Input/Input Reader" )]
    public class InputReader : ScriptableObject, GameInput.IPlayerActions
    {
        private GameInput _gameInput;
        
        private void OnEnable()
        {
            if( _gameInput == null )
            {
                _gameInput = new GameInput();
                _gameInput.Player.SetCallbacks( this );
            }
        }

        private void OnDisable()
        {
            _gameInput.Player.Disable();
        }
        public void OnMove( InputAction.CallbackContext context )
        {
            throw new System.NotImplementedException();
        }
        public void OnLook( InputAction.CallbackContext context )
        {
            throw new System.NotImplementedException();
        }
        public void OnAttack( InputAction.CallbackContext context )
        {
            throw new System.NotImplementedException();
        }
        public void OnInteract( InputAction.CallbackContext context )
        {
            throw new System.NotImplementedException();
        }
        public void OnCrouch( InputAction.CallbackContext context )
        {
            throw new System.NotImplementedException();
        }
        public void OnJump( InputAction.CallbackContext context )
        {
            throw new System.NotImplementedException();
        }
        public void OnPrevious( InputAction.CallbackContext context )
        {
            throw new System.NotImplementedException();
        }
        public void OnNext( InputAction.CallbackContext context )
        {
            throw new System.NotImplementedException();
        }
        public void OnSprint( InputAction.CallbackContext context )
        {
            throw new System.NotImplementedException();
        }
    }
}