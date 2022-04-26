using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class ClothingChanger: MonoBehaviour {
  public List<ClothingItem> ClothingItems = new List<ClothingItem>();
  public List<Outfit> Outfits = new List<Outfit>();
  public UnityEvent OnOutfitChanged;

  private List<string> activeoutfits = new List<string>();

  public void ActivateOutfit(string outfitname){
    foreach(Outfit o in Outfits){
      if(o.Name == outfitname && o.GetActive() == false){
        foreach(string act in o.ActivatesClothingItems){
          foreach(ClothingItem c in ClothingItems){
            if(c.Name == act){
              c.OnEnabled.Invoke();
              c.Mesh.gameObject.SetActive(true);
            }
          }
        }
        foreach(string dea in o.DeactivatesClothingItems){
          foreach(ClothingItem c in ClothingItems){
            if(c.Name == dea){
              c.OnDisabled.Invoke();
              c.Mesh.gameObject.SetActive(false);
            }
          }
        }
       OnOutfitChanged.Invoke();
       o.PutOn(true);
       break;
      } else {
       o.PutOn(false);
      }
    }
  }

  public List<string> GetActiveOutfits(){
    activeoutfits.Clear();
    foreach(Outfit o in Outfits){
      if(o.GetActive){activeoutfits.Add(o.Name);}
    }
    return activeoutfits;
  }

}

[System.Serializable]
public class ClothingItem{
  public string Name;
  public Transform Mesh;
  public UnityEvent OnEnabled;
  public UnityEvent OnDisabled;
}
