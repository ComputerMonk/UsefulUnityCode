using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class ConversationPlayer : MonoBehaviour {
  public Transform Player;
  public string defaultConversation;
  public float maxConversationDistance;

  private string currentconversation;
  private string barkconversation;
  private bool conversing;
  private Transform conversant;
  private bool watchdistance;

  void Start(){
    currentconversation = defaultConversation;
    conversant = this.transform;
  }

  public void OnUse(Transform player){
    SetPlayerTransform(player);
    StartConversation();
  }


  public void SetWatchDistance(bool set){
      watchdistance = set;
  }

  public void SetPlayerTransform(Transform player){
    Player = player;
  }

  public void SetConversation(string conversationname){
    currentconversation = conversationname;
  }

  public string GetCurrentConversation(){
    return currentconversation;
  }

  public void SetBarkConversation(string conversationname){
    barkconversation = conversationname;
  }

  public string GetBarkConversation(){
    return barkconversation;
  }

  public void StartConversation(){
    if(!conversing){
      conversing = true;
      DialogueManager.StartConversation(currentconversation, Player, conversant);
      if(watchdistance){StartCoroutine(WatchDistance());}
    }
  }

  public void EndConversation(){
    DialogueManager.StopConversation();
    conversing = false;
  }

  private IEnumerator WatchDistance(){
    while((Player.position-conversant.position).magnitude <= maxConversationDistance){
      yield return null;
    }
    EndConversation();
  }

  public void Bark(){
    DialogueManager.Bark(barkconversation, conversant, Player);
  }

  public void BarkLine(string line){
    DialogueManager.BarkString(line, conversant, Player);
  }

}
