using UnityEngine;
using System.Collections;

public class ParticleControlScript : MonoBehaviour {

    public ParticleSystem ParticlePrefab;

    private ParticleSystem mParticleInstance;
    private bool mIsLaunched;

	void Start () 
    {
        if (ParticlePrefab == null)
        {
            Debug.LogError("No particle to control!");
            GameObject.Destroy(this);
        }

        mIsLaunched = false;
	}
	
	void Update () 
    {
        if (mIsLaunched && mParticleInstance != null && mParticleInstance.isStopped)
        {
            GameObject.Destroy(mParticleInstance.gameObject);
        }
	}

    public void Play(Vector3 position)
    {
        mIsLaunched = true;

        mParticleInstance = GameObject.Instantiate(ParticlePrefab) as ParticleSystem;
        mParticleInstance.transform.position = position;
        mParticleInstance.Play();
    }
}
