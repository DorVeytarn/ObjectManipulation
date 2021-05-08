using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    [SerializeField] private SurfaceType type;

    public SurfaceType Type => type;
}

