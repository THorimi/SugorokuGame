using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraCheck : MonoBehaviour
{
    // ���߂ŕ`��ɗ��p�����J����
    public Camera _currentCamera;
    private void Start()
    {
        // �J�����`��C�x���g���w�ǂ���
        RenderPipelineManager.beginCameraRendering += setCamera;
    }
    private void OnDisable()
    {
        // �J�����`��C�x���g����������
        RenderPipelineManager.beginCameraRendering -= setCamera;
    }

    void setCamera(ScriptableRenderContext context, Camera camera)
    {

        // �ŐV�̕`��p�J�����Ƃ��ēo�^����
        _currentCamera = camera;
    }

    private void Update()
    {
    }
}

