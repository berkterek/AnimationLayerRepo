using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimatorLayerChangeController : MonoBehaviour
{
    [SerializeField] InputActionReference _baseLayerInputReference;
    [SerializeField] InputActionReference _firstLayerInputReference;
    [SerializeField] InputActionReference _secondLayerInputReference;
    [SerializeField] Animator _animator;
    [SerializeField] float _lerpSpeed = 1f;

    void OnValidate()
    {
        if (_animator == null) _animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        _baseLayerInputReference.action.Enable();
        _firstLayerInputReference.action.Enable();
        _secondLayerInputReference.action.Enable();

        _baseLayerInputReference.action.performed += HandleOnBaseLayerPressed;
        _baseLayerInputReference.action.canceled += HandleOnBaseLayerPressed;

        _firstLayerInputReference.action.performed += HandleOnFirstLayerPressed;
        _firstLayerInputReference.action.canceled += HandleOnFirstLayerPressed;

        _secondLayerInputReference.action.performed += HandleOnSecondLayerPressed;
        _secondLayerInputReference.action.canceled += HandleOnSecondLayerPressed;
    }

    void OnDisable()
    {
        _baseLayerInputReference.action.Disable();
        _firstLayerInputReference.action.Disable();
        _secondLayerInputReference.action.Disable();

        _baseLayerInputReference.action.performed -= HandleOnBaseLayerPressed;
        _baseLayerInputReference.action.canceled -= HandleOnBaseLayerPressed;

        _firstLayerInputReference.action.performed -= HandleOnFirstLayerPressed;
        _firstLayerInputReference.action.canceled -= HandleOnFirstLayerPressed;

        _secondLayerInputReference.action.performed -= HandleOnSecondLayerPressed;
        _secondLayerInputReference.action.canceled -= HandleOnSecondLayerPressed;
    }

    void HandleOnBaseLayerPressed(InputAction.CallbackContext context)
    {
        if (_baseLayerInputReference.action.WasPressedThisFrame())
        {
            Debug.Log("Base layer");
            StartCoroutine(NoWeaponSelectionAsync());
        }
    }

    void HandleOnFirstLayerPressed(InputAction.CallbackContext context)
    {
        if (_firstLayerInputReference.action.WasPressedThisFrame())
        {
            Debug.Log("First layer");

            StartCoroutine(PistolSelectionAsync());
        }
    }

    void HandleOnSecondLayerPressed(InputAction.CallbackContext context)
    {
        if (_secondLayerInputReference.action.WasPressedThisFrame())
        {
            Debug.Log("Second layer");
        }
    }

    private IEnumerator NoWeaponSelectionAsync()
    {
        float timer = 0f;
        float weight = 1f;
        while (_animator.GetLayerWeight(1) > 0.1f)
        {
            timer += Time.deltaTime * _lerpSpeed;
            weight = Mathf.Lerp(weight, 0f, timer);
            _animator.SetLayerWeight(1, weight);
            yield return null;
        }
        
        _animator.SetLayerWeight(1, 0f);
    }

    private IEnumerator PistolSelectionAsync()
    {
        float timer = 0f;
        float weight = 0f;

        while (_animator.GetLayerWeight(1) < 0.99f)
        {
            timer += Time.deltaTime * _lerpSpeed;
            weight = Mathf.Lerp(weight, 1f, timer);
            _animator.SetLayerWeight(1, weight);
            yield return null;
        }
        
        _animator.SetLayerWeight(1, 1f);
    }
}