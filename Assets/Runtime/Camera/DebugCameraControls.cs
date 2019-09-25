using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InThePocket.Utility
{
	[RequireComponent(typeof(Camera))]
	public class DebugCameraControls : MonoBehaviour {
	
		public bool Active {
			get { return _active; }

			set {
				_active = value;

				if (_active) {
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
				}
				else {
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
				}
			}
		}

		[SerializeField] private float _cameraSensitivity = 90;
		[SerializeField] private float _climbSpeed = 2;
		[SerializeField] private float _normalMoveSpeed = 4f;
		[SerializeField] private float _slowMoveFactor = 0.2f;
		[SerializeField] private float _fastMoveFactor = 5;
		[SerializeField] private float _rotationX = 0.0f;
		[SerializeField] private float _rotationY = 0.0f;

		private bool _active = false;

		public void Start() {
			if (Application.isEditor) {
				Debug.Log("<color=#ff00ff>Enable debug camera controls by pressing TAB.</color>");
			}
		}

		public void Update() {
			if (Application.isEditor && Input.GetKeyDown (KeyCode.Tab)) {
				Active = !Active;
			}

			if (Active) {
				var camera = gameObject.GetComponent<Camera>();	

				_rotationX += Input.GetAxis("Mouse X") * _cameraSensitivity * Time.deltaTime;
				_rotationY += Input.GetAxis("Mouse Y") * _cameraSensitivity * Time.deltaTime;

				
				_rotationY = Mathf.Clamp (_rotationY, -90, 90);

				camera.transform.localRotation = Quaternion.AngleAxis(_rotationX, Vector3.up);
				camera.transform.localRotation *= Quaternion.AngleAxis(_rotationY, Vector3.left);
		
				var moveSpeed = _normalMoveSpeed;
				
				if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
					moveSpeed *= _fastMoveFactor;
				}
				else if (Input.GetKey (KeyCode.LeftAlt) || Input.GetKey (KeyCode.RightAlt)) {
					moveSpeed *= _slowMoveFactor;
				}

				camera.transform.position += camera.transform.forward * moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
				camera.transform.position += camera.transform.right * moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
		
				if (Input.GetKey (KeyCode.Space)) {
					camera.transform.position += camera.transform.up * _climbSpeed * Time.deltaTime;
				}

				if (Input.GetKey (KeyCode.LeftControl)) {
					camera.transform.position -= camera.transform.up * _climbSpeed * Time.deltaTime;
				}
			}
		}

	}
		
}
