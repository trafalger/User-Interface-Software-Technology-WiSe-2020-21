using UnityEngine;
using System.Collections;
using TUIO;
using System.Collections.Generic;
using Assets;

public partial class TouchReceiver : MonoBehaviour
{

    [SerializeField]
    Mesh debugMesh;
    [SerializeField]
    Material startMat;
    [SerializeField]
    Material currentMat;
    [SerializeField]
    GestureState currentState = GestureState.None;
    public TouchPoint FirstTouchPoint { get; set; }
    public TouchPoint SecondTouchPoint { get; set; }
    Matrix4x4 startTransform;
    Vector3 startTouchA;
    Vector3 startTouchB;
    Vector3 newTouchA;
    Vector3 newTouchB;

    
    void Start()
    {
        
        //Debug.Log(FirstTouchPoint);
        //Debug.Log(SecondTouchPoint);
    }

    void Update()
    {
        //DrawDebugMeshes();
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        //TODO
        //Simple example:
        int counter = 0;
        if (FirstTouchPoint != null && counter==0)
        {
            this.transform.position = FirstTouchPoint.fromTUIO();
            Debug.Log(FirstTouchPoint.Active);
            counter++;

        }
        if (SecondTouchPoint != null && counter==1)
        {
             switch (currentState)
            {
                case GestureState.None:
                case GestureState.Transform:
                    //when this gesture starts...
                    currentState = GestureState.Drag;
                    startTransform = transform.localToWorldMatrix;
                    Debug.Log(startTransform);
                    startTouchA = FirstTouchPoint.fromTUIO();
                    break;
                case GestureState.Drag:
                    //if this gesture is ongoing
                    currentState = GestureState.Transform;
                    newTouchA = FirstTouchPoint.fromTUIO();
                    Matrix4x4 newTrans = Matrix4x4.TRS(newTouchA - startTouchA, Quaternion.identity, Vector3.one) * startTransform;
                    Helper.SetTransformFromMatrix(transform, ref newTrans);

                    break;
            }
          //  startTouchA = FirstTouchPoint.fromTUIO();
           // startTouchB = SecondTouchPoint.fromTUIO();
            //Matrix4x4 scaleMat = Matrix4x4.TRS(startTouchA - startTouchB, Quaternion.identity, startTouchB);
            //Quaternion rot = Quaternion.FromToRotation(SAB, NAB);
            //Helper.SetTransformFromMatrix(transform, ref scaleMat);
           
           
        }
    }
    /*
        void HandleTouchInput()
    {
        int count = 0;
        if (FirstTouchPoint != null && FirstTouchPoint.Active)
        {
            count++;
        }
        if (SecondTouchPoint != null && SecondTouchPoint.Active)
        {
            count++;
        }

        if (count == 0)
        {
            currentState = GestureState.None;
        }
        if (count == 1)
        {
            // drag

            switch (currentState)
            {
                case GestureState.None:
                case GestureState.Transform:
                    //when this gesture starts...
                    currentState = GestureState.Drag;
                    startTransform = transform.localToWorldMatrix;
                    startTouchA = FirstTouchPoint.fromTUIO();
                    break;
                case GestureState.Drag:
                    //if this gesture is ongoing
                    newTouchA = FirstTouchPoint.fromTUIO();
                    Matrix4x4 newTrans = Matrix4x4.TRS(newTouchA - startTouchA, Quaternion.identity, Vector3.one) * startTransform;
                    Helper.SetTransformFromMatrix(transform, ref newTrans);

                    break;
            }
        }
        else if (count == 2)
        {
            switch (currentState)
            {
                case GestureState.None:
                case GestureState.Drag:
                    //when this gesture starts...
                    currentState = GestureState.Transform;
                    startTransform = transform.localToWorldMatrix;
                    startTouchA = FirstTouchPoint.fromTUIO();
                    startTouchB = SecondTouchPoint.fromTUIO();
                    break;
                case GestureState.Transform:
                    //if this gesture is ongoing
                    newTouchA = FirstTouchPoint.fromTUIO();
                    newTouchB = SecondTouchPoint.fromTUIO();
                    Vector3 SAB = startTouchB - startTouchA;
                    Vector3 NAB = newTouchB - newTouchA;
                    Quaternion rot = Quaternion.FromToRotation(SAB, NAB);
                    float scale = NAB.magnitude / SAB.magnitude;
                    Vector3 newScale = new Vector3(scale, scale, scale);

                    Matrix4x4 toRoot = Helper.TranslationMatrix(-startTouchA);
                    Matrix4x4 toNewPos = Helper.TranslationMatrix(newTouchA);
                    Matrix4x4 rotMat = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
                    Matrix4x4 scaleMat = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, newScale);
                    Matrix4x4 newTrans = toNewPos * scaleMat * rotMat * toRoot;

                    Graphics.DrawMesh(debugMesh, Matrix4x4.TRS(startTouchA, Quaternion.identity, new Vector3(0.05f, 0.05f, 0.05f)), startMat, 0);
                    Graphics.DrawMesh(debugMesh, Matrix4x4.TRS(startTouchB, Quaternion.identity, new Vector3(0.05f, 0.05f, 0.05f)), startMat, 0);
                    Graphics.DrawMesh(debugMesh, Matrix4x4.TRS(newTrans.MultiplyPoint(startTouchA), Quaternion.identity, new Vector3(0.05f, 0.05f, 0.05f)), currentMat, 0);
                    Graphics.DrawMesh(debugMesh, Matrix4x4.TRS(newTrans.MultiplyPoint(startTouchB), Quaternion.identity, new Vector3(0.05f, 0.05f, 0.05f)), currentMat, 0);


                    newTrans = newTrans * startTransform;
                    //Matrix4x4 newTrans = Matrix4x4.TRS(newTouchA - startTouchA, rot, newScale) * startTransform;
                    Helper.SetTransformFromMatrix(transform, ref newTrans);



                    break;
            }
            // drag rotate scale

        }

    }
*/
    /// <summary>
    /// Processes Tuio-Events
    /// </summary>
    /// <param name="events"></param>
	void processEvents(ArrayList events)
    {
        foreach (BBCursorEvent cursorEvent in events)
        {
            TuioCursor myCursor = cursorEvent.cursor;
            if (myCursor.getCursorID() == 0)
            {
                FirstTouchPoint = new TouchPoint(myCursor);
            }
            if (myCursor.getCursorID() == 1)
            {
                SecondTouchPoint = new TouchPoint(myCursor);
                Debug.Log("zweiter touchpunkt regestriert");
            }
        }
    }

    void DrawDebugMeshes()
    {
        DrawDebugMeshForTouchPoint(FirstTouchPoint);
        DrawDebugMeshForTouchPoint(SecondTouchPoint);

    }

    private void DrawDebugMeshForTouchPoint(TouchPoint touchPoint)
    {
        if (touchPoint != null && touchPoint.Active)
        {
            DrawDebugMeshFor(touchPoint.fromTUIO());
        }
    }

    private void DrawDebugMeshFor(Vector3 touchPointVector)
    {
        Graphics.DrawMesh(debugMesh,
                            Matrix4x4.TRS(touchPointVector, Quaternion.identity,
                                new Vector3(0.05f, 0.05f, 0.05f)), currentMat, 0);
    }
}
