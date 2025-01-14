﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    // Camera-specific stuff
    [SerializeField] private Transform highCameraTransform = null;
    private Camera hiCam = null;
    [SerializeField] private Transform lowCameraTransform = null;
    private Camera loCam = null;

    [SerializeField] private RectTransform outputImages = null;
    [SerializeField, Range(0.0f, 2.0f)] private float screenRatioAdjustmentTime = 0.5f;
    private float screenRatio = 0.5f;
    private float screenRatioGoal = 0.5f;

    // Stuff related to camera tracking
    [Header("Camera tracking config")]
    [SerializeField] private Transform followTarget = null;
    [SerializeField] private float trackingDeadZone = 1.0f;
    [SerializeField, Range(0.0f,1.0f)] private float trackingMargin = 0.5f;
    [SerializeField, Range(0.0f, 2.0f)] private float catchupTime = 0.5f;
    [SerializeField] private float verticalDeadZone = 5.0f;                            // how high from their baseline the player has to be to be for vertical scrolling to occur
    [SerializeField, Range(0.0f, 1.0f)] private float verticalDampening = 0.5f; // how much dampening there is on vertical position
    private bool moving = false;

    private Vector2 hiCamDisplacement;
    private Vector2 loCamDisplacement;

    // Start is called before the first frame update
    void Start()
    {
        // bail out if refs are bad
        if (!highCameraTransform || !lowCameraTransform) return;

        // grab camera refs and figure out their base displacement from the central transform
        hiCam = highCameraTransform.GetComponent<Camera>();
        loCam = lowCameraTransform.GetComponent<Camera>();

        hiCamDisplacement = highCameraTransform.position - transform.position;
        loCamDisplacement = lowCameraTransform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (followTarget)
        {
            Rigidbody rb = followTarget.GetComponent<Rigidbody>();
            MoveTowards((rb == null) ? followTarget.position : followTarget.position + (rb.velocity * catchupTime));
        }

        if(screenRatio != screenRatioGoal) AdjustScreenRatio();
    }

    // Move methods for continuity between Cameras
    private void Move(Vector2 deltaPos)
    {
        transform.Translate(deltaPos);
        lowCameraTransform.Translate(new Vector2(0, -deltaPos.y));
    }

    // Figures out if the tracked object has moved far enough, and moves to match it if needed
    private void MoveTowards(Vector2 target)
    {
        target.y = (target.y <= verticalDeadZone) ? 0 : target.y * verticalDampening;
        Vector2 dist = target - (Vector2)transform.position;

        if(moving || dist.sqrMagnitude > (trackingDeadZone * trackingDeadZone))
        {
            moving = true;
            Vector2 translation = Vector2.Lerp(Vector2.zero, dist, Time.deltaTime / catchupTime);
            Move(translation);
        }
        // if we made it within the tolerance, stop tracking
        if (dist.sqrMagnitude < Mathf.Pow((trackingMargin * trackingDeadZone), 2)) moving = false;
    }

    public float ScreenRatio
    {
        get { return screenRatio; }
        set
        {
            screenRatioGoal = Mathf.Clamp01(value);
        }
    }

    public void HardSetScreenRatio(float val)
    {
        screenRatio = Mathf.Clamp01(val);
        screenRatioGoal = screenRatio;
        AdjustScreenRatio();
    }

    private void AdjustScreenRatio()
    {
            screenRatio = Mathf.Clamp01(Mathf.Lerp(screenRatio,screenRatioGoal,Time.deltaTime / screenRatioAdjustmentTime));

            outputImages.GetChild(0).localScale = new Vector3(1,1-screenRatio,1);
            outputImages.GetChild(1).localScale = new Vector3(1, screenRatio, 1);
            outputImages.anchoredPosition = new Vector2(outputImages.anchoredPosition.x, Mathf.Lerp(-540, 540, screenRatio));
    }

    public void SetFollowTarget(Transform newTarget)
    {
        followTarget = newTarget;
    }
}