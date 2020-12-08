using TUIO;
using UnityEngine;

namespace Assets
{
    [System.Serializable]
    public class TouchPoint
    {

        public TouchPoint(TuioCursor tuioCursor)
        {
            X = tuioCursor.getX();
            Y = tuioCursor.getY();
            Active = tuioCursor.getTuioState() != TuioContainer.TUIO_REMOVED;
            //Debug.Log(tuioCursor.getTuioState());
        }

        public float X { get; set; }
        public float Y { get; set; }
        public bool Active { get; set; } 
        public Vector3 fromTUIO()
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(X, 1 - Y, -Camera.main.transform.position.z));
        }
    }
}



