using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FirstPersonScrollDoor: MonoBehaviour {

  public UnityEvent OnOpen;
  public UnityEvent OnClose;
  public bool Locked;
  public float Speed;
  public float LeftLimit;
  public float RightLimit;
  public enum ClosedLimits{
    LeftLimit,
    RightLimit
  }
  public ClosedLimits ClosedLimit;
  public AudioSource SoundSource;
  public AudioClip Squeak;
  public AudioClip Latch;


  private bool Active;
  private float currentrotationleft;
  private float currentrotationright;
  private bool openinvoked;
  private bool closeinvoked = true;
  private float currenttimesample = 0f;
  private float previoustimesample;
  private bool uptried = false;
  private bool downtried = false;

void Start(){
  SoundSource.clip = Squeak;
}
  void Update(){
    if(Active && !Locked){
      if(currentrotationleft + Speed <= LeftLimit){
        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            transform.Rotate(Vector3.down * Speed, Space.Self);
            StartCoroutine(PlaySqueak());
            currentrotationleft += Speed;
            currentrotationright -= Speed;
        }
      } else {
        switch(ClosedLimit){
          case ClosedLimits.LeftLimit:
            if(!closeinvoked){
              Debug.Log("Scroll Door: Door Closed");
              OnClose.Invoke();
              StartCoroutine(PlayLatch());
              openinvoked = false;
              closeinvoked = true;
            }
            break;
          case ClosedLimits.RightLimit:
            if(!openinvoked){
              Debug.Log("Scroll Door: Door Open");
              OnOpen.Invoke();
              openinvoked = true;
              closeinvoked = false;
            }
            break;
          default:
            break;
        }
      }
      if(currentrotationright + Speed <= RightLimit){
        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            transform.Rotate(Vector3.up * Speed, Space.Self);
            StartCoroutine(PlaySqueak());
            currentrotationleft -= Speed;
            currentrotationright += Speed;
        }
      } else {
        switch(ClosedLimit){
          case ClosedLimits.LeftLimit:
            if(!openinvoked){
              Debug.Log("Scroll Door: Door Open");
              OnOpen.Invoke();
              openinvoked = true;
              closeinvoked = false;
            }
            break;
          case ClosedLimits.RightLimit:
            if(!closeinvoked){
              Debug.Log("Scroll Door: Door Closed");
              OnClose.Invoke();
              StartCoroutine(PlayLatch());
              openinvoked = false;
              closeinvoked = true;
            }
            break;
          default:
            break;
        }
      }
    }
    if(Active && Locked){
      if(Input.GetAxis("Mouse ScrollWheel") > 0){
        if(uptried == false){
          uptried = true;
          downtried = false;
          StartCoroutine(PlayLatch());
          Debug.Log("Scroll Door: Door Locked");
        }
      }
      if(Input.GetAxis("Mouse ScrollWheel") < 0){
        if(downtried== false){
          downtried = true;
          uptried = false;
          StartCoroutine(PlayLatch());
          Debug.Log("Scroll Door: Door Locked");
        }
      }
    }
  }

 public void SetActive(bool active){
   Active = active;
 }

 public void SetLocked(bool locked){
   Locked = locked;
 }

 private IEnumerator PlaySqueak(){
   if(SoundSource != null && Squeak != null){
     SoundSource.Play();
     yield return new WaitForSeconds(0.25f);
     SoundSource.Pause();
     previoustimesample = currenttimesample;
     currenttimesample = SoundSource.timeSamples;
   }
 }

 private IEnumerator PlayLatch(){
   if(SoundSource != null && Squeak != null){
     SoundSource.PlayOneShot(Latch);
     yield return null;
   }
 }

}
