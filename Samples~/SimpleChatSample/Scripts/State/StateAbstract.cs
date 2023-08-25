// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using UnityEngine;

namespace Sendbird.Chat.Sample
{
    public abstract class StateAbstract : MonoBehaviour
    {
        public abstract class OpenParamsAbstract
        {
            
        }
        public abstract StateType GetStateType();

        public virtual void InitializeState() { }
        public virtual void OpenState(OpenParamsAbstract inOpenParamsAbstract) { }
        public virtual void CloseState() { }
        public virtual void TerminateState() { }
    }
}