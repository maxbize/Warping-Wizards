﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Skeletor : Unit {

  public GameObject attack;
  public GameObject magicPrefab;
  public Transform magicSpawnPoint;

  void Start() {

    fsm.AddStateName("Magic");

    attack = GameObject.FindGameObjectWithTag("Player");

    if (attack != null) {
      SetFollow(attack.transform);
    }
  }

  void Update() {

    animator.SetBool("isFollowing", IsFollowing());
    if (IsFollowing()) {
      animator.SetFloat("isFollowingDistance", Vector3.Distance(GetFollowing().position, transform.position));
    } else {
      animator.SetFloat("isFollowingDistance", Mathf.Infinity);
    }

    UpdateVelocity();
    UpdateAnimator();
  }

  void OnFsmStateChangeEvent(GameObject go, Fsm fsm, string stateName) {
    if (go == gameObject) {
      switch (stateName) {
        case "Idle":
          SetFollow(attack.transform);
          break;
        case "Run":
          break;
        case "Magic":
          SetFollow(null);
          StartCoroutine(delayedMagicSpawn());
          break;
        
      }
    }
  }

  protected override void OnEnable() {
    base.OnEnable();
    Fsm.OnFsmStateChangeEvent += OnFsmStateChangeEvent;
  }

  protected override void OnDisable() {
    base.OnEnable();
    Fsm.OnFsmStateChangeEvent -= OnFsmStateChangeEvent;
  }

  IEnumerator delayedMagicSpawn() {
    yield return new WaitForSeconds(0.5f);

    // mouse pos
    // position
    Vector2 direction = attack.transform.position - magicSpawnPoint.position;
    direction.Normalize();

    if (hp > 0) {
      if (magicPrefab) {
        Instantiate(magicPrefab, magicSpawnPoint.position, Quaternion.LookRotation(direction));
      }
    }
    yield return null;
  }
}
