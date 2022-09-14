using System;
using System.Collections;
using System.Collections.Generic;
using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.Input.Legacy;
using UnityEngine;

public class OcclusionSample : MonoBehaviour
{
    [SerializeField] GameObject characher;
    [SerializeField] Camera maincam;

    private IARSession _session;


    // Start is called before the first frame update
    void Start()
    {
        ARSessionFactory.SessionInitialized += OnAnyARSessionDidInitialize;
    }

    private void OnAnyARSessionDidInitialize(AnyARSessionInitializedArgs args)
    {
        ARSessionFactory.SessionInitialized -= OnAnyARSessionDidInitialize;
        _session = args.Session;
    }

    // Update is called once per frame
    void Update()
    {
        if (_session == null)
        {
            return;
        }

        if (PlatformAgnosticInput.touchCount <= 0)
        {
            return;
        }

        var touch = PlatformAgnosticInput.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            TouchBegan(touch);
        }
    }

    private void TouchBegan(Touch touch)
    {
        var currentFrame = _session.CurrentFrame;
        if (currentFrame == null)
        {
            return;
        }

        var results = currentFrame.HitTest
        (
          maincam.pixelWidth,
          maincam.pixelHeight,
          touch.position,
          ARHitTestResultType.ExistingPlane
        );

        int count = results.Count;
        if (count <= 0)
            return;

        // Get the closest result
        var result = results[0];

        var hitPosition = result.WorldTransform.ToPosition();

        characher.transform.position = hitPosition;
        characher.transform.LookAt(new Vector3( currentFrame.Camera.Transform[0, 3],
                                                characher.transform.position.y,
                                                currentFrame.Camera.Transform[2, 3]));


    }
}
