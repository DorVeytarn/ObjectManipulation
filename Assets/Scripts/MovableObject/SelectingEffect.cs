using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class SelectingEffect : MonoBehaviour
{
    [SerializeField] private MovableObject movable;
    [SerializeField] private Outline outline;

    private ObjectSelector objectSelector;

    private void Start()
    {
        objectSelector = S.DI<ObjectSelector>();

        objectSelector.ObjectSelected += ChangeEffectVisibility;
    }

    private void OnDestroy()
    {
        objectSelector.ObjectSelected -= ChangeEffectVisibility;
    }

    private void ChangeEffectVisibility(MovableObject movable)
    {
        outline.enabled = this.movable == movable;
    }
}
