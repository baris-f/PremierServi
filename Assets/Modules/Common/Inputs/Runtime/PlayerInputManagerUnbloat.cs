// using System;
// using UnityEngine.Events;
// using UnityEngine.InputSystem.Controls;
// using UnityEngine.InputSystem.LowLevel;
// using UnityEngine.InputSystem.Users;
// using UnityEngine.InputSystem.Utilities;
// #if UNITY_EDITOR
// using UnityEditor;
// #endif
//
// namespace UnityEngine.InputSystem
// {
//     public class PlayerInputManager : MonoBehaviour
//     {
//         public const string PlayerJoinedMessage = "OnPlayerJoined";
//         public const string PlayerLeftMessage = "OnPlayerLeft";
//         public int playerCount => PlayerInput.s_AllActivePlayersCount;
//         public int maxPlayerCount => m_MaxPlayerCount;
//
//         public bool joiningEnabled => m_AllowJoining;
//
//         public PlayerJoinBehavior joinBehavior
//         {
//             get => m_JoinBehavior;
//             set
//             {
//                 if (m_JoinBehavior == value)
//                     return;
//
//                 var joiningEnabled = m_AllowJoining;
//                 if (joiningEnabled)
//                     DisableJoining();
//                 m_JoinBehavior = value;
//                 if (joiningEnabled)
//                     EnableJoining();
//             }
//         }
//
//         public InputActionProperty joinAction
//         {
//             get => m_JoinAction;
//             set
//             {
//                 if (m_JoinAction == value)
//                     return;
//
//                 var joinEnabled = m_AllowJoining &&
//                                   m_JoinBehavior == PlayerJoinBehavior.JoinPlayersWhenJoinActionIsTriggered;
//                 if (joinEnabled)
//                     DisableJoining();
//
//                 m_JoinAction = value;
//
//                 if (joinEnabled)
//                     EnableJoining();
//             }
//         }
//         public GameObject playerPrefab
//         {
//             get => m_PlayerPrefab;
//             set => m_PlayerPrefab = value;
//         }
//         
//         public void EnableJoining()
//         {
//             switch (m_JoinBehavior)
//             {
//                 case PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed:
//                     ValidateInputActionAsset();
//
//                     if (!m_UnpairedDeviceUsedDelegateHooked)
//                     {
//                         if (m_UnpairedDeviceUsedDelegate == null)
//                             m_UnpairedDeviceUsedDelegate = OnUnpairedDeviceUsed;
//                         InputUser.onUnpairedDeviceUsed += m_UnpairedDeviceUsedDelegate;
//                         m_UnpairedDeviceUsedDelegateHooked = true;
//                         ++InputUser.listenForUnpairedDeviceActivity;
//                     }
//
//                     break;
//
//                 case PlayerJoinBehavior.JoinPlayersWhenJoinActionIsTriggered:
//                     // Hook into join action if we have one.
//                     if (m_JoinAction.action != null)
//                     {
//                         if (!m_JoinActionDelegateHooked)
//                         {
//                             if (m_JoinActionDelegate == null)
//                                 m_JoinActionDelegate = JoinPlayerFromActionIfNotAlreadyJoined;
//                             m_JoinAction.action.performed += m_JoinActionDelegate;
//                             m_JoinActionDelegateHooked = true;
//                         }
//
//                         m_JoinAction.action.Enable();
//                     }
//                     else
//                     {
//                         Debug.LogError(
//                             $"No join action configured on PlayerInputManager but join behavior is set to {nameof(PlayerJoinBehavior.JoinPlayersWhenJoinActionIsTriggered)}",
//                             this);
//                     }
//
//                     break;
//             }
//
//             m_AllowJoining = true;
//         }
//
//         /// <summary>
//         /// Inhibit players from joining the game.
//         /// </summary>
//         /// <seealso cref="EnableJoining"/>
//         /// <seealso cref="joiningEnabled"/>
//         public void DisableJoining()
//         {
//             switch (m_JoinBehavior)
//             {
//                 case PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed:
//                     if (m_UnpairedDeviceUsedDelegateHooked)
//                     {
//                         InputUser.onUnpairedDeviceUsed -= m_UnpairedDeviceUsedDelegate;
//                         m_UnpairedDeviceUsedDelegateHooked = false;
//                         --InputUser.listenForUnpairedDeviceActivity;
//                     }
//
//                     break;
//
//                 case PlayerJoinBehavior.JoinPlayersWhenJoinActionIsTriggered:
//                     if (m_JoinActionDelegateHooked)
//                     {
//                         var joinAction = m_JoinAction.action;
//                         if (joinAction != null)
//                             m_JoinAction.action.performed -= m_JoinActionDelegate;
//                         m_JoinActionDelegateHooked = false;
//                     }
//
//                     m_JoinAction.action?.Disable();
//                     break;
//             }
//
//             m_AllowJoining = false;
//         }
//
//         /// <summary>
//         /// Spawn a new player from <see cref="playerPrefab"/>.
//         /// </summary>
//         /// <param name="playerIndex">Optional explicit <see cref="PlayerInput.playerIndex"/> to assign to the player. Must be unique within
//         /// <see cref="PlayerInput.all"/>. If not supplied, a player index will be assigned automatically (smallest unused index will be used).</param>
//         /// <param name="splitScreenIndex">Optional <see cref="PlayerInput.splitScreenIndex"/>. If supplied, this assigns a split-screen area to the player. For example,
//         /// a split-screen index of </param>
//         /// <param name="controlScheme">Control scheme to activate on the player (optional). If not supplied, a control scheme will
//         /// be selected based on <paramref name="pairWithDevice"/>. If no device is given either, the first control scheme that matches
//         /// the currently available unpaired devices (see <see cref="InputUser.GetUnpairedInputDevices()"/>) is used.</param>
//         /// <param name="pairWithDevice">Device to pair to the player. Also determines which control scheme to use if <paramref name="controlScheme"/>
//         /// is not given.</param>
//         /// <returns>The newly instantiated player or <c>null</c> if joining failed.</returns>
//         /// <remarks>
//         /// Joining must be enabled (see <see cref="joiningEnabled"/>) or the method will fail.
//         ///
//         /// To pair multiple devices, use <see cref="JoinPlayer(int,int,string,InputDevice[])"/>.
//         /// </remarks>
//         public PlayerInput JoinPlayer(int playerIndex = -1, int splitScreenIndex = -1, string controlScheme = null,
//             InputDevice pairWithDevice = null)
//         {
//             if (!CheckIfPlayerCanJoin(playerIndex))
//                 return null;
//
//             // PlayerInput.s_DestroyIfDeviceSetupUnsuccessful = true;
//             return PlayerInput.Instantiate(m_PlayerPrefab, playerIndex: playerIndex, splitScreenIndex: splitScreenIndex,
//                 controlScheme: controlScheme, pairWithDevice: pairWithDevice);
//         }
//
//         /// <summary>
//         /// Spawn a new player from <see cref="playerPrefab"/>.
//         /// </summary>
//         /// <param name="playerIndex">Optional explicit <see cref="PlayerInput.playerIndex"/> to assign to the player. Must be unique within
//         /// <see cref="PlayerInput.all"/>. If not supplied, a player index will be assigned automatically (smallest unused index will be used).</param>
//         /// <param name="splitScreenIndex">Optional <see cref="PlayerInput.splitScreenIndex"/>. If supplied, this assigns a split-screen area to the player. For example,
//         /// a split-screen index of </param>
//         /// <param name="controlScheme">Control scheme to activate on the player (optional). If not supplied, a control scheme will
//         /// be selected based on <paramref name="pairWithDevices"/>. If no device is given either, the first control scheme that matches
//         /// the currently available unpaired devices (see <see cref="InputUser.GetUnpairedInputDevices()"/>) is used.</param>
//         /// <param name="pairWithDevices">Devices to pair to the player. Also determines which control scheme to use if <paramref name="controlScheme"/>
//         /// is not given.</param>
//         /// <returns>The newly instantiated player or <c>null</c> if joining failed.</returns>
//         /// <remarks>
//         /// Joining must be enabled (see <see cref="joiningEnabled"/>) or the method will fail.
//         /// </remarks>
//         public PlayerInput JoinPlayer(int playerIndex = -1, int splitScreenIndex = -1, string controlScheme = null,
//             params InputDevice[] pairWithDevices)
//         {
//             if (!CheckIfPlayerCanJoin(playerIndex))
//                 return null;
//
//             PlayerInput.s_DestroyIfDeviceSetupUnsuccessful = true;
//             return PlayerInput.Instantiate(m_PlayerPrefab, playerIndex: playerIndex, splitScreenIndex: splitScreenIndex,
//                 controlScheme: controlScheme, pairWithDevices: pairWithDevices);
//         }
//
//         [SerializeField] internal PlayerNotifications m_NotificationBehavior;
//         [Tooltip("Set a limit for the maximum number of players who are able to join.")]
//         [SerializeField] internal int m_MaxPlayerCount = -1;
//         [SerializeField] internal bool m_AllowJoining = true;
//         [SerializeField] internal PlayerJoinBehavior m_JoinBehavior;
//         [SerializeField] internal PlayerJoinedEvent m_PlayerJoinedEvent;
//         [SerializeField] internal PlayerLeftEvent m_PlayerLeftEvent;
//         [SerializeField] internal InputActionProperty m_JoinAction;
//         [SerializeField] internal GameObject m_PlayerPrefab;
//         [SerializeField] internal bool m_SplitScreen;
//         [SerializeField] internal bool m_MaintainAspectRatioInSplitScreen;
//         [Tooltip(
//             "Explicitly set a fixed number of screens or otherwise allow the screen to be divided automatically to best fit the number of players.")]
//         [SerializeField] internal int m_FixedNumberOfSplitScreens = -1;
//         [SerializeField] internal Rect m_SplitScreenRect = new Rect(0, 0, 1, 1);
//
//         [NonSerialized] private bool m_JoinActionDelegateHooked;
//         [NonSerialized] private bool m_UnpairedDeviceUsedDelegateHooked;
//         [NonSerialized] private Action<InputAction.CallbackContext> m_JoinActionDelegate;
//         [NonSerialized] private Action<InputControl, InputEventPtr> m_UnpairedDeviceUsedDelegate;
//         [NonSerialized] private CallbackArray<Action<PlayerInput>> m_PlayerJoinedCallbacks;
//         [NonSerialized] private CallbackArray<Action<PlayerInput>> m_PlayerLeftCallbacks;
//
//         internal static string[] messages => new[]
//         {
//             PlayerJoinedMessage,
//             PlayerLeftMessage,
//         };
//
//         private bool CheckIfPlayerCanJoin(int playerIndex = -1)
//         {
//             if (m_PlayerPrefab == null)
//             {
//                 Debug.LogError("playerPrefab must be set in order to be able to join new players", this);
//                 return false;
//             }
//
//             if (m_MaxPlayerCount >= 0 && playerCount >= m_MaxPlayerCount)
//             {
//                 Debug.LogError("Have reached maximum player count of " + maxPlayerCount, this);
//                 return false;
//             }
//
//             // If we have a player index, make sure it's unique.
//             if (playerIndex != -1)
//             {
//                 for (var i = 0; i < PlayerInput.s_AllActivePlayersCount; ++i)
//                     if (PlayerInput.s_AllActivePlayers[i].playerIndex == playerIndex)
//                     {
//                         Debug.LogError(
//                             $"Player index #{playerIndex} is already taken by player {PlayerInput.s_AllActivePlayers[i]}",
//                             PlayerInput.s_AllActivePlayers[i]);
//                         return false;
//                     }
//             }
//
//             return true;
//         }
//
//         private void OnUnpairedDeviceUsed(InputControl control, InputEventPtr eventPtr)
//         {
//             if (!m_AllowJoining)
//                 return;
//
//             if (m_JoinBehavior == PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed)
//             {
//                 // Make sure it's a button that was actuated.
//                 if (!(control is ButtonControl))
//                     return;
//
//                 // Make sure it's a device that is usable by the player's actions. We don't want
//                 // to join a player who's then stranded and has no way to actually interact with the game.
//                 if (!IsDeviceUsableWithPlayerActions(control.device))
//                     return;
//
//                 ////REVIEW: should we log a warning or error when the actions for the player do not have control schemes?
//
//                 JoinPlayer(pairWithDevice: control.device);
//             }
//         }
//
//         private void OnEnable()
//         {
//             // if the join action is a reference, clone it so we don't run into problems with the action being disabled by
//             // PlayerInput when devices are assigned to individual players
//             if (joinAction.reference != null && joinAction.action?.actionMap?.asset != null)
//             {
//                 var inputActionAsset = Instantiate(joinAction.action.actionMap.asset);
//                 var inputActionReference =
//                     InputActionReference.Create(inputActionAsset.FindAction(joinAction.action.name));
//                 joinAction = new InputActionProperty(inputActionReference);
//             }
//
//             // Join all players already in the game.
//             for (var i = 0; i < PlayerInput.s_AllActivePlayersCount; ++i)
//                 NotifyPlayerJoined(PlayerInput.s_AllActivePlayers[i]);
//
//             if (m_AllowJoining)
//                 EnableJoining();
//         }
//
//         private void OnDisable()
//         {
//             if (m_AllowJoining)
//                 DisableJoining();
//         }
//
//         /// <summary>
//         /// If split-screen is enabled, then for each player in the game, adjust the player's <see cref="Camera.rect"/>
//         /// to fit the player's split screen area according to the number of players currently in the game and the
//         /// current split-screen configuration.
//         /// </summary>
//         private void UpdateSplitScreen()
//         {
//             // Nothing to do if split-screen is not enabled.
//             if (!m_SplitScreen)
//                 return;
//
//             // Determine number of split-screens to create based on highest player index we have.
//             var minSplitScreenCount = 0;
//             foreach (var player in PlayerInput.all)
//             {
//                 if (player.playerIndex >= minSplitScreenCount)
//                     minSplitScreenCount = player.playerIndex + 1;
//             }
//
//             // Adjust to fixed number if we have it.
//             if (m_FixedNumberOfSplitScreens > 0)
//             {
//                 if (m_FixedNumberOfSplitScreens < minSplitScreenCount)
//                     Debug.LogWarning(
//                         $"Highest playerIndex of {minSplitScreenCount} exceeds fixed number of split-screens of {m_FixedNumberOfSplitScreens}",
//                         this);
//
//                 minSplitScreenCount = m_FixedNumberOfSplitScreens;
//             }
//
//             // Determine divisions along X and Y. Usually, we have a square grid of split-screens so all we need to
//             // do is make it large enough to fit all players.
//             var numDivisionsX = Mathf.CeilToInt(Mathf.Sqrt(minSplitScreenCount));
//             var numDivisionsY = numDivisionsX;
//             if (!m_MaintainAspectRatioInSplitScreen && numDivisionsX * (numDivisionsX - 1) >= minSplitScreenCount)
//             {
//                 // We're allowed to produce split-screens with aspect ratios different from the screen meaning
//                 // that we always add one more column before finally adding an entirely new row.
//                 numDivisionsY -= 1;
//             }
//
//             // Assign split-screen area to each player.
//             foreach (var player in PlayerInput.all)
//             {
//                 // Make sure the player's splitScreenIndex isn't out of range.
//                 var splitScreenIndex = player.splitScreenIndex;
//                 if (splitScreenIndex >= numDivisionsX * numDivisionsY)
//                 {
//                     Debug.LogError(
//                         $"Split-screen index of {splitScreenIndex} on player is out of range (have {numDivisionsX * numDivisionsY} screens); resetting to playerIndex",
//                         player);
//                     player.m_SplitScreenIndex = player.playerIndex;
//                 }
//
//                 // Make sure we have a camera.
//                 var camera = player.camera;
//                 if (camera == null)
//                 {
//                     Debug.LogError(
//                         "Player has no camera associated with it. Cannot set up split-screen. Point PlayerInput.camera to camera for player.",
//                         player);
//                     continue;
//                 }
//
//                 // Assign split-screen area based on m_SplitScreenRect.
//                 var column = splitScreenIndex % numDivisionsX;
//                 var row = splitScreenIndex / numDivisionsX;
//                 var rect = new Rect
//                 {
//                     width = m_SplitScreenRect.width / numDivisionsX,
//                     height = m_SplitScreenRect.height / numDivisionsY
//                 };
//                 rect.x = m_SplitScreenRect.x + column * rect.width;
//                 // Y is bottom-to-top but we fill from top down.
//                 rect.y = m_SplitScreenRect.y + m_SplitScreenRect.height - (row + 1) * rect.height;
//                 camera.rect = rect;
//             }
//         }
//
//         private bool IsDeviceUsableWithPlayerActions(InputDevice device)
//         {
//             Debug.Assert(device != null);
//
//             if (m_PlayerPrefab == null)
//                 return true;
//
//             var playerInput = m_PlayerPrefab.GetComponentInChildren<PlayerInput>();
//             if (playerInput == null)
//                 return true;
//
//             var actions = playerInput.actions;
//             if (actions == null)
//                 return true;
//
//             // If the asset has control schemes, see if there's one that works with the device plus
//             // whatever unpaired devices we have left.
//             if (actions.controlSchemes.Count > 0)
//             {
//                 using (var unpairedDevices = InputUser.GetUnpairedInputDevices())
//                 {
//                     if (InputControlScheme.FindControlSchemeForDevices(unpairedDevices, actions.controlSchemes,
//                             mustIncludeDevice: device) == null)
//                         return false;
//                 }
//
//                 return true;
//             }
//
//             // Otherwise just check whether any of the maps has bindings usable with the device.
//             foreach (var actionMap in actions.actionMaps)
//                 if (actionMap.IsUsableWithDevice(device))
//                     return true;
//
//             return false;
//         }
//
//         private void ValidateInputActionAsset()
//         {
// #if DEVELOPMENT_BUILD || UNITY_EDITOR
//             if (m_PlayerPrefab == null || m_PlayerPrefab.GetComponentInChildren<PlayerInput>() == null)
//                 return;
//
//             var actions = m_PlayerPrefab.GetComponentInChildren<PlayerInput>().actions;
//             if (actions == null)
//                 return;
//
//             var isValid = true;
//             foreach (var controlScheme in actions.controlSchemes)
//             {
//                 if (controlScheme.deviceRequirements.Count > 0)
//                     break;
//
//                 isValid = false;
//             }
//
//             if (isValid) return;
//
//             var assetInfo = actions.name;
// #if UNITY_EDITOR
//             assetInfo = AssetDatabase.GetAssetPath(actions);
// #endif
//             Debug.LogWarning(
//                 $"The input action asset '{assetInfo}' in the player prefab assigned to PlayerInputManager has " +
//                 "no control schemes with required devices. The JoinPlayersWhenButtonIsPressed join behavior " +
//                 "will not work unless the expected input devices are listed as requirements in the input " +
//                 "action asset.", m_PlayerPrefab);
// #endif
//         }
//
//         /// <summary>
//         /// Called by <see cref="PlayerInput"/> when it is enabled.
//         /// </summary>
//         /// <param name="player"></param>
//         internal void NotifyPlayerJoined(PlayerInput player)
//         {
//             Debug.Assert(player != null);
//
//             UpdateSplitScreen();
//
//             switch (m_NotificationBehavior)
//             {
//                 case PlayerNotifications.SendMessages:
//                     SendMessage(PlayerJoinedMessage, player, SendMessageOptions.DontRequireReceiver);
//                     break;
//
//                 case PlayerNotifications.BroadcastMessages:
//                     BroadcastMessage(PlayerJoinedMessage, player, SendMessageOptions.DontRequireReceiver);
//                     break;
//
//                 case PlayerNotifications.InvokeUnityEvents:
//                     m_PlayerJoinedEvent?.Invoke(player);
//                     break;
//
//                 case PlayerNotifications.InvokeCSharpEvents:
//                     DelegateHelpers.InvokeCallbacksSafe(ref m_PlayerJoinedCallbacks, player, "onPlayerJoined");
//                     break;
//             }
//         }
//
//         /// <summary>
//         /// Called by <see cref="PlayerInput"/> when it is disabled.
//         /// </summary>
//         /// <param name="player"></param>
//         internal void NotifyPlayerLeft(PlayerInput player)
//         {
//             Debug.Assert(player != null);
//
//             UpdateSplitScreen();
//
//             switch (m_NotificationBehavior)
//             {
//                 case PlayerNotifications.SendMessages:
//                     SendMessage(PlayerLeftMessage, player, SendMessageOptions.DontRequireReceiver);
//                     break;
//
//                 case PlayerNotifications.BroadcastMessages:
//                     BroadcastMessage(PlayerLeftMessage, player, SendMessageOptions.DontRequireReceiver);
//                     break;
//
//                 case PlayerNotifications.InvokeUnityEvents:
//                     m_PlayerLeftEvent?.Invoke(player);
//                     break;
//
//                 case PlayerNotifications.InvokeCSharpEvents:
//                     DelegateHelpers.InvokeCallbacksSafe(ref m_PlayerLeftCallbacks, player, "onPlayerLeft");
//                     break;
//             }
//         }
//
//         [Serializable]
//         public class PlayerJoinedEvent : UnityEvent<PlayerInput>
//         {
//         }
//
//         [Serializable]
//         public class PlayerLeftEvent : UnityEvent<PlayerInput>
//         {
//         }
//     }
// }