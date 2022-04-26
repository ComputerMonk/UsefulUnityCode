using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputContextManager: MonoBehaviour {

  public static InputContextManager Instance;

  private List<InputContextComponent> configurableObjects = new List<InputContextComponent>();

  private void Awake() {
    if(Instance == null){
      Instance = this;
    } else if(Instance != this){
      Destroy(this);
    }
  }

  public void RegisterListener(InputContextComponent component){
    configurableObjects.Add(component);
  }

  public void RegisterListener(InputContextComponent component){
    configurableObjects.Remove(component);
  }

  public void ActivateExclusiveContext(string name){
    foreach(InputContextComponent c in configurableObjects){
      c.SetSingleContext(name);
    }
  }

  public void ActivateContext(string name){
    foreach(InputContextComponent c in configurableObjects){
      c.AddContext(name);
    }
  }

  public void DeactivateContext(string name){
    foreach(InputContextComponent c in configurableObjects){
      c.RemoveContext(name);
    }
  }

  public void DeactivateAllContexts(){
    foreach(InputContextComponent c in configurableObjects){
      c.ClearAllContexts();
    }
  }

  public void ReturnToDefaultContexts(){
    foreach(InputContextComponent c in configurableObjects){
      c.RevertToDefaultContexts();
    }
  }

}
