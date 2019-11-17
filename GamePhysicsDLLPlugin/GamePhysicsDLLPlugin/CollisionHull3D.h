#pragma once
#include <math.h>
#include <vector>

#ifndef COLLISIONHULLEXPORTS
#define COLLISIONHULL_API  __declspec(dllexport)
#else
#define COLLISIONHULL_API  __declspec(dllimport)
#endif

struct PriceVector3
{
	PriceVector3()
	{
		x = 0;
		y = 0;
		z = 0;
	}

	PriceVector3(float newX, float newY, float newZ)
	{
		x = newX;
		y = newY;
		z = newZ;
	}
	float x, y, z;

	float magnitude() {return sqrt(x * x + y * y + z * z); }

	PriceVector3 InverseTransformPoint(PriceVector3 transform)
	{
		PriceVector3 newVec3(0, 0, 0);

		newVec3 = transform / (transform.Dot(transform));

		return newVec3;
	}

	float Dot(PriceVector3 other)
	{
		float result = x * other.x + y * other.y + z * other.z;

		return result;
	}

	float sqrMagnitude()
	{
		return (x * x + y * y + z * z);
	}
};

PriceVector3 operator+(PriceVector3 left, PriceVector3 right)
{
	PriceVector3 newVec3(0,0,0);

	newVec3.x = left.x + right.x;
	newVec3.y = left.y + right.y;
	newVec3.z = left.z + right.z;

	return newVec3;
}

PriceVector3 operator-(PriceVector3 left, PriceVector3 right)
{
	PriceVector3 newVec3(0,0,0);

	newVec3.x = left.x - right.x;
	newVec3.y = left.y - right.y;
	newVec3.z = left.z - right.z;

	return newVec3;
} 

PriceVector3 operator/(PriceVector3 left, float right)
{
	PriceVector3 newVec3(0,0,0);

	newVec3.x = left.x / right;
	newVec3.y = left.y / right;
	newVec3.z = left.z / right;

	return newVec3;
} 


struct Contact 
{
	PriceVector3 point;
	PriceVector3 normal;
	float restitution;
	float penetration;
};
class Collision 
{
public:
	//collisionHull3D a, b;
	Contact contacts[4];
	int contactCount;
	bool status;
	PriceVector3 closingVelocity;
	Collision();
};
enum hull_type
{
	hull_spehere,
	hull_aabb,
	hull_obb
};

class CollisionHull3D
{
public:
	hull_type type;
	//For collision response vvv
	//particle3D
	Collision c;
	bool colliding;
	float restitution;

	virtual bool TestCollisionVSSphere(SphereCollisionHull3D other, Collision& newc);
	virtual bool TestCollisionVSAABB(AABBCollisionHull3D other, Collision& newc);
	virtual bool TestCollisionVSOBB(OBBCollisionHull3D other, Collision& newc);
};


	bool SeparateAxis(PriceVector3 T, PriceVector3 comparedAxis, float lwh1, float Rij1, float lwh2, float Rij2, float lwh3, float Rij3, float lwhBySelf)
	{
		return T.Dot(comparedAxis) > abs(lwh1 * Rij1) + abs(lwh2 * Rij2) + abs(lwh2 * Rij2) + lwhBySelf;
	}

	// T = PB-PA, compared axis (think Ax, Bx)
	bool SeparateAxisCrossAB(PriceVector3 T, PriceVector3 comparedAxis1, float Rij1, PriceVector3 comparedAxis2, float Rij2,
		float lwh1, float lwhRi1, float lwh2, float lwhRi2, float lwh3, float lwhRi3, float lwh4, float lwhRi4)
	{
		/*
		Let A and B be oriented bounding boxes (OBB).
		Parameters
		PA = coordinate position of the center of A
		Ax = unit vector representing the x-axis of A
		Ay = unit vector representing the y-axis of A
		Az = unit vector representing the z-axis of A
		WA = half width of A (corresponds with the local x-axis of A)
		HA = half height of A (corresponds with the local y-axis of A)
		DA = half depth of A (corresponds with the local z-axis of A)
		PB = coordinate position of the center of B
		Bx = unit vector representing the x-axis of B
		By = unit vector representing the y-axis of B
		Bz = unit vector representing the z-axis of B
		W = half width of B (corresponds with the local x-axis of B)
		H = half height of B (corresponds with the local y-axis of B)
		D = half depth of B (corresponds with the local z-axis of B)
		*/

		/*
			Variables
			T = PB – PA
			Rij = Ai •Bj
		 */

		return (abs(T.Dot(comparedAxis1) * Rij1 - T.Dot(comparedAxis2) * Rij2) >
			abs(lwh1 * lwhRi1) + abs(lwh2 * lwhRi2) + abs(lwh3 * lwhRi3) + abs(lwh4 * lwhRi4));
	}

class SphereCollisionHull3D : CollisionHull3D
{
public:
	SphereCollisionHull3D()
	{
		type = hull_spehere;
		radius = 0;
		centerOfSphere = PriceVector3(0,0,0);
	}

	SphereCollisionHull3D(PriceVector3 newPos, float newRadius)
	{
		type = hull_spehere;
		radius = newRadius;
		centerOfSphere = newPos;
	}

	PriceVector3 centerOfSphere;
	float radius;

	bool TestCollisionVSSphere(SphereCollisionHull3D other, Collision& newc)
	{
        PriceVector3 dist = other.centerOfSphere - centerOfSphere;
        if (dist.magnitude <= radius + other.radius)
        {
            return true;

        }
        else
        {
            return false;

        }
	}

	bool TestCollisionVSAABB(AABBCollisionHull3D other, Collision& newc)
	{
		PriceVector3 center = centerOfSphere;
		PriceVector3 relCenter = other.rectCenter.InverseTransformPoint(center);

		/*
		Vector3 closestPt(0,0,0);
		real dist;
		// Clamp each coordinate to the box.
		dist = relCenter.x;
		if (dist > box.halfSize.x)
			dist = box.halfSize.x;
		if (dist < -box.halfSize.x)
			dist = -box.halfSize.x;

		closestPt.x = dist;
		*/
		PriceVector3 closestPoint(0, 0, 0);// = new PriceVector3(0, 0, 0);
		float dist;

		dist = relCenter.x;

		if (dist > other.length * 0.5f)
		{
			dist = other.length * 0.5f;
		}
		if (dist < -other.length * 0.5f)
		{
			dist = -other.length * 0.5f;
		}

		closestPoint.x = dist;

		/*
		dist = relCenter.y;

		if (dist > box.halfSize.y)
			dist = box.halfSize.y;
		if (dist < -box.halfSize.y)
			dist = -box.halfSize.y;

		closestPt.y = dist;

		*/
		dist = relCenter.y;

		if (dist > other.height * 0.5f)
		{
			dist = other.height * 0.5f;
		}
		if (dist < -other.height * 0.5f)
		{
			dist = -other.height * 0.5f;
		}

		closestPoint.y = dist;
		/*
		dist = relCenter.z;

		if (dist > box.halfSize.z)
			dist = box.halfSize.z;
		if (dist < -box.halfSize.z)
			dist = -box.halfSize.z;

		closestPt.z = dist;
		*/
		dist = relCenter.z;

		if (dist > other.width * 0.5f)
		{
			dist = other.width * 0.5f;
		}
		if (dist < -other.width * 0.5f)
		{
			dist = -other.width * 0.5f;
		}

		closestPoint.z = dist;
		/*
		// Check to see if we’re in contact.
		dist = (closestPt - relCenter).squareMagnitude();

		if (dist > sphere.radius * sphere.radius)
			return 0;
		else
			return 1;
		 */



		dist = (closestPoint - relCenter).sqrMagnitude;

		if (dist > radius* radius)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
	
	bool TestCollisionVSOBB(OBBCollisionHull3D other, Collision& newc)
	{
		PriceVector3 center = centerOfSphere;
		PriceVector3 relCenter = other.rectCenter.InverseTransformPoint(center);

		/*
		Vector3 closestPt(0,0,0);
		real dist;
		// Clamp each coordinate to the box.
		dist = relCenter.x;
		if (dist > box.halfSize.x)
			dist = box.halfSize.x;
		if (dist < -box.halfSize.x)
			dist = -box.halfSize.x;

		closestPt.x = dist;
		*/
		PriceVector3 closestPoint(0, 0, 0);
		float dist;

		dist = relCenter.x;

		if (dist > other.length * 0.5f)
		{
			dist = other.length * 0.5f;
		}
		if (dist < -other.length * 0.5f)
		{
			dist = -other.length * 0.5f;
		}

		closestPoint.x = dist;

		/*
		dist = relCenter.y;

		if (dist > box.halfSize.y)
			dist = box.halfSize.y;
		if (dist < -box.halfSize.y)
			dist = -box.halfSize.y;

		closestPt.y = dist;

		*/
		dist = relCenter.y;

		if (dist > other.height * 0.5f)
		{
			dist = other.height * 0.5f;
		}
		if (dist < -other.height * 0.5f)
		{
			dist = -other.height * 0.5f;
		}

		closestPoint.y = dist;
		/*
		dist = relCenter.z;

		if (dist > box.halfSize.z)
			dist = box.halfSize.z;
		if (dist < -box.halfSize.z)
			dist = -box.halfSize.z;

		closestPt.z = dist;
		*/
		dist = relCenter.z;

		if (dist > other.width * 0.5f)
		{
			dist = other.width * 0.5f;
		}
		if (dist < -other.width * 0.5f)
		{
			dist = -other.width * 0.5f;
		}

		closestPoint.z = dist;
		/*
		// Check to see if we’re in contact.
		dist = (closestPt - relCenter).squareMagnitude();

		if (dist > sphere.radius * sphere.radius)
			return 0;
		else
			return 1;
		 */



		dist = (closestPoint - relCenter).sqrMagnitude;

		if (dist > radius* radius)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
};

class AABBCollisionHull3D : public CollisionHull3D
{
public:
	float length, width, height;
	PriceVector3 rectCenter;
	AABBCollisionHull3D()
	{
		type = hull_aabb;
		length = 0;
		height = 0;
		width = 0;
		rectCenter.x = 0;
		rectCenter.y = 0;
		rectCenter.z = 0;
	}

	AABBCollisionHull3D(float newLength, float newHeight, float newWidth, PriceVector3 newPos)
	{
		type = hull_aabb;
		length = newLength;
		height = newHeight;
		width = newWidth;
		rectCenter.x = newPos.x;
		rectCenter.y = newPos.y;
		rectCenter.z = newPos.z;
	}

	bool TestCollisionVSSphere(SphereCollisionHull3D other, Collision& newc)
	{
		return other.TestCollisionVSAABB(*this, newc);
	}

	bool TestCollisionVSAABB(AABBCollisionHull3D other, Collision& newc)
	{
		// for each dimension, max extent of A greater than min extent of B

		// rectCenter - 1/2(length or width) for min point in x or y
		float thisXMin = rectCenter.x - width * 0.5f;

		// rectCenter + 1/2(length or width) for max point in x or y
		float thisXMax = rectCenter.x + width * 0.5f;

		// rectCenter - 1/2(length or width) for min point in x or y
		float thisYMin = rectCenter.y - height * 0.5f;

		// rectCenter + 1/2(length or width) for max point in x or y
		float thisYMax = rectCenter.y + height * 0.5f;

		float thisZMax = rectCenter.z + width * 0.5f;

		float thisZMin = rectCenter.z - width * 0.5f;

		// rectCenter - 1/2(length or width) for min point in x or y
		float otherXMin = other.rectCenter.x - width * 0.5f;

		// rectCenter + 1/2(length or width) for max point in x or y
		float otherXMax = other.rectCenter.x + width * 0.5f;

		// rectCenter - 1/2(length or width) for min point in x or y
		float otherYMin = other.rectCenter.y - height * 0.5f;

		// rectCenter + 1/2(length or width) for max point in x or y
		float otherYMax = other.rectCenter.y + height * 0.5f;

		float otherZMax = other.rectCenter.z + width * 0.5f;

		float otherZMin = other.rectCenter.z - width * 0.5f;

		// check which min is greater, greater min becomes the one, other becomes other
		// if one max.x < other max.x && one max.x > other min.x
		if (otherXMin <= thisXMax && thisXMax <= otherXMax)
		{
			// if this passes, check same thing with y
			if (otherYMin <= thisYMax && thisYMax <= otherYMax)
			{
				// if this passes, check same thing with z
				if (otherZMin <= thisZMax && thisZMax <= otherZMax)
				{
					return true;
				}
				else if (otherZMin <= thisZMin && thisZMin <= otherZMax)
				{
					return true;
				}
			}
			else if (otherYMin <= thisYMin && thisYMin <= otherYMax)
			{
				// if this passes, check same thing with z
				if (otherZMin <= thisZMax && thisZMax <= otherZMax)
				{
					return true;
				}
				else if (otherZMin <= thisZMin && thisZMin <= otherZMax)
				{
					return true;
				}
			}
		}

		// check which min is greater, greater min becomes the one, other becomes other
		// if one min.x < other max.x && one min.x > other min.x
		if (otherXMin <= thisXMin && thisXMin <= otherXMax)
		{
			// if this passes, check same thing with y
			if (otherYMin <= thisYMax && thisYMax <= otherYMax)
			{
				// if this passes, check same thing with z
				if (otherZMin <= thisZMax && thisZMax <= otherZMax)
				{
					return true;
				}
				else if (otherZMin <= thisZMin && thisZMin <= otherZMax)
				{
					return true;
				}
			}
			else if (otherYMin <= thisYMin && thisYMin <= otherYMax)
			{
				// if this passes, check same thing with z
				if (otherZMin <= thisZMax && thisZMax <= otherZMax)
				{
					return true;
				}
				else if (otherZMin <= thisZMin && thisZMin <= otherZMax)
				{
					return true;
				}
			}
		}


		return false;
	}

	bool TestCollisionVSOBB(OBBCollisionHull3D other, Collision& newc)
	{
		
		// positions of box a and box b
		PriceVector3 PA = rectCenter;
		PriceVector3 PB = other.rectCenter;

		PriceVector3 transformRightA = rectCenter + PriceVector3(1, 0, 0);
		PriceVector3 transformUpA = rectCenter + PriceVector3(0, 1, 0);
		PriceVector3 transformForwardA = rectCenter + PriceVector3(0, 0, 1);

		// box A's axis
		PriceVector3 Ax = transformRightA;
		PriceVector3 Ay = transformUpA;
		PriceVector3 Az = transformForwardA;

		// length width height of box a
		float WA = length;
		float DA = width;
		float HA = height;

		PriceVector3 transformRightB = other.rectCenter + PriceVector3(1, 0, 0);
		PriceVector3 transformUpB = other.rectCenter + PriceVector3(0, 1, 0);
		PriceVector3 transformForwardB = other.rectCenter + PriceVector3(0, 0, 1);

		// box b's axis
		PriceVector3 Bx = transformRightB;
		PriceVector3 By = transformUpB;
		PriceVector3 Bz = transformForwardB;

		// length width height of box b
		float WB = other.length;
		float DB = other.width;
		float HB = other.height;

		PriceVector3 T = PB - PA;

		float Rxx = Ax.Dot(Bx);
		float Rxy = Ax.Dot(By);
		float Rxz = Ax.Dot(Bz);
					
		float Ryx = Ay.Dot(Bx);
		float Ryy = Ay.Dot(By);
		float Ryz = Ay.Dot(Bz);
					
		float Rzx = Az.Dot(Bx);
		float Rzy = Az.Dot(By);
		float Rzz = Az.Dot(Bz);

		// case 1,2,3
		if (SeparateAxis(T, Ax, WB, Rxx, HB, Rxy, DB, Rxz, WA))
		{
			//Debug.Log("failed case 1");
			return false;
		}
		if (SeparateAxis(T, Ay, WB, Ryx, HB, Ryy, DB, Ryz, HA))
		{
			//Debug.Log("failed case 2");
			return false;
		}
		if (SeparateAxis(T, Az, WB, Rzx, HB, Rzy, DB, Rzz, DA))
		{
			//Debug.Log("failed case 3");
			return false;
		}

		// case 4,5,6
		if (SeparateAxis(T, Bx, WA, Rxx, HA, Ryx, DA, Rzx, WB))
		{
			//Debug.Log("failed case 4");
			return false;
		}
		if (SeparateAxis(T, By, WA, Rxy, HA, Ryy, DA, Rzy, HB))
		{
			//Debug.Log("failed case 5");
			return false;
		}
		if (SeparateAxis(T, Bz, WA, Rxz, HA, Ryz, DA, Rzz, DB))
		{
			//Debug.Log("failed case 6");
			return false;
		}
		return false;
	}
};

class OBBCollisionHull3D : public CollisionHull3D
{
public:
	float length, width, height;
	PriceVector3 rectCenter;

	OBBCollisionHull3D()
	{
		type = hull_obb;
		length = 0;
		height = 0;
		width = 0;
		rectCenter.x = 0;
		rectCenter.y = 0;
		rectCenter.z = 0;
	}

	OBBCollisionHull3D(float newLength, float newHeight, float newWidth, PriceVector3 newVec3)
	{
		type = hull_obb;
		length = newLength;
		width = newWidth;
		height = newHeight;

		rectCenter.x = newVec3.x;
		rectCenter.y = newVec3.y;
		rectCenter.z = newVec3.z;
	}


	bool TestCollisionVSSphere(SphereCollisionHull3D other, Collision & newc)
	{
		PriceVector3 center = other.centerOfSphere;
		PriceVector3 relCenter = rectCenter.InverseTransformPoint(center);

		/*
		Vector3 closestPt(0,0,0);
		real dist;
		// Clamp each coordinate to the box.
		dist = relCenter.x;
		if (dist > box.halfSize.x)
			dist = box.halfSize.x;
		if (dist < -box.halfSize.x)
			dist = -box.halfSize.x;

		closestPt.x = dist;
		*/
		PriceVector3 closestPoint(0, 0, 0);
		float dist;

		dist = relCenter.x;

		if (dist > length * 0.5f)
		{
			dist = length * 0.5f;
		}
		if (dist < -length * 0.5f)
		{
			dist = -length * 0.5f;
		}

		closestPoint.x = dist;

		/*
		dist = relCenter.y;

		if (dist > box.halfSize.y)
			dist = box.halfSize.y;
		if (dist < -box.halfSize.y)
			dist = -box.halfSize.y;

		closestPt.y = dist;

		*/
		dist = relCenter.y;

		if (dist > height * 0.5f)
		{
			dist = height * 0.5f;
		}
		if (dist < -height * 0.5f)
		{
			dist = -height * 0.5f;
		}

		closestPoint.y = dist;
		/*
		dist = relCenter.z;

		if (dist > box.halfSize.z)
			dist = box.halfSize.z;
		if (dist < -box.halfSize.z)
			dist = -box.halfSize.z;

		closestPt.z = dist;
		*/
		dist = relCenter.z;

		if (dist > width * 0.5f)
		{
			dist = width * 0.5f;
		}
		if (dist < -width * 0.5f)
		{
			dist = -width * 0.5f;
		}

		closestPoint.z = dist;
		/*
		// Check to see if we’re in contact.
		dist = (closestPt - relCenter).squareMagnitude();

		if (dist > sphere.radius * sphere.radius)
			return 0;
		else
			return 1;
		 */



		dist = (closestPoint - relCenter).sqrMagnitude;

		if (dist > other.radius* other.radius)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	bool TestCollisionVSAABB(AABBCollisionHull3D other, Collision& newc)
	{
		// positions of box a and box b
		PriceVector3 PA = rectCenter;
		PriceVector3 PB = other.rectCenter;

		PriceVector3 transformRightA = rectCenter + PriceVector3(1, 0, 0);
		PriceVector3 transformUpA = rectCenter + PriceVector3(0, 1, 0);
		PriceVector3 transformForwardA = rectCenter + PriceVector3(0, 0, 1);

		// box A's axis
		PriceVector3 Ax = transformRightA;
		PriceVector3 Ay = transformUpA;
		PriceVector3 Az = transformForwardA;

		// length width height of box a
		float WA = length;
		float DA = width;
		float HA = height;

		PriceVector3 transformRightB = other.rectCenter + PriceVector3(1, 0, 0);
		PriceVector3 transformUpB = other.rectCenter + PriceVector3(0, 1, 0);
		PriceVector3 transformForwardB = other.rectCenter + PriceVector3(0, 0, 1);

		// box b's axis
		PriceVector3 Bx = transformRightB;
		PriceVector3 By = transformUpB;
		PriceVector3 Bz = transformForwardB;

		// length width height of box b
		float WB = other.length;
		float DB = other.width;
		float HB = other.height;

		PriceVector3 T = PB - PA;

		float Rxx = Ax.Dot(Bx);
		float Rxy = Ax.Dot(By);
		float Rxz = Ax.Dot(Bz);
					
		float Ryx = Ay.Dot(Bx);
		float Ryy = Ay.Dot(By);
		float Ryz = Ay.Dot(Bz);
					
		float Rzx = Az.Dot(Bx);
		float Rzy = Az.Dot(By);
		float Rzz = Az.Dot(Bz);

		// case 1,2,3
		if (SeparateAxis(T, Ax, WB, Rxx, HB, Rxy, DB, Rxz, WA))
		{
			//Debug.Log("failed case 1");
			return false;
		}
		if (SeparateAxis(T, Ay, WB, Ryx, HB, Ryy, DB, Ryz, HA))
		{
			//Debug.Log("failed case 2");
			return false;
		}
		if (SeparateAxis(T, Az, WB, Rzx, HB, Rzy, DB, Rzz, DA))
		{
			//Debug.Log("failed case 3");
			return false;
		}

		// case 4,5,6
		if (SeparateAxis(T, Bx, WA, Rxx, HA, Ryx, DA, Rzx, WB))
		{
			//Debug.Log("failed case 4");
			return false;
		}
		if (SeparateAxis(T, By, WA, Rxy, HA, Ryy, DA, Rzy, HB))
		{
			//Debug.Log("failed case 5");
			return false;
		}
		if (SeparateAxis(T, Bz, WA, Rxz, HA, Ryz, DA, Rzz, DB))
		{
			//Debug.Log("failed case 6");
			return false;
		}
		return true;
	}

	bool TestCollisionVSOBB(OBBCollisionHull3D other, Collision& newc)
	{

		//Debug.Log("testing obb");

		// https://www.jkh.me/files/tutorials/Separating%20Axis%20Theorem%20for%20Oriented%20Bounding%20Boxes.pdf
		/*
			Let A and B be oriented bounding boxes (OBB).
			Parameters
			PA = coordinate position of the center of A
			Ax = unit vector representing the x-axis of A
			Ay = unit vector representing the y-axis of A
			Az = unit vector representing the z-axis of A
			WA = half width of A (corresponds with the local x-axis of A)
			HA = half height of A (corresponds with the local y-axis of A)
			DA = half depth of A (corresponds with the local z-axis of A)

			PB = coordinate position of the center of B
			Bx = unit vector representing the x-axis of B
			By = unit vector representing the y-axis of B
			Bz = unit vector representing the z-axis of B
			LB = half width of B (corresponds with the local x-axis of B)
			HB = half height of B (corresponds with the local y-axis of B)
			WB = half depth of B (corresponds with the local z-axis of B)
		*/

		/*
			Variables
			T = PB – PA
			Rij = Ai •Bj
		*/

		// positions of box a and box b
		PriceVector3 PA = rectCenter;
		PriceVector3 PB = other.rectCenter;

		PriceVector3 transformRightA = rectCenter + PriceVector3(1, 0, 0);
		PriceVector3 transformUpA = rectCenter + PriceVector3(0, 1, 0);
		PriceVector3 transformForwardA = rectCenter + PriceVector3(0, 0, 1);

		// box A's axis
		PriceVector3 Ax = transformRightA;
		PriceVector3 Ay = transformUpA;
		PriceVector3 Az = transformForwardA;

		// length width height of box a
		float WA = length * 0.5f;
		float DA = width * 0.5f;
		float HA = height * 0.5f;

		PriceVector3 transformRightB = other.rectCenter + PriceVector3(1, 0, 0);
		PriceVector3 transformUpB = other.rectCenter + PriceVector3(0, 1, 0);
		PriceVector3 transformForwardB = other.rectCenter + PriceVector3(0, 0, 1);

		// box b's axis
		PriceVector3 Bx = transformRightB;
		PriceVector3 By = transformUpB;
		PriceVector3 Bz = transformForwardB;

		// length width height of box b
		float WB = other.length * 0.5f;
		float DB = other.width * 0.5f;
		float HB = other.height * 0.5f;

		PriceVector3 T = PB - PA;

		float Rxx = Ax.Dot(Bx);
		float Rxy = Ax.Dot(By);
		float Rxz = Ax.Dot(Bz);
					
		float Ryx = Ay.Dot(Bx);
		float Ryy = Ay.Dot(By);
		float Ryz = Ay.Dot(Bz);
					
		float Rzx = Az.Dot(Bx);
		float Rzy = Az.Dot(By);
		float Rzz = Az.Dot(Bz);

		// case 1,2,3
		if (SeparateAxis(T, Ax, WB, Rxx, HB, Rxy, DB, Rxz, WA))
		{
			//Debug.Log("failed case 1");
			return false;
		}
		if (SeparateAxis(T, Ay, WB, Ryx, HB, Ryy, DB, Ryz, HA))
		{
			//Debug.Log("failed case 2");
			return false;
		}
		if (SeparateAxis(T, Az, WB, Rzx, HB, Rzy, DB, Rzz, DA))
		{
			//Debug.Log("failed case 3");
			return false;
		}

		// case 4,5,6
		if (SeparateAxis(T, Bx, WA, Rxx, HA, Ryx, DA, Rzx, WB))
		{
			//Debug.Log("failed case 4");
			return false;
		}
		if (SeparateAxis(T, By, WA, Rxy, HA, Ryy, DA, Rzy, HB))
		{
			//Debug.Log("failed case 5");
			return false;
		}
		if (SeparateAxis(T, Bz, WA, Rxz, HA, Ryz, DA, Rzz, DB))
		{
			//Debug.Log("failed case 6");
			return false;
		}

		// case 7,8,9
		if (SeparateAxisCrossAB(T, Az, Ryx, Ay, Rzx, HA, Rzx, DA, Ryx, HB, Rxz, DB, Rxy))
		{
			//Debug.Log("failed case 7");
			return false;
		}
		if (SeparateAxisCrossAB(T, Az, Ryy, Ay, Rzy, HA, Rzy, DA, Ryy, WB, Rxz, DB, Rxx))
		{
			//Debug.Log("failed case 8");
			return false;
		}
		if (SeparateAxisCrossAB(T, Az, Ryz, Ay, Rzz, HA, Rzz, DA, Ryz, WB, Rxy, HB, Rxx))
		{
			//Debug.Log("failed case 9");
			return false;
		}

		// case 10,11,12
		if (SeparateAxisCrossAB(T, Ax, Rzx, Az, Rxx, WA, Rzx, DA, Rxx, HB, Ryz, DB, Ryy))
		{
			//Debug.Log("failed case 10");
			return false;
		}
		if (SeparateAxisCrossAB(T, Ax, Rzy, Az, Rxy, WA, Rzy, DA, Rxy, WB, Ryz, DB, Ryx))
		{
			//Debug.Log("failed case 11");
			return false;
		}
		if (SeparateAxisCrossAB(T, Ax, Rzz, Az, Rxz, WA, Rzz, DA, Rxz, WB, Ryy, HB, Ryx))
		{
			//Debug.Log("failed case 12");
			return false;
		}

		// case 13,14,15
		if (SeparateAxisCrossAB(T, Ay, Rxx, Ax, Ryx, WA, Ryx, HA, Rxx, HB, Rzz, DB, Rzy))
		{
			//Debug.Log("failed case 13");
			return false;
		}
		if (SeparateAxisCrossAB(T, Ay, Rxy, Ax, Ryy, WA, Ryy, HA, Rxy, WB, Rzz, DB, Rzx))
		{
			//Debug.Log("failed case 14");
			return false;
		}
		if (SeparateAxisCrossAB(T, Ay, Rxz, Ax, Ryz, WA, Ryz, HA, Rxz, WB, Rzy, HB, Rzx))
		{
			//Debug.Log("failed case 15");
			return false;
		}
		return true;
	}
};