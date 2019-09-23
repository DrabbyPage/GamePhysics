using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull2D : MonoBehaviour
{

    public enum HULLTYPE
    {
        hull_circle,
        hull_aabb,
        hull_obb,
    }
    private HULLTYPE type { get; }

    protected CollisionHull2D(HULLTYPE type_set)
    {
        type = type_set;
    }

    protected Particle2D particle;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b)
    {
        return false;
    }

    public abstract bool TestCollisionVSCircle(CircleCollisionHull2D other);
    public abstract bool TestCollisionVSAABB(AxisAlignBoundingBoxHull2D other);
    public abstract bool TestCollisionVSOBB(ObjectBoundingBoxHull2D other);

}
