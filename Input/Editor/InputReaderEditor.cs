using UnityEditor;
using UnityEngine;

namespace RSG.Editor
{
    [CustomEditor(typeof(InputReader))]
    public class InputReaderEditor : UnityEditor.Editor
    {
        private InputReader m_inputReader;

        // --- Player Debug ---
        private Vector2 m_debugMove;
        private Vector2 m_debugLook;
        private bool m_debugCrouchHeld;
        private bool m_debugSprintHeld;
        private string m_debugLastPlayerEvent = "Idle";
        private double m_lastPlayerEventTime;

        // --- UI Debug ---
        private Vector2 m_debugNavigate;
        private Vector2 m_debugScrollWheel;
        private Vector2 m_debugPoint;
        private string m_debugLastUIEvent = "Idle";
        private double m_lastUIEventTime;

        private void OnEnable()
        {
            m_inputReader = (InputReader)target;
            
            // --- Player Events ---
            m_inputReader.MoveEvent += OnMove;
            m_inputReader.LookEvent += OnLook;
            m_inputReader.AttackEvent += OnAttack;
            m_inputReader.InteractEvent += OnInteract;
            m_inputReader.CrouchEvent += OnCrouch;
            m_inputReader.JumpEvent += OnJump;
            m_inputReader.PreviousEvent += OnPrevious;
            m_inputReader.NextEvent += OnNext;
            m_inputReader.SprintEvent += OnSprint;

            // --- UI Events ---
            m_inputReader.NavigateEvent += OnNavigate;
            m_inputReader.SubmitEvent += OnSubmit;
            m_inputReader.CancelEvent += OnCancel;
            m_inputReader.ClickEvent += OnClick;
            m_inputReader.RightClickEvent += OnRightClick;
            m_inputReader.MiddleClickEvent += OnMiddleClick;
            m_inputReader.ScrollWheelEvent += OnScrollWheel;
            m_inputReader.MouseDownEvent += OnMouseDown;
            m_inputReader.MouseUpEvent += OnMouseUp;
            m_inputReader.PointEvent += OnPointEvent;
        }

        private void OnDisable()
        {
            if (m_inputReader == null) return;
            
            // --- Player Events ---
            m_inputReader.MoveEvent -= OnMove;
            m_inputReader.LookEvent -= OnLook;
            m_inputReader.AttackEvent -= OnAttack;
            m_inputReader.InteractEvent -= OnInteract;
            m_inputReader.CrouchEvent -= OnCrouch;
            m_inputReader.JumpEvent -= OnJump;
            m_inputReader.PreviousEvent -= OnPrevious;
            m_inputReader.NextEvent -= OnNext;
            m_inputReader.SprintEvent -= OnSprint;

            // --- UI Events ---
            m_inputReader.NavigateEvent -= OnNavigate;
            m_inputReader.SubmitEvent -= OnSubmit;
            m_inputReader.CancelEvent -= OnCancel;
            m_inputReader.ClickEvent -= OnClick;
            m_inputReader.RightClickEvent -= OnRightClick;
            m_inputReader.MiddleClickEvent -= OnMiddleClick;
            m_inputReader.ScrollWheelEvent -= OnScrollWheel;
            m_inputReader.MouseDownEvent -= OnMouseDown;
            m_inputReader.MouseUpEvent -= OnMouseUp;
            m_inputReader.PointEvent -= OnPointEvent;
        }

        public override void OnInspectorGUI()
        {
            // Draw the default inspector (which is empty, but good practice)
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Live Debug Values", EditorStyles.boldLabel);

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Enter Play Mode to see live input values.", MessageType.Info);
                return;
            }
            
            // --- Player Section ---
            EditorGUILayout.LabelField("--- Player ---", EditorStyles.boldLabel);
            EditorGUILayout.Vector2Field("Move", m_debugMove);
            EditorGUILayout.Vector2Field("Look", m_debugLook);
            EditorGUILayout.Toggle("Crouch Held", m_debugCrouchHeld);
            EditorGUILayout.Toggle("Sprint Held", m_debugSprintHeld);

            string playerEvent = GetFlashEvent(m_debugLastPlayerEvent, m_lastPlayerEventTime);
            EditorGUILayout.LabelField("Last Event", playerEvent);

            
            // --- UI Section ---
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("--- UI ---", EditorStyles.boldLabel);
            EditorGUILayout.Vector2Field("Navigate", m_debugNavigate);
            EditorGUILayout.Vector2Field("Point Position", m_debugPoint);
            EditorGUILayout.Vector2Field("Scroll Wheel", m_debugScrollWheel);

            string uiEvent = GetFlashEvent(m_debugLastUIEvent, m_lastUIEventTime);
            EditorGUILayout.LabelField("Last UI Event", uiEvent);
            if (playerEvent != "Idle" || uiEvent != "Idle")
            {
                Repaint();
            }
            
            string swapMessage = "N/A";
            bool isPlayerInputActive = m_inputReader.IsPlayerInputEnabled();
            bool isUIInputActive = m_inputReader.IsUIInputEnabled();
            if( isPlayerInputActive)
            {
                swapMessage = "Switch to UI Input";
            }
            else if( isUIInputActive)
            {
                swapMessage = "Switch to Player Input";
            }

            if( GUILayout.Button( swapMessage ) )
            {
                if( isPlayerInputActive )
                {
                    m_inputReader.EnableUIInput();
                }
                else if( isUIInputActive )
                {
                    m_inputReader.EnablePlayerInput();
                }
            }
        }
        
        private string GetFlashEvent(string eventName, double eventTime)
        {
            // Check if the event happened in the last 0.25 seconds
            if (EditorApplication.timeSinceStartup - eventTime < 0.25)
            {
                return eventName;
            }
            return "Idle";
        }

        // --- Player Handlers ---
        private void OnMove(Vector2 val) { m_debugMove = val; Repaint(); }
        private void OnLook(Vector2 val) { m_debugLook = val; Repaint(); }
        private void OnCrouch(bool val) { m_debugCrouchHeld = val; Repaint(); }
        private void OnSprint(bool val) { m_debugSprintHeld = val; Repaint(); }

        private void OnAttack() => FlashPlayerEvent("Attack");
        private void OnInteract() => FlashPlayerEvent("Interact");
        private void OnJump() => FlashPlayerEvent("Jump");
        private void OnPrevious() => FlashPlayerEvent("Previous");
        private void OnNext() => FlashPlayerEvent("Next");

        // --- UI Handlers ---
        private void OnNavigate(Vector2 val) { m_debugNavigate = val; Repaint(); }
        private void OnScrollWheel(Vector2 val) { m_debugScrollWheel = val; Repaint(); }
        private void OnPointEvent(Vector2 val) { m_debugPoint = val; Repaint(); }
        
        private void OnSubmit() => FlashUIEvent("Submit");
        private void OnCancel() => FlashUIEvent("Cancel");
        private void OnClick() => FlashUIEvent("Click");
        private void OnMouseDown() => FlashUIEvent("MouseDown");
        private void OnMouseUp() => FlashUIEvent("MouseUp");
        private void OnRightClick() => FlashUIEvent("RightClick");
        private void OnMiddleClick() => FlashUIEvent("MiddleClick");
        
        
        // --- Event Flashing Logic ---
        private void FlashPlayerEvent(string eventName)
        {
            m_debugLastPlayerEvent = eventName;
            m_lastPlayerEventTime = EditorApplication.timeSinceStartup;
            Repaint();
        }

        private void FlashUIEvent(string eventName)
        {
            m_debugLastUIEvent = eventName;
            m_lastUIEventTime = EditorApplication.timeSinceStartup;
            Repaint();
        }
    }
}