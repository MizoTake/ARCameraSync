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

	private const float LIMIT_ROT = 80.0f;

	[SerializeField]
	private RectTransform[] topUI;
	[SerializeField]
	private RectTransform[] bottomUI;
	[SerializeField]
	private RectTransform[] rightUI;
	[SerializeField]
	private RectTransform[] leftUI;
	private ReactiveProperty<float> triggerRotationX = new ReactiveProperty<float> ();
	private ReactiveProperty<float> triggerRotationY = new ReactiveProperty<float> ();

	// Use this for initialization
	void Start ()
	{
		Input.gyro.enabled = true;
		var animTime = 0.3f;

		triggerRotationX
			.Subscribe (_ =>
			{
				foreach (var ui in topUI)
				{
					ui.DORotate (Vector3.right * (LIMIT_ROT - _), animTime).Play ();
					ui.DOAnchorPosY ((LIMIT_ROT - _), animTime).Play ();
				}
				foreach (var ui in bottomUI)
				{
					ui.DORotate (Vector3.right * _, animTime).Play ();
					ui.DOAnchorPosY (-_, animTime).Play ();
				}
			})
			.AddTo (this);

		triggerRotationY
			.Subscribe (_ =>
			{
				foreach (var ui in rightUI)
				{
					ui.DORotate (Vector3.up * _, animTime).Play ();
					ui.DOAnchorPosX (_, animTime).Play ();
				}
				foreach (var ui in leftUI)
				{
					ui.DORotate (Vector3.up * (LIMIT_ROT - _), animTime).Play ();
					ui.DOAnchorPosX (-(LIMIT_ROT - _), animTime).Play ();
				}
			})
			.AddTo (this);
	}

	// Update is called once per frame
	void Update ()
	{
		var nextX = triggerRotationX.Value + Input.gyro.rotationRate.x * 10.0f;
		var nextY = triggerRotationY.Value + Input.gyro.rotationRate.y * 10.0f;
		triggerRotationX.Value = Mathf.Clamp (nextX, 0.0f, LIMIT_ROT);
		triggerRotationY.Value = Mathf.Clamp (nextY, 0.0f, LIMIT_ROT);
	}
}