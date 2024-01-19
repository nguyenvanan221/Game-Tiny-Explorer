using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arc : MonoBehaviour
{
    public IEnumerator TravelArc(Vector3 destination, float duration)
    {
        var startPosition = transform.position;

        var percentComplete = 0.0f;

        while (percentComplete < 1.0f)
        {
            // time.deltatime is the time since the last frame waw drawn
            percentComplete += Time.deltaTime / duration ;


            // fire ammo travel in a line
            // transform.position = Vector3.Lerp(startPosition, destination, percentComplete);

            // fire ammo travel in a arc
            var currentHeight = Mathf.Sin(Mathf.PI * percentComplete);
            transform.position = Vector3.Lerp(startPosition, destination, percentComplete) + Vector3.up * currentHeight;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
