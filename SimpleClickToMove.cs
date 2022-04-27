using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class SimpleClickToMove: MonoBehaviour {

public bool Enabled = true;
public int MovementMouseButton;
public bool AutoFindCamera = true;
public Camera NavigationCamera;
public float checktempo = 0.1f;

private NavMeshAgent agent=null;
private WaitForSeconds tempo;


void Start(){
  if(NavigationCamera == null && AutoFindCamera){
    NavigationCamera = Camera.main;
  } else {
    Debug.Log(this.gameObject.name + " Simple Click To Move Script: No Camera Assigned");
  }
  if(this.gameObject.GetComponent<NavMeshAgent>()){
    agent = this.gameObject.GetComponent<NavMeshAgent>();
  } else {
    Debug.Log(this.gameObject.name + " Simple Click To Move Script: No Navmesh Agent");
  }
  tempo = new WaitForSeconds(checktempo);
}

private void Move(Vector3 position){
  if(!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)){
    return;
  } else {
    agent.SetDestination(hit.position);
  }
}

public void SetEnabled(bool e){
  Enabled = e;
}

private IEnumerator WaitForClick(){
  while(true){
    if(Input.GetMouseButtonDown(MovementMouseButton) && Enabled){
      Ray ray = NavigationCamera.ScreenPointToRay(Input.mousePosition);
      if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)){
        Move(hit.point);
      }
    }
    yield return tempo;
  }
}

}
