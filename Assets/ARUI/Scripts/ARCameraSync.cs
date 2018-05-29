using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ARCameraSync : UIBehaviour
{

	[SerializeField]
	private RectTransform[] planes;
	private ReactiveProperty<float> triggerRotationX = new ReactiveProperty<float> ();
	private ReactiveProperty<float> triggerRotationY = new ReactiveProperty<float> ();

	// Use this for initialization
	void Start ()
	{
		Input.gyro.enabled = true;
		var animTime = 0.3f;
		var limitRot = 60.0f;

		triggerRotationX
			.Subscribe (_ =>
			{
				var x = Mathf.Clamp (_ * 10.0f, 0.0f, limitRot);
				planes[0].DORotate (Vector3.right * (limitRot - x), animTime);
				planes[1].DORotate (Vector3.right * x, animTime);
			})
			.AddTo (this);

		triggerRotationY
			.Subscribe (_ =>
			{
				var y = Mathf.Clamp (_ * 10.0f, 0.0f, limitRot);
				planes[2].DORotate (Vector3.up * y, animTime);
				planes[3].DORotate (Vector3.up * (limitRot - y), animTime);
			})
			.AddTo (this);
	}

	// Update is called once per frame
	void Update ()
	{
		triggerRotationX.Value += Input.gyro.rotationRate.x;
		triggerRotationY.Value += Input.gyro.rotationRate.y;
	}

	protected void OnGUI ()
	{
		GUI.skin.label.fontSize = Screen.width / 40;

		GUILayout.Label ("Orientation: " + Screen.orientation);
		GUILayout.Label ("input.gyro.attitude: " + Input.gyro.attitude);
		GUILayout.Label ("Input.gyro.rotationRate: " + Input.gyro.rotationRate);
		GUILayout.Label ("GyroToUnity (Input.gyro.attitude).eulerAngles: " + GyroToUnity (Input.gyro.attitude).eulerAngles);
	}

	private Quaternion GyroToUnity (Quaternion q)
	{
		return new Quaternion (q.x, q.y, -q.z, -q.w);
	}
}