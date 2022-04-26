using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public class ContextListener{
  public string ContextName;
  public bool Active;
  public bool InDefault;
  public UnityEvent ConfigurationOnEnabled;
  public UnityEvent ConfigurationOnDisabled;
}
