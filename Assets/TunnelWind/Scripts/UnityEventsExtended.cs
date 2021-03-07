﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]	 public class IntEvent : UnityEvent<int>{}
[System.Serializable]    public class FloatEvent : UnityEvent<float>{}
[System.Serializable]    public class StringEvent : UnityEvent<string>{}
[System.Serializable]    public class BoolEvent : UnityEvent<bool>{}
[System.Serializable]    public class VectorEvent : UnityEvent<Vector3>{}
[System.Serializable]    public class VectorRotationEvent : UnityEvent<Vector3, Quaternion>{}