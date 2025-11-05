using System;
using UnityEngine;

namespace RSG.Core
{
    public interface IPoolable<TMonoBehaviour, TEnum>
        where TMonoBehaviour : MonoBehaviour 
        where TEnum : Enum
    {
        PoolService<TMonoBehaviour,  TEnum> PoolService { get; set; }
        TEnum PoolType { get; set; }
    }
}