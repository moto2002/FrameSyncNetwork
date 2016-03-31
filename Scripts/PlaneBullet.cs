using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlaneBullet : FrameBehaviour {

    public PlayerPlane owner;
    public Vector3 dir;
    public int group;

    protected override void OnInit()
    {
        base.OnInit();
        this.transform.SetParent(owner.transform.parent);
        this.transform.localPosition = owner.transform.localPosition;
    }

    public Rect GetRect
    {
        get
        {
            RectTransform rectTrans = transform as RectTransform;
            var localPos = rectTrans.localPosition;
            var rect = rectTrans.rect;
            rect.center = localPos;
            return rect;
        }
    }

    public override void FrameUpdate()
    {
        var localPos = (this.transform.localPosition += dir * 10);
        RectTransform rectTrans = transform as RectTransform;
        Rect myRect = GetRect;
        var planeRoot = owner.transform.parent;
        for (int i = 0, imax = planeRoot.childCount; i < imax; i++) {
            var t = planeRoot.GetChild(i);
            var plane = t.GetComponent<PlayerPlane>();
            if (plane != null && plane.group != owner.group) {
                var otherRect = plane.GetRect;
                if (otherRect.Overlaps(myRect))
                {
                    Explode(plane);
                    return;
                }
            }
        }
        if (localPos.x > 900 || localPos.x < -900) {
            IsDisposed = true;
            Destroy(gameObject);
        }
    }

    void Explode(PlayerPlane target)
    {
        target.GotDamage(2);
        this.IsDisposed = true;
        Destroy(gameObject);
    }
}
