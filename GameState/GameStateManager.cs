using System;
using UnityEngine;

namespace RSG
{
    public enum GameState
    {
        Uninitialised,
        Splash,
        LoadMainScene,
        Tutorial,
        MainMenu,
        Gameplay,
        Win,
        Fail
    }

    public static class GameStateManager
    {
        public static event Action<GameState> OnStateChanged;
        public static event Action<GameState, GameState> OnStateTransition;

        public static GameState CurrentState { get; private set; } = GameState.Uninitialised;
        public static GameState PreviousState { get; private set; } = GameState.Uninitialised;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInitialise()
        {
            if (CurrentState == GameState.Uninitialised)
                SetState(GameState.Splash);
        }

        public static void SetState(GameState newState)
        {
            if (CurrentState == newState)
            {
                Debug.Log($"[GameStateManager] Already in state {newState}. Ignoring.");
                return;
            }

            GameState previousState = CurrentState;
            Debug.Log($"[GameStateManager] {previousState} -> {newState}");
            OnStateTransition?.Invoke(previousState, newState);
            PreviousState = previousState;
            CurrentState = newState;
            OnStateChanged?.Invoke(CurrentState);
        }
    }
}