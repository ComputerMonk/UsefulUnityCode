using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommandProxyTrigger: MonoBehaviour {
  public bool Enabled = true;
  public List<string> DetectedTags = new List<string>();
  public AudioSource audiosource = null;

  [Header("When Objects Enter")]
  public AudioClip ObjectEnterAudio = null;
  public UnityEvent OnObjectEnter;
  public int DefaultEnterCommandSetIndex;
  public List<CommandSet> OnEnterCommandSets = new List<CommandSet>();


  [Header("When Objects Exit")]
  public AudioClip ObjectExitAudio = null;
  public UnityEvent OnObjectExit;
  public int DefaultExitCommandSetIndex;
  public List<CommandSet> OnExitCommandSets = new List<CommandSet>();

  [Header("Debug")]
  public bool DebugHit;

 private CommandSet activeentercommandset;
 private CommandSet activeexitcommandset;

  void Start(){
    if(OnEnterCommandSets.Count > 0){activeentercommandset = OnEnterCommandSets[DefaultEnterCommandSetIndex];}
    if(OnExitCommandSets.Count > 0){activeexitcommandset = OnExitCommandSets[DefaultExitCommandSetIndex];}
  }

public void ChangeActiveEntranceCommandSet(string title){
  foreach(CommandSet set in OnEnterCommandSets){
    if(set.Name == title){
      if(debugon){Debug.Log("Successfully Changed On Enter Command Set")}
      activeentercommandset = set;
      break;
    }
  }
}

public void ChangeActiveExitCommandSet(string title){
  foreach(CommandSet set in OnEnterCommandSets){
    if(set.Name == title){
      if(debugon){Debug.Log("Successfully Changed On Enter Command Set")}
      activeentercommandset = set;
      break;
    }
  }
}

// Enable or disable the trigger
  public void Enable(bool enabled){
    Enabled = enabled;
  }

// Adds a detectable tag via script
  public void AddTag(string tag){
    DetectedTags.Add(tag);
  }

// Removes a detectable tag by script
  public void RemoveTag(string tag){
    var index = 0;
    foreach(string t in DetectedTags){
      if(t == tag){
        DetectedTags.RemoveAt(index);
      }
      index ++;
    }
  }

// When an object enters the trigger;
  private void OnTriggerEnter(Collider other){
    // Does it have a recognized tag?
    if(DetectedTags.Contains(other.gameObject.tag) && Enabled == true){
      if(DebugHit){Debug.Log(this.gameObject.name + " Trigger Script: Hit " + other.gameObject.name + " Tagged " + other.gameObject.tag);}
      // If we have audio play it
      if(audiosource != null && ObjectEnterAudio != null){audiosource.PlayOneShot(ObjectEnterAudio);}
      if(OnEnterCommandSets.Count > 0){
        foreach(TriggerCallCommand t in activeentercommandset.Commands){
          EvaluateCommand(t,other);
        }
      }
      OnObjectEnter.Invoke();
    }
  }

  private void OnTriggerExit(Collider other){
    if(DetectedTags.Contains(other.gameObject.tag) && Enabled == true){
      // If we have audio play it
      if(audiosource != null && ObjectExitAudio != null){audiosource.PlayOneShot(ObjectExitAudio);}
      // If spawnable object actions exist
      if(OnExitCommandSets.Count > 0){
        foreach(TriggerCallCommand t in activeexitcommandset.Commands){
          EvaluateCommand(t,other);
        }
      }
      OnObjectExit.Invoke();
    }
  }

  private void EvaluateCommand(TriggerCallCommand comm, Collider other){
    int check = comm.ReturnCheckType();
    if(check == 0){
      if(this.gameObject.GetComponent(comm.ComponentName)){
        object[] parameters = BuildParameterList(comm);
        InvokeInThisObject(comm.ComponentName, comm.FunctionName, parameters);
      }
    } else if(check == 1){
      if(comm.ExternalObjectToCheck.GetComponent(comm.ComponentName)){
        object[] parameters = BuildParameterList(comm);
        InvokeInExternalObject(comm.ExternalObjectToCheck, comm.ComponentName, comm.FunctionName, parameters);
      }
    } else if(check == 2){
      if(other.gameObject.GetComponent(comm.ComponentName)){
        object[] parameters = BuildParameterList(comm);
        InvokeInExternalObject(other.gameObject, comm.ComponentName, comm.FunctionName, parameters);
      }
    }
  }

private object[] BuildParameterList(TriggerCallCommand comm){
    object[] paramarr = new object[comm.Parameters.Count];
    int index = 0;
    foreach(Parameter param in comm.Parameters){
      switch(param.ReturnParameterType()){
        case 0:
          paramarr[index] = this;
          break;
        case 1:
          paramarr[index] = param.ExternalObjectToSend;
          break;
        case 2:
          paramarr[index] = param.FloatToSend;
          break;
        case 3:
         paramarr[index] = param.IntToSend;
         break;
        case 4:
          paramarr[index] = param.BoolToSend;
         break;
        case 5:
          paramarr[index] = param.StringToSend;
          break;
        case 6:
          paramarr[index] = Type.Missing;
          break;
        default:
          break;
      }
    }
    index ++;
    return paramarr;
  }

private void InvokeInThisObject(string componentname, string functionname, object[] arguments){
  var component = this.gameObject.GetComponent(componentname);
  var loadingMethod = component.GetType().GetMethod(functionname);
  if(arguments.Length > 0){
    loadingMethod.Invoke(component, arguments);
  } else {
    loadingMethod.Invoke(component, null);
  }
}

private void InvokeInExternalObject(GameObject externalobject, string componentname, string functionname, object[] arguments){
  var component = externalobject.GetComponent(componentname);
  var loadingMethod = component.GetType().GetMethod(functionname);
  if(arguments.Length > 0){
    loadingMethod.Invoke(component, arguments);
  } else {
    loadingMethod.Invoke(component, null);
  }
}

}
