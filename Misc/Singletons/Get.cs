using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Get
{
	public static bool Approximately(this Vector3 point, Vector3 other)
	{
		return Mathf.Approximately(point.x, other.x) && Mathf.Approximately(point.y, other.y) && Mathf.Approximately(point.z, other.z);
	}
	
	public static bool IsZero(this Vector3 point)
	{
		return Approximately(point, Vector3.zero);
	}
	
	public static void SetX(this Transform transform, float value)
	{
		var p = transform.localPosition;
		p.x = value;
		transform.localPosition = p;
	}
	
	public static void SetY(this Transform transform, float value)
	{
		var p = transform.localPosition;
		p.y = value;
		transform.localPosition = p;
	}
	
	public static void SetZ(this Transform transform, float value)
	{
		var p = transform.localPosition;
		p.z = value;
		transform.localPosition = p;
	}
	
	public static Vector3 DirectionTo(this Transform transform, Vector3 position)
	{
		return Vector3.Normalize(position - transform.position);
	}
	
	public static Vector3 DirectionTo(this Transform transform, Transform other)
	{
		return Vector3.Normalize(other.position - transform.position);
	}
	
	public static float DistanceTo(this Transform transform, Transform other)
	{
		return Vector3.Distance(other.position, transform.position);
	}
	
	public static bool isInFront(this Transform transform, Transform Testing, float angle) {
		float findAngle = Vector3.Angle(transform.forward, Testing.position-transform.position);
        
        if (Mathf.Abs(findAngle) < angle)
			return true;
		else
			return false;
	}
	
	public static bool isInFront(this Transform transform, Transform Testing) {
		float angle = 60f;
		float findAngle = Vector3.Angle(transform.forward, Testing.position-transform.position);
        
        if (Mathf.Abs(findAngle) < angle)
			return true;
		else
			return false;
	}
	
	public static Vector3 LookAwayFrom(this Transform transform, Vector3 point)
	{
	    point = 2.0f * transform.position - point;
	    return point;
	}

}
