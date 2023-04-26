using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    public enum eType { center, inset, outset };

    [System.Flags]
    public enum eScreenLocs {
        onScreen = 0,
        offRight = 1,
        offLeft = 2,
        offUp = 4,
        offDown = 8
    }

    [Header("Inscribed")]
    public eType boundsType = eType.center;
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Dynamic")]
    public eScreenLocs screenLocs = eScreenLocs.onScreen;
    //public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;

    void Awake() {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    void LateUpdate () {
        float checkRadius = 0;
        if (boundsType == eType.inset) {
            checkRadius = -radius;
        }
        if (boundsType == eType.outset) {
            checkRadius = radius;
        }

        Vector3 pos = transform.position;
        screenLocs = eScreenLocs.onScreen;
        //isOnScreen = true;

        if (pos.x > camWidth + checkRadius) {
            pos.x = camWidth + checkRadius;
            //isOnScreen = false;
            screenLocs |= eScreenLocs.offRight;
        }

        if (pos.x < -camWidth - checkRadius) {
            pos.x = -camWidth - checkRadius;
            //isOnScreen = false;
            screenLocs |= eScreenLocs.offLeft;
        }

        if (pos.y > camWidth + checkRadius) {
            pos.y = camWidth + checkRadius;
            //isOnScreen = false;
            screenLocs |= eScreenLocs.offUp;
        }

        if (pos.y < -camWidth - checkRadius) {
            pos.y = -camWidth - checkRadius;
            //isOnScreen = false;
            screenLocs |= eScreenLocs.offDown;
        }

        if (keepOnScreen & !isOnScreen) {
            transform.position = pos;
            //isOnScreen = true;
            screenLocs = eScreenLocs.onScreen;
        }
        
    }

    public bool isOnScreen {
        get { return ( screenLocs ==eScreenLocs.onScreen ); }
    }

    public bool LocIs( eScreenLocs checkLoc ) {
        if ( checkLoc == eScreenLocs.onScreen ) return isOnScreen;
        return ( (screenLocs & checkLoc) == checkLoc );
    }
}
