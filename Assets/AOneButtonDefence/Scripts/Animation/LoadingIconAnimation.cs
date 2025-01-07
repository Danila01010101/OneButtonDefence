using System;
using DG.Tweening;
using UnityEngine;

	public class LoadingIconAnimation : MonoBehaviour
	{
		[SerializeField] private float animationSpeed = 2;
		
		private void Update()
		{
			transform.Rotate(Vector3.back, animationSpeed);
		}
	}