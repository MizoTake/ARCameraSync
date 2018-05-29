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
		var limitRot = 80.0f;

		triggerRotationX
			.Select (_ => Mathf.Clamp (_ * 10.0f, 0.0f, limitRot))
			.Subscribe (_ =>
			{
				foreach (var ui in topUI)
				{
					ui.DORotate (Vector3.right * (limitRot - _), animTime).Play ();
					ui.DOAnchorPosY ((limitRot - _), animTime).Play ();
				}
				foreach (var ui in bottomUI)
				{
					ui.DORotate (Vector3.right * _, animTime).Play ();
					ui.DOAnchorPosY (-_, animTime).Play ();
				}
			})
			.AddTo (this);

		triggerRotationY
			.Select (_ => Mathf.Clamp (_ * 10.0f, 0.0f, limitRot))
			.Subscribe (_ =>
			{
				foreach (var ui in rightUI)
				{
					ui.DORotate (Vector3.up * _, animTime).Play ();
					ui.DOAnchorPosX (_, animTime).Play ();
				}
				foreach (var ui in leftUI)
				{
					ui.DORotate (Vector3.up * (limitRot - _), animTime).Play ();
					ui.DOAnchorPosX (-(limitRot - _), animTime).Play ();
				}
			})
			.AddTo (this);
	}

	// Update is called once per frame
	void Update ()
	{
		triggerRotationX.Value += Input.gyro.rotationRate.x;
		triggerRotationY.Value += Input.gyro.rotationRate.y;
	}
}