// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using UnityEngine;

namespace Sendbird.Chat.Sample
{
    public class SampleChatMain : MonoBehaviour
    {
        [SerializeField] private string _appId = "26426997-4777-4A66-BFCF-2418658B3B54";
        [SerializeField] private StateAbstract[] _states = null;
        [SerializeField] private GameObject _blockingGameObject = null;
        [SerializeField] private NotifyPopup notifyPopup = null;
        [SerializeField] private InfoPopup infoPopup = null;
        public string AppId => _appId;

        public static SampleChatMain Instance { get; private set; }
        private StateAbstract _currentState = null;

        private void Awake()
        {
            Instance = this;

            if (notifyPopup != null) notifyPopup.gameObject.SetActive(false);
            if (infoPopup != null) infoPopup.gameObject.SetActive(false);
            
            //Active only connecting state when start.
            if (_states != null)
            {
                foreach (StateAbstract state in _states)
                {
                    if (state == null)
                        continue;

                    state.InitializeState();
                    if (state.GetStateType() == StateType.Connecting)
                    {
                        state.gameObject.SetActive(true);
                        _currentState = state;
                    }
                    else
                    {
                        state.gameObject.SetActive(false);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (_states != null)
            {
                foreach (StateAbstract state in _states)
                {
                    if (state != null)
                        state.TerminateState();
                }
            }
        }

        private void Start()
        {
            UnblockUI();

            OpenState(StateType.Connecting);
        }

        public StateAbstract OpenState(StateType inStateType, StateAbstract.OpenParamsAbstract inOpenParams = null)
        {
            if (_states == null)
                return null;

            if (_currentState != null)
            {
                _currentState.CloseState();
                _currentState = null;
            }

            StateAbstract openedState = null;
            foreach (StateAbstract state in _states)
            {
                if (state == null)
                    continue;

                bool canOpen = state.GetStateType() == inStateType;
                state.gameObject.SetActive(canOpen);

                if (canOpen)
                {
                    state.OpenState(inOpenParams);
                    openedState = state;
                }
            }

            _currentState = openedState;
            return _currentState;
        }

        public void BlockUI()
        {
            if (_blockingGameObject != null)
            {
                _blockingGameObject.gameObject.SetActive(true);
            }
        }

        public void UnblockUI()
        {
            if (_blockingGameObject != null)
            {
                _blockingGameObject.gameObject.SetActive(false);
            }
        }

        public void OpenNotifyPopup(string inNotifyMessage)
        {
            if (notifyPopup != null)
            {
                notifyPopup.Open(inNotifyMessage);
            }
        }

        public void OpenInfoPopup()
        {
            if (infoPopup != null)
            {
                infoPopup.gameObject.SetActive(true);
            }
        }
    }
}