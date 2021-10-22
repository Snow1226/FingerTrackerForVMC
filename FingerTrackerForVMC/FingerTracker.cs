using UnityEngine;
using VMCMod;
using System;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;

namespace FingerTrackerForVMC
{
    [VMCPlugin(
    Name: "FingerTracker",
    Version: "0.0.1",
    Author: "すのー",
    Description: "指トラッキングをシリアルポート経由で受信するMod",
    AuthorURL: "https://twitter.com/snow_mil",
    PluginURL: "https://github.com/Snow1226")]
    public class FingerTracker : MonoBehaviour
    {
        private List<Transform> _fingerTransforms;

        private SerialHandler _comLeft = null;
        private SerialHandler _comRight = null;
        private void Awake()
        {
            _fingerTransforms = new List<Transform>();
            VMCEvents.OnModelLoaded += OnModelLoaded;
            VMCEvents.OnCameraChanged += OnCameraChanged;
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (_comLeft.RecevedMessage.Length >= 2)
            {
                var data = _comLeft.RecevedMessage;
                if (data[0] == "L_Finger")
                {
                    for (int i = 0; i < 3; i++)
                    {
                        _fingerTransforms[i].localEulerAngles = new Vector3(_fingerTransforms[i].localEulerAngles.x, 5 * i - float.Parse(data[1]) / 100 * 30 * i, _fingerTransforms[i].localEulerAngles.z);
                        _fingerTransforms[3 + i].localEulerAngles = new Vector3(_fingerTransforms[3 + i].localEulerAngles.x, _fingerTransforms[3 + i].localEulerAngles.y, float.Parse(data[2]) / 100 * 90);
                        _fingerTransforms[6 + i].localEulerAngles = new Vector3(_fingerTransforms[6 + i].localEulerAngles.x, _fingerTransforms[6 + i].localEulerAngles.y, float.Parse(data[3]) / 100 * 90);
                        _fingerTransforms[9 + i].localEulerAngles = new Vector3(_fingerTransforms[9 + i].localEulerAngles.x, _fingerTransforms[9 + i].localEulerAngles.y, float.Parse(data[4]) / 100 * 90);
                        _fingerTransforms[12 + i].localEulerAngles = new Vector3(_fingerTransforms[12 + i].localEulerAngles.x, _fingerTransforms[12 + i].localEulerAngles.y, float.Parse(data[5]) / 100 * 90);
                    }
                }
            }
            if (_comRight.RecevedMessage.Length >= 2)
            {
                var data = _comRight.RecevedMessage;
                if (data[0] == "R_Finger")
                {
                    for (int i = 0; i < 3; i++)
                    {
                        _fingerTransforms[15 + i].localEulerAngles = new Vector3(_fingerTransforms[15 + i].localEulerAngles.x, - 5 * i + float.Parse(data[1]) / 100 * 30 * i, _fingerTransforms[15 + i].localEulerAngles.z);
                        _fingerTransforms[18 + i].localEulerAngles = new Vector3(_fingerTransforms[18 + i].localEulerAngles.x, _fingerTransforms[18 + i].localEulerAngles.y, - float.Parse(data[2]) / 100 * 90);
                        _fingerTransforms[21 + i].localEulerAngles = new Vector3(_fingerTransforms[21 + i].localEulerAngles.x, _fingerTransforms[21 + i].localEulerAngles.y, - float.Parse(data[3]) / 100 * 90);
                        _fingerTransforms[24 + i].localEulerAngles = new Vector3(_fingerTransforms[24 + i].localEulerAngles.x, _fingerTransforms[24 + i].localEulerAngles.y, - float.Parse(data[4]) / 100 * 90);
                        _fingerTransforms[27 + i].localEulerAngles = new Vector3(_fingerTransforms[27 + i].localEulerAngles.x, _fingerTransforms[27 + i].localEulerAngles.y, - float.Parse(data[5]) / 100 * 90);
                    }
                }
            }
        }

        [OnSetting]
        public void OnSetting()
        {

        }

        private void OnDestroy()
        {
            if (_comLeft)
            {
                _comLeft.Close();
                Destroy(_comLeft.gameObject);
            }
            if (_comRight)
            {
                _comRight.Close();
                Destroy(_comRight.gameObject);
            }
        }

        private void OnModelLoaded(GameObject currentModel)
        {
            if (currentModel == null) return;

            var animator = currentModel.GetComponent<Animator>();
            _fingerTransforms.Clear();
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.LeftThumbProximal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.LeftThumbIntermediate));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.LeftThumbDistal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.LeftIndexProximal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.LeftIndexIntermediate));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.LeftIndexDistal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.LeftMiddleProximal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.LeftMiddleIntermediate));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.LeftMiddleDistal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.LeftRingProximal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.LeftRingIntermediate));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.LeftRingDistal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.LeftLittleProximal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.LeftLittleIntermediate));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.LeftLittleDistal));

            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.RightThumbProximal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.RightThumbIntermediate));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.RightThumbDistal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.RightIndexProximal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.RightIndexIntermediate));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.RightIndexDistal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.RightMiddleDistal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.RightRingProximal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.RightRingIntermediate));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.RightRingDistal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.RightLittleProximal));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.RightLittleIntermediate));
            _fingerTransforms.Add(animator.GetBoneTransform(HumanBodyBones.RightLittleDistal));

            if (_comLeft==null)
            {
                _comLeft = new GameObject("ComPortLeft").AddComponent<SerialHandler>();
                _comLeft.portName = "COM4";
                _comLeft.Open();
            }
            if (_comRight==null)
            {
                _comRight = new GameObject("ComPortRight").AddComponent<SerialHandler>();
                _comRight.portName = "COM6";
                _comRight.Open();
            }
        }

        private void OnCameraChanged(Camera currentCamera)
        {
            //カメラ切り替え時に現在のカメラを取得できます
        }
    }
}
