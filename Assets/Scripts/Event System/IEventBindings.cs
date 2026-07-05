using System;
using UnityEngine;

public interface IEventBindings<T>
{
    Action<T> OnEvent { get; set; }
    Action OnEventNoArgs { get; set; }
}
