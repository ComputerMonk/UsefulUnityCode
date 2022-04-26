using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputContextComponent: MonoBehaviour{

  public List<ContextListener> Listeners = new List<ContextListener>();
  public bool debug;

  void OnStart(){
    InputContextManager.Instance.RegisterListener(this);
    RevertToDefaultContexts();
  }

  void OnDestroy(){
    InputContextManager.Instance.DeregisterListener(this);
  }

  public void SetSingleContext(string name){
    StartCoroutine(SetOneContext(name));
  }

  public void AddContext(string name){
    StartCoroutine(ChangeContexts(false, name));
  }

  public void RemoveContext(string name){
    StartCoroutine(ChangeContexts(true, name));
  }

  public void ClearAllContexts(){
      foreach(ContextListener c in Listeners){
        deactivateConfig(c);
      }
  }

  public void RevertToDefaultContexts(){
    StartCoroutine(SetDefaultContexts());
  }

  private IEnumerator SetOneContext(string name){
   yield return 0;
   foreach(ContextListener c in Listeners ){
     if(c.ContextName == name){
       activateConfig(c);
     } else {
       deactivateConfig(c);
     }
   }
  }

  private IEnumerator ChangeContexts(bool off, string name){
    yield return 0;
    if(off){
      foreach(ContextListener c in Listeners ){
        if(c.ContextName == name){
          deactivateConfig(c);
        }
      }
    } else {
      foreach(ContextListener c in Listeners ){
        if(c.ContextName == name){
          activateConfig(c);
        }
      }
    }
  }

  private IEnumerator SetDefaultContexts(){
    yield return 0;
    foreach(ContextListener c in Listeners ){
      if(c.InDefault){
        activateConfig(c);
      } else {
        deactivateConfig(c);
      }
    }
  }

  private void activateConfig(ContextListener c){
    if(c.Active == false){
      c.Active = true;
      c.ConfigurationOnEnabled.Invoke();
      if(debug){Debug.Log(this.gameObject.name + " InputContextComponent: Activating " + c.ContextName);}
    }
  }

  private void deactivateConfig(ContextListener c){
    if(c.Active){
      c.Active = false;
      c.ConfigurationOnDisabled.Invoke();
      if(debug){Debug.Log(this.gameObject.name + " InputContextComponent: Deactivating " + c.ContextName);}
    }
  }

}
