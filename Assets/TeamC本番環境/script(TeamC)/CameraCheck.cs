using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraCheck : MonoBehaviour
{
    // 直近で描画に利用したカメラ
    public Camera _currentCamera;
    private void Start()
    {
        // カメラ描画イベントを購読する
        RenderPipelineManager.beginCameraRendering += setCamera;
    }
    private void OnDisable()
    {
        // カメラ描画イベントを解除する
        RenderPipelineManager.beginCameraRendering -= setCamera;
    }

    void setCamera(ScriptableRenderContext context, Camera camera)
    {

        // 最新の描画用カメラとして登録する
        _currentCamera = camera;
    }

    private void Update()
    {
    }
}

