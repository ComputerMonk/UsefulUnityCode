using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using CrazyMinnow.SALSA;

public class DialogueEmoterLink: MonoBehaviour {

public List<Emoter> EmoterList = new List<Emoter>();

public void TriggerExpression(double emoter,string emotion, double duration){
  float speed = (float)duration;
  int index = (int)emoter;
  EmoterList[index].ManualEmote(emotion, ExpressionComponent.ExpressionHandler.RoundTrip, speed);
}

void OnEnable(){
    Lua.RegisterFunction("TriggerExpression", this, SymbolExtensions.GetMethodInfo(() => TriggerExpression((double)0,string.Empty,(double)1)));
}

}
