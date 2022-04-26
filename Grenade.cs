using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grenade: MonoBehaviour {

  public float Delay = 4f;
  public UnityEvent OnDetonate;
  public bool DeleteOnExplode = true;
  public GameObject ExplosionPrefab;

  private float countdown;
  private bool exploded;
  private bool pinpulled = false;

  void Start(){
    countdown = delay;
  }

  public void PullPin(){
    if(!pinpulled){
      StartCoroutine(fuse());
      pinpulled = true;
    }
  }

  private IEnumerator fuse(){
    while(!exploded){
      countdown -= Time.deltaTime;
      if(countdown <= 0f){
        Explode();
      }
      yield return null;
    }
  }

  public void Explode(){
      Debug.Log("boom");
      Instantiate(ExplosionPrefab, this.transform.position, this.transform.rotation);
      OnDetonate.Invoke();
      if(DeleteOnExplode){
        Destroy(this);
      } else {
        exploded = true;
      }
  }
}
