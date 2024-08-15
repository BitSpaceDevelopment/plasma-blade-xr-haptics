﻿using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

/// <summary>
/// Off screen indicator.
/// Classic wrapper, user doesn't need to worry about implementation
/// </summary>
namespace Greyman{
	public class OffScreenIndicator : MonoBehaviour {
		public static OffScreenIndicator Instance;
		public bool	enableDebug = true;
		public bool VirtualRealitySupported = true;
		public float VR_cameraDistance = 5;
		public float VR_radius = 1.8f;
		public float VR_indicatorScale = 0.1f;
		public GameObject canvas;
		public int Canvas_circleRadius = 5; //size in pixels
		public int Canvas_border = 10; // when Canvas is Square pixels in border
		public int Canvas_indicatorSize = 100; //size in pixels
		public Indicator[] indicators;
		public List<FixedTarget> Targets;
		public List<GameObject> DroidsAlive;
		//public 
		private OffScreenIndicatorManager manager;

		void Awake () {
			if (Instance != null && Instance != this)
        	{
           		Destroy(this);
        	}
        	else
        	{
            	Instance = this;
        	}

			if(VirtualRealitySupported){
				manager = gameObject.AddComponent<OffScreenIndicatorManagerVR>();
				(manager as OffScreenIndicatorManagerVR).cameraDistance = VR_cameraDistance;
				(manager as OffScreenIndicatorManagerVR).radius = VR_radius;
				(manager as OffScreenIndicatorManagerVR).indicatorScale = VR_indicatorScale;
				(manager as OffScreenIndicatorManagerVR).CreateIndicatorsParent();
			} else {
				manager = gameObject.AddComponent<OffScreenIndicatorManagerCanvas>();
				(manager as OffScreenIndicatorManagerCanvas).indicatorsParentObj = canvas;
				(manager as OffScreenIndicatorManagerCanvas).circleRadius = Canvas_circleRadius;
				(manager as OffScreenIndicatorManagerCanvas).border = Canvas_border;
				(manager as OffScreenIndicatorManagerCanvas).indicatorSize = Canvas_indicatorSize;
			}
			manager.indicators = indicators;
			manager.enableDebug = enableDebug;
			manager.CheckFields();
			// foreach(FixedTarget target in targets){
			// 	AddIndicator(target.target, target.indicatorID);
			// }
		}

		public void AddDroid(GameObject droid)
		{
			DroidsAlive.Add(droid);
			UpdateIndicators();
		}
		public void ResetIndicators()
		{
			ClearIndicators();
			DroidsAlive.Clear();
			Targets.Clear();
		}
		public void RemoveDroid(GameObject droid)
		{
			ClearIndicators();
			DroidsAlive.Remove(droid);
			UpdateIndicators();
		}
		public void ClearIndicators()
		{
			foreach(GameObject droid in DroidsAlive){
				RemoveIndicator(droid.transform);
			}
		}
		public void UpdateIndicators(){
			int i = 0;
			List<FixedTarget> targets = new List<FixedTarget>();
			foreach(GameObject droid in DroidsAlive){
				if(droid.name == "TrainingDroid(Clone)")
				{
					targets.Add(new FixedTarget(){target = droid.transform, indicatorID = i, indicatorColor = "Yellow"});
					i++;
				}
				else if(droid.name == "MachineGunnerDroid(Clone)")
				{
					targets.Add(new FixedTarget(){target = droid.transform, indicatorID = i, indicatorColor = "Red"});
					i++;
				}
				
			}
			Targets = targets;
			SendTargets();
		}
		public void SendTargets(){
			foreach(FixedTarget target in Targets){
				AddIndicator(target.target, target.indicatorID, target.indicatorColor);
			}
		}

		public void AddIndicator(Transform target, int indicatorID, string indicatorColor){
			manager.AddIndicator(target, indicatorID, indicatorColor);
		}

		public void RemoveIndicator(Transform target){
			manager.RemoveIndicator(target);
		}

	}

	/// <summary>
	/// Indicator.
	/// References and colors for indicator sprites
	/// </summary>
	[System.Serializable]
	public class Indicator{
		public Sprite onScreenSprite;
		public Color onScreenColor = Color.white;
		public bool onScreenRotates;
		public Sprite offScreenSprite;
		public Color offScreenColor = Color.white;
		public bool offScreenRotates;
		public Vector3 targetOffset;
		/// <summary>
		/// Both sprites need to have the same transition
		/// aswell both sprites need to have the same duration.
		/// </summary>
		public Transition transition;
		public float transitionDuration = 1;
		[System.NonSerialized]
		public bool showOnScreen;
		[System.NonSerialized]
		public bool showOffScreen;

		public enum Transition{
			None,
			Fading,
			Scaling
		}
	}

	[System.Serializable]
	public class FixedTarget{
		public Transform target;
		public int indicatorID;
		public string indicatorColor;
	}
}