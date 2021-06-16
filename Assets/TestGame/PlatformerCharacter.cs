using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerCharacter : MonoBehaviour {
    public float minJumpHeight = 1f;
    public float maxJumpHeight = 3f;
    public float timeToReachApex = 1f;
    
    
    
    public bool jumpPressing;
    public Vector2 input;
    public bool _jumping = false;
    public bool _grounded = false;
    public float _jumpStartTime = 0f;
    private Rigidbody2D _body;

    private void Awake() {
        _body = GetComponent<Rigidbody2D>();
    }
    private void Update() {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        jumpPressing = Input.GetButton("Jump");
    }
    private void FixedUpdate() {
        if (jumpPressing && _grounded) {
            _grounded = false;
            _jumping = true;
            _jumpStartTime = Time.fixedTime;
            _body.velocity = new Vector2(_body.velocity.x, maxJumpHeight / timeToReachApex);
        }
        if (_jumping) {
            _body.AddForce(-Physics2D.gravity);
        }
        if (_jumping && Time.fixedTime - _jumpStartTime > timeToReachApex) {
            _jumping = false;
            _body.velocity = new Vector2(_body.velocity.x, 0f);

        }
        if (_jumping && !jumpPressing && Time.fixedTime - _jumpStartTime > (minJumpHeight / maxJumpHeight) * timeToReachApex) {
            _jumping = false;
            _body.velocity = new Vector2(_body.velocity.x, 0f);
        }
    }
    // private void OnCollisionEnter2D(Collision2D other) {
    //     //im grounded if i hit something and our relative velocities was towards down
    //     if (Vector3.Dot(Vector3.down, other.relativeVelocity) >= 0) {
    //         _grounded = true;
    //     }
    //     
    // }
    private void OnTriggerStay2D(Collider2D other) {
        _grounded = true;
    }
}
