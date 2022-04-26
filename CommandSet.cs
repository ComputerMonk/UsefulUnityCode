using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class CommandSet{
[Header("Title")]
public string Name;
[Header("Description, what does this set of cammands do?")]
public string Description;
[Header("Commands")]
public List<TriggerCallCommand> Commands = new List<TriggerCallCommand>();
}


[System.Serializable]
public class TriggerCallCommand{
  public enum ObjectToCheck{
    ThisObject,
    ExternalObject,
    CollidedObject
  }
  [Header("Where To Check")]
  public ObjectToCheck SearchIn;
  public GameObject ExternalObjectToCheck = null;
  [Header("Function To Call")]
  public string ComponentName;
  public string FunctionName;
  [Header("Parameters To Send")]
  public List<Parameter> Parameters = new List<Parameter>();

  public int ReturnCheckType(){
    if(SearchIn == ObjectToCheck.ThisObject ){ return 0;}
    if(SearchIn == ObjectToCheck.ExternalObject ){ return 1;}
    if(SearchIn == ObjectToCheck.CollidedObject){ return 2;} else { return 0;}
  }
}

[System.Serializable]
public class Parameter{
  public enum ParameterType{
    OptionalMissing,
    ThisObject,
    ExternalObject,
    Float,
    Int,
    Bool,
    String
  }
  public ParameterType ParameterTypeToSend;
  public float FloatToSend = 0f;
  public bool BoolToSend = false;
  public int IntToSend = 0;
  public string StringToSend = "";
  public GameObject ExternalObjectToSend = null;

  public int ReturnParameterType(){
    if(ParameterTypeToSend == ParameterType.ThisObject ){ return 0;}
    if(ParameterTypeToSend == ParameterType.ExternalObject){ return 1;}
    if(ParameterTypeToSend == ParameterType.Float ){ return 2;}
    if(ParameterTypeToSend == ParameterType.Int){ return 3;}
    if(ParameterTypeToSend == ParameterType.Bool ){ return 4;}
    if(ParameterTypeToSend == ParameterType.String){ return 5;}
    if(ParameterTypeToSend == ParameterType.String){ return 6;} else { return 0;}
  }
}
