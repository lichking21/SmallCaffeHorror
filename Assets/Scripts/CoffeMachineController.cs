using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class CoffeMachineController : MonoBehaviour
{
    [SerializeField] private Transform placePoint;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip machineSound;
    [SerializeField] private AudioClip readySound;
    private float workingTime = 3f;
    private bool isON;
    public bool isPouring;

    public void PlaceCup(GameObject cup)
    {
        if (!isON || cup == null || isPouring) return;

        cup.transform.SetParent(placePoint);
        cup.transform.localPosition = Vector3.zero;
        cup.transform.localRotation = Quaternion.Euler(0, 0, 0);

        var rb = cup.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        if (!isPouring)
        StartCoroutine(StartPouring());
    }

    public void TrunOn()
    {
        if (isON) return;

        isON = true;
    }

    private IEnumerator StartPouring()
    {
        isPouring = true;

        if (audioSource != null && machineSound != null) audioSource.PlayOneShot(machineSound);
        yield return new WaitForSeconds(workingTime);

        if (audioSource != null && readySound != null) audioSource.PlayOneShot(readySound);

        isPouring = false;
    }
}