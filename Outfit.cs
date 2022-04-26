using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Computer Monk Assets/Clothing Changer/New Outfit")]
public class Outfit : ScriptableObject{
  public string Name;
  public List<string> ActivatesClothingItems = new List<string>();
  public List<string> DeactivatesClothingItems = new List<string>();

  private bool active;

  public bool GetActive(){
    return active;
  }

  public void PutOn(bool dir){
    active = dir;
  }
}
