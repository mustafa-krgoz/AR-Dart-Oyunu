using System.Collections;
using UnityEngine;

public class Dart : MonoBehaviour
{
    private Rigidbody rg;
    private GameObject dirObj;
    public bool isForceOK = false;
    bool isDartRotating = false;
    bool isDartReadyToShoot = true;
    bool isDartHitOnBoard = false;

    public Collider dartFrontCollider;

    
    private bool isScored = false;

    
    void Start()
    {
        
        if (TryGetComponent(out Rigidbody rigid))
            rg = rigid;

        dirObj = GameObject.FindGameObjectWithTag("DartThrowPoint");
    }

    private void FixedUpdate()
    {
        if (isForceOK)
        {
            dartFrontCollider.enabled = true;
            StartCoroutine(InitDartDestroyVFX());

            if (TryGetComponent(out Rigidbody rigid))
                rigid.isKinematic = false;

            isForceOK = false;
            isDartRotating = true;
        }

        
        rg.AddForce(transform.forward * (12f + 6f) * Time.deltaTime, ForceMode.VelocityChange);

        
        if (isDartReadyToShoot)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * 20f);
        }

        
        if (isDartRotating)
        {
            isDartReadyToShoot = false;
            transform.Rotate(Vector3.forward * Time.deltaTime * 400f);
        }
    }

    IEnumerator InitDartDestroyVFX()
    {
        yield return new WaitForSeconds(7f);
        Destroy(gameObject); 
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("dart_board") && !isScored)
        {
            
            Handheld.Vibrate();

            
            isDartHitOnBoard = true;
            GetComponent<Rigidbody>().isKinematic = true;
            isDartRotating = false;

            
            DartController dartController = FindObjectOfType<DartController>();
            dartController.IncreaseScore();

            
            isScored = true;
        }
    }
}
