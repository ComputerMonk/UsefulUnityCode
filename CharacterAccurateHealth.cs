using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Invector;

public class CharacterAccurateHealth: MonoBehaviour {
  public bool Active;
  public Animator CharacterAnimator;
  public vHealthController HealthController;
  public List<BodyPart> BodyParts = new List<BodyPart>();
  public List<DamageEffect> DamageEffects = new List<DamageEffect>();
  public List<Poison> PossiblePoisons = new List<Poison>();
  public float EffectUpdateTempo;

  private float totalbleedrate;
  private float totalhealrate;
  private float totalpoisonrate;
  private Animator animator;

  void Start()
  {
      if(CharacterAnimator == null){
        animator = GetComponent<Animator>();
      } else {
        animator = CharacterAnimator;
      }
     foreach(var damageReceiver in  GetComponentsInChildren<vIDamageReceiver>(true)){
      var g = damageReceiver.gameObject;
      if (g != gameObject){
       damageReceiver.onReceiveDamage.AddListener((damage) =>{
              if(g.GetComponent<AccurateHealthLabel>()){
                ProcessVDamage(g.GetComponent<AccurateHealthLabel>().BodyPartType, damage.damageValue);
              }
          });
      }
    }
    StartCoroutine(DoHealth());
    StartCoroutine(DoEffects());
  }

  public float GetTotalBloodLoss(){
    return totalbleedrate;
  }

  public float GetTotalHealing(){
    return totalhealrate;
  }

  public void SetActive(bool set){
    Active = set;
  }

  public void Poison(string name, float amount){
    foreach(Poison p in PossiblePoisons){
      if(p.Name == name){
        p.AddPoison(amount);
        break;
      }
    }
  }

  public void AddBodyPartDamage(string command){
      string[] parsedcommand = command.Split(',');
      foreach(BodyPart b in BodyParts){
        if(b.Name == parsedcommand[0]){
          b.AddDamage(float.Parse(parsedcommand[1]));
          break;
        }
      }
  }

  private void ProcessVDamage(string part, float damage){
      foreach(BodyPart b in BodyParts){
        if(b.Name == part){
          b.AddDamage(damage);
          break;
        }
      }
  }

public void AnimatorSetTrigger(string trigger){
  animator.SetTrigger(trigger);
}

public void AnimatorSetInt(string param, int set){
  animator.SetInteger(param, set);
}

public void AnimatorSetFloat(string param, float set){
  animator.SetFloat(param,set);
}

public void AnimatorSetBool(string param, bool set){
  animator.SetBool(param, set);
}

  public float GetBodyPartDamage(string name){
    foreach(BodyPart b in BodyParts){
      if(b.Name == name){
        return b.GetDamage();
      }
    }
    return 0f;
  }

  public void ActivatePoison(string name){
    foreach(Poison p in PossiblePoisons){
      if(p.Name == name){
        p.Active = true;
      }
    }
  }

  public bool GetPoisonActive(string name){
    foreach(Poison p in PossiblePoisons){
      if(p.Name == name){
        return p.Active;
      }
    }
    return false;
  }

  public float GetPoisonAmount(string name){
    foreach(Poison p in PossiblePoisons){
      if(p.Name == name){
        if(p.Active){
          return p.PoisonLeft();
        } else {
          return 0f;
        }
      }
    }
    return 0f;
  }

  public bool GetIsPoisoned(){
    foreach(Poison p in PossiblePoisons){
      if(p.Active){
        return p.Active;
      }
    }
    return false;
  }

  private IEnumerator DoHealth(){
    WaitForSeconds tempo = new WaitForSeconds(1);
    while(true){
      if(Active){
        float healingrate = 0f;
        float bleedrate = 0f;
        float poisonrate = 0f;
        foreach(BodyPart b in BodyParts){
          if(b.GetDamage()>0){
            if(b.GetDamage() >= b.BleedDamageThreshold){bleedrate += b.BleedRateMultiplier*b.GetDamage();}
            healingrate += b.HealRatePerSecond;
            b.SubtractDamage(b.HealRatePerSecond);
          }
        }
        foreach(Poison p in PossiblePoisons){
          if(p.Active){
            p.DissipatePoison();
            poisonrate += p.GetPoisonDamage();
          }
        }
        totalbleedrate = bleedrate;
        totalhealrate = healingrate;
        totalpoisonrate = poisonrate;
        HealthController.AddHealth((int)totalhealrate);
        int subtractedhealth = (int)totalbleedrate + (int)totalpoisonrate;
        HealthController.ChangeHealth((int)HealthController.currentHealth - subtractedhealth);
      }
      yield return tempo;
    }
  }

  private IEnumerator DoEffects(){
    WaitForSeconds tempo = new WaitForSeconds(EffectUpdateTempo);
    while(true){
      foreach(DamageEffect d in DamageEffects){
        bool conditionsmet = true;
        foreach(DamageEffectFactor e in d.Factors){
          switch(e.ReturnFactorType()){
            case 0:
              if(e.ReturnThresholdType() == 0){
                if(GetBodyPartDamage(e.PoisonOrBodyPart) < e.Threshold){
                  conditionsmet = false;
                }
              } else if(e.ReturnThresholdType() == 1){
                if(GetBodyPartDamage(e.PoisonOrBodyPart) != e.Threshold){
                  conditionsmet = false;
                }
              } else if(e.ReturnThresholdType() == 2){
                if(GetBodyPartDamage(e.PoisonOrBodyPart) > e.Threshold){
                  conditionsmet = false;
                }
              }
              break;
            case 1:
              if(e.ReturnThresholdType() == 0){
                if(totalbleedrate < e.Threshold){
                  conditionsmet = false;
                }
              } else if(e.ReturnThresholdType() == 1){
                if(totalbleedrate != e.Threshold){
                  conditionsmet = false;
                }
              } else if(e.ReturnThresholdType() == 2){
                if(totalbleedrate > e.Threshold){
                  conditionsmet = false;
                }
              }
              break;
            case 2:
              if(e.ReturnThresholdType() == 0){
                if(totalhealrate < e.Threshold){
                  conditionsmet = false;
                }
              } else if(e.ReturnThresholdType() == 1){
                if(totalhealrate != e.Threshold){
                  conditionsmet = false;
                }
              } else if(e.ReturnThresholdType() == 2){
                if(totalhealrate > e.Threshold){
                  conditionsmet = false;
                }
              }
              break;
            case 3:
              if(e.ReturnThresholdType() == 0){
                if(HealthController.currentHealth < e.Threshold){
                  conditionsmet = false;
                }
              } else if(e.ReturnThresholdType() == 1){
                if(HealthController.currentHealth != e.Threshold){
                  conditionsmet = false;
                }
              } else if(e.ReturnThresholdType() == 2){
                if(HealthController.currentHealth > e.Threshold){
                  conditionsmet = false;
                }
              }
              break;
            case 4:
              if(e.ReturnThresholdType() == 0){
                if(GetPoisonAmount(e.PoisonOrBodyPart) < e.Threshold){
                  conditionsmet = false;
                }
              } else if(e.ReturnThresholdType() == 1){
                if(GetPoisonAmount(e.PoisonOrBodyPart) != e.Threshold){
                  conditionsmet = false;
                }
              } else if(e.ReturnThresholdType() == 2){
                if(GetPoisonAmount(e.PoisonOrBodyPart) > e.Threshold){
                  conditionsmet = false;
                }
              }
              break;
            default:
              break;
          }
        }
        if(conditionsmet == true && d.getconditionsmet()==false){
          d.OnConditionsMet.Invoke();
          switch(d.ReturnAnimatorChange()){
            case 0:
            break;
            case 1:
            AnimatorSetTrigger(d.AnimatorParameterOrTrigger);
            break;
            case 2:
            AnimatorSetFloat(d.AnimatorParameterOrTrigger, d.AnimatorFloat);
            break;
            case 3:
            AnimatorSetInt(d.AnimatorParameterOrTrigger, d.AnimatorInteger);
            break;
            case 4:
            AnimatorSetBool(d.AnimatorParameterOrTrigger, d.AnimatorBool);
            break;
            default:
            break;
          }
          d.setconditionsmet(true);
        } else if(conditionsmet == false && d.getconditionsmet()==true){
          d.OnConditionsNotMet.Invoke();
          d.setconditionsmet(false);
        }

      }
     yield return tempo;
    }
  }

}

[System.Serializable]
public class BodyPart{
  public string Name;
  public float BleedRateMultiplier;
  public float BleedDamageThreshold;
  public float HealRatePerSecond;

  private float totaldamage;

  public void AddDamage(float amount){
    totaldamage += amount;
  }

  public void SubtractDamage(float amount){
    totaldamage -= amount;
  }

  public float GetDamage(){
    return totaldamage;
  }

}

[System.Serializable]
public class DamageEffect{

  public enum AnimatorReaction {
    None,
    Trigger,
    Float,
    Integer,
    Bool
  }
  public AnimatorReaction AnimatorChange;
  public string AnimatorParameterOrTrigger;
  public int AnimatorInteger;
  public float AnimatorFloat;
  public bool AnimatorBool;
  public UnityEvent OnConditionsMet;
  public UnityEvent OnConditionsNotMet;
  public List<DamageEffectFactor> Factors = new List<DamageEffectFactor>();

  private bool conditionsmet;

  public void setconditionsmet(bool set){
    conditionsmet = set;
  }

  public bool getconditionsmet(){
    return conditionsmet;
  }

  public int ReturnAnimatorChange(){
    if(AnimatorChange == AnimatorReaction.None){ return 0;}
    if(AnimatorChange == AnimatorReaction.Trigger){ return 1;}
    if(AnimatorChange == AnimatorReaction.Float){ return 2;}
    if(AnimatorChange == AnimatorReaction.Integer){ return 3;}
    if(AnimatorChange == AnimatorReaction.Bool){ return 4;} else { return 0;}
  }

}


[System.Serializable]
public class DamageEffectFactor{
  public enum FactorType{
    SpecificBodyPartDamage,
    TotalBloodLossPerSecond,
    TotalHealingPerSecond,
    TotalHealth,
    PoisonAmount
  }
  public FactorType TypeOfFactor;
  public string PoisonOrBodyPart;
  public enum ThresholdType{
    GreaterThan,
    EqualTo,
    LessThan
  }
  public ThresholdType MetWhen;
  public float Threshold;

  public int ReturnFactorType(){
    if(TypeOfFactor == FactorType.SpecificBodyPartDamage ){ return 0;}
    if(TypeOfFactor == FactorType.TotalBloodLossPerSecond){ return 1;}
    if(TypeOfFactor == FactorType.TotalHealingPerSecond ){ return 2;}
    if(TypeOfFactor == FactorType.TotalHealth){ return 3;}
    if(TypeOfFactor == FactorType.PoisonAmount ){ return 4;} else { return 0;}
  }

  public int ReturnThresholdType(){
    if(MetWhen == ThresholdType.GreaterThan ){ return 0;}
    if(MetWhen == ThresholdType.EqualTo){ return 1;}
    if(MetWhen == ThresholdType.LessThan ){ return 2;} else { return 0;}
  }
}

[System.Serializable]
public class Poison{
  public string Name;
  public bool Active;
  public float DamageAmountPerSecond;
  public float DissipationAmountPerSecond;

  private float currentpoisonamount;

  public void AddPoison(float amount){
    Active = true;
    currentpoisonamount = amount;
  }

  public float GetPoisonDamage(){
    if(currentpoisonamount > 0f){
      return DamageAmountPerSecond;
    } else {
      Active = false;
      return 0f;
    }
  }

  public float PoisonLeft(){
    return currentpoisonamount;
  }

  public void DissipatePoison(){
    currentpoisonamount -= DissipationAmountPerSecond;
  }

}
