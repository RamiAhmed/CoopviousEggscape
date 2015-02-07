// Version 1.3.9
// ©2013 Reindeer Games
// All rights reserved
// Redistribution of source code without permission not allowed

using UnityEngine;

namespace Exploder
{
    /// <summary>
    /// options for deactivating the fragment
    /// </summary>
    public enum DeactivateOptions
    {
        /// <summary>
        /// fragments remain active until they are needed for next explosion
        /// </summary>
        Never,

        /// <summary>
        /// fragment will be deactivated if it is not visible by main camera
        /// </summary>
        OutsideOfCamera,

        /// <summary>
        /// fragment will be deactivated after timeout
        /// </summary>
        Timeout,
    }

    /// <summary>
    /// options for fadeout fragments
    /// </summary>
    public enum FadeoutOptions
    {
        /// <summary>
        /// fragments will be fully visible until deactivation
        /// </summary>
        None,

        /// <summary>
        /// fragments will fadeout on deactivateTimeout
        /// </summary>
        FadeoutAlpha,

        /// <summary>
        /// fragments will scale down to zero on deactivateTimeout
        /// </summary>
        ScaleDown,
    }

    /// <summary>
    /// script component for fragment game object
    /// the only logic here is visibility test against main camera and timeout sleeping for rigidbody
    /// </summary>
    public class Fragment : MonoBehaviour
    {
        /// <summary>
        /// is this fragment explodable
        /// </summary>
        public bool explodable;

        /// <summary>
        /// options for deactivating the fragment after explosion
        /// </summary>
        public DeactivateOptions deactivateOptions;

        /// <summary>
        /// deactivate timeout, valid only if DeactivateOptions == DeactivateTimeout
        /// </summary>
        public float deactivateTimeout = 10.0f;

        /// <summary>
        /// options for fading out fragments after explosion
        /// </summary>
        public FadeoutOptions fadeoutOptions = FadeoutOptions.None;

        /// <summary>
        /// maximum velocity of fragment
        /// </summary>
        public float maxVelocity = 1000;

        /// <summary>
        /// disable colliders
        /// </summary>
        public bool disableColliders = false;

        /// <summary>
        /// timeout to re-enable disabled colliders
        /// </summary>
        public float disableCollidersTimeout;

        /// <summary>
        /// flag if this fragment is visible from main camera
        /// </summary>
        public bool visible;

        /// <summary>
        /// is this fragment active
        /// </summary>
        public bool activeObj;

        /// <summary>
        /// minimum size of fragment bounding box to be explodable (if explodable flag is true)
        /// </summary>
        public float minSizeToExplode = 0.5f;

        /// <summary>
        /// mesh filter component for faster access
        /// </summary>
        public MeshFilter meshFilter;

        /// <summary>
        /// mesh renderer component for faster access
        /// </summary>
        public MeshRenderer meshRenderer;

        /// <summary>
        /// mesh collider component for faster access
        /// </summary>
        public MeshCollider meshCollider;

        /// <summary>
        /// box collider component for faster access
        /// </summary>
        public BoxCollider boxCollider;

        /// <summary>
        /// optional audio source on this fragment
        /// </summary>
        public AudioSource audioSource;

        /// <summary>
        /// optional audio clip for this fragment
        /// </summary>
        public AudioClip audioClip;

        /// <summary>
        /// optional particle emitter for fragment
        /// </summary>
        private ParticleEmitter[] emmiters;

        private GameObject particleChild;

        public PolygonCollider2D polygonCollider2D;

        public Rigidbody2D rigid2D;

        public bool IsSleeping()
        {
            if (rigid2D)
            {
                return rigid2D.IsSleeping();
            }
            return rigidBody.IsSleeping();
        }

        public void Sleep()
        {
            if (rigid2D)
            {
                rigid2D.Sleep();
            }
            else
            {
                rigidBody.Sleep();
            }
        }

        public void WakeUp()
        {
            if (rigid2D)
            {
                rigid2D.WakeUp();
            }
            else
            {
                rigidBody.WakeUp();
            }
        }

        public void SetConstraints(RigidbodyConstraints constraints)
        {
            if (rigidbody)
            {
                rigidBody.constraints = constraints;
            }
        }

        public void SetSFX(ExploderObject.SFXOption sfx, bool allowParticle)
        {
            audioClip = sfx.FragmentSoundClip;
            
            if (audioClip && !audioSource)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            if (sfx.FragmentEmitter && allowParticle)
            {
                if (!particleChild)
                {
                    var dup = Instantiate(sfx.FragmentEmitter) as GameObject;

                    if (dup)
                    {
                        particleChild = new GameObject("Particles");
                        particleChild.transform.parent = gameObject.transform;

                        dup.transform.parent = particleChild.transform;

                        emmiters = dup.GetComponentsInChildren<ParticleEmitter>();
                    }                    
                }
            }
            else
            {
                if (particleChild)
                {
                    Destroy(particleChild);
                }
            }
        }

        void OnCollisionEnter()
        {
            var pool = FragmentPool.Instance;

            if (pool.CanPlayHitSound())
            {
                if (audioClip && audioSource)
                {
                    audioSource.PlayOneShot(audioClip);
                }
                
                pool.OnFragmentHit();
            }
        }

        public void DisableColliders(bool disable, bool meshColliders, bool physics2d)
        {
            if (disable)
            {
                if (physics2d)
                {
                    Object.Destroy(polygonCollider2D);
                }
                else
                {
                    if (meshCollider)
                    {
                        Object.Destroy(meshCollider);
                    }
                    if (boxCollider)
                    {
                        Object.Destroy(boxCollider);
                    }
                }
            }
            else
            {
                if (physics2d)
                {
                    if (!polygonCollider2D)
                    {
                        polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
                    }
                }
                else
                {
                    if (meshColliders)
                    {
                        if (!meshCollider)
                        {
                            meshCollider = gameObject.AddComponent<MeshCollider>();
                        }
                    }
                    else
                    {
                        if (!boxCollider)
                        {
                            boxCollider = gameObject.AddComponent<BoxCollider>();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// apply physical explosion to fragment piece
        /// </summary>
        public void ApplyExplosion(Transform meshTransform, Vector3 centroid, Vector3 mainCentroid, ExploderObject.FragmentOption fragmentOption, 
                                    bool useForceVector, Vector3 ForceVector, float force, GameObject original, int targetFragments)
        {
            if (rigid2D)
            {
                ApplyExplosion2D(meshTransform, centroid, mainCentroid, fragmentOption, useForceVector, ForceVector, force, original, targetFragments);
                return;
            }

            var rigid = rigidBody;

            // apply fragment mass and velocity properties
            var parentVelocity = Vector3.zero;
            var parentAngularVelocity = Vector3.zero;
            var mass = fragmentOption.Mass;
            var useGravity = fragmentOption.UseGravity;

            rigid.maxAngularVelocity = fragmentOption.MaxAngularVelocity;

            // inherit velocity and mass from original object
            if (fragmentOption.InheritParentPhysicsProperty)
            {
                if (original && original.rigidbody)
                {
                    var parentRigid = original.rigidbody;

                    parentVelocity = parentRigid.velocity;
                    parentAngularVelocity = parentRigid.angularVelocity;
                    mass = parentRigid.mass / targetFragments;
                    useGravity = parentRigid.useGravity;
                }
            }

            var forceVector = (meshTransform.TransformPoint(centroid) - mainCentroid).normalized;
            var angularVelocity = fragmentOption.AngularVelocity * (fragmentOption.RandomAngularVelocityVector ? Random.onUnitSphere : fragmentOption.AngularVelocityVector);

            if (useForceVector)
            {
                forceVector = ForceVector;
            }

            rigid.velocity = forceVector * force + parentVelocity;
            rigid.angularVelocity = angularVelocity + parentAngularVelocity;
            rigid.mass = mass;
            maxVelocity = fragmentOption.MaxVelocity;
            rigid.useGravity = useGravity;
        }

        /// <summary>
        /// apply physical explosion to fragment piece (2D case)
        /// </summary>
        void ApplyExplosion2D(Transform meshTransform, Vector3 centroid, Vector3 mainCentroid,
                              ExploderObject.FragmentOption fragmentOption,
                              bool useForceVector, Vector2 ForceVector, float force, GameObject original,
                              int targetFragments)
        {
            var rigid = rigid2D;

            // apply fragment mass and velocity properties
            var parentVelocity = Vector2.zero;
            var parentAngularVelocity = 0.0f;
            var mass = fragmentOption.Mass;

            // inherit velocity and mass from original object
            if (fragmentOption.InheritParentPhysicsProperty)
            {
                if (original && original.rigidbody2D)
                {
                    var parentRigid = original.rigidbody2D;

                    parentVelocity = parentRigid.velocity;
                    parentAngularVelocity = parentRigid.angularVelocity;
                    mass = parentRigid.mass / targetFragments;
                }
            }

            Vector2 forceVector = (meshTransform.TransformPoint(centroid) - mainCentroid).normalized;
            float angularVelocity = fragmentOption.AngularVelocity * (fragmentOption.RandomAngularVelocityVector ? Random.insideUnitCircle.x : fragmentOption.AngularVelocityVector.y);

            if (useForceVector)
            {
                forceVector = ForceVector;
            }

            rigid.velocity = forceVector * force + parentVelocity;
            rigid.angularVelocity = angularVelocity + parentAngularVelocity;
            rigid.mass = mass;
            maxVelocity = fragmentOption.MaxVelocity;
        }

        /// <summary>
        /// options component for faster access
        /// </summary>
        public ExploderOption options;

        /// <summary>
        /// rigidbody component for faster access
        /// </summary>
        public Rigidbody rigidBody;

        /// <summary>
        /// refresh local members components objects
        /// </summary>
        public void RefreshComponentsCache()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            meshCollider = GetComponent<MeshCollider>();
            boxCollider = GetComponent<BoxCollider>();
            options = GetComponent<ExploderOption>();
            rigidBody = GetComponent<Rigidbody>();
            rigid2D = GetComponent<Rigidbody2D>();
            polygonCollider2D = GetComponent<PolygonCollider2D>();
        }

    /// <summary>
        /// this is called from exploder class to start the explosion
        /// </summary>
        public void Explode()
        {
            activeObj = true;
            ExploderUtils.SetActiveRecursively(gameObject, true);
            visibilityCheckTimer = 0.1f;
            visible = true;
            deactivateTimer = deactivateTimeout;
            originalScale = transform.localScale;

            if (explodable)
            {
                tag = ExploderObject.Tag;
            }

            Emit(true);
        }

        public void Emit(bool centerToBound)
        {
            if (emmiters != null)
            {
                if (centerToBound)
                {
                    if (particleChild && meshRenderer)
                    {
                        particleChild.transform.position = meshRenderer.bounds.center;
                    }
                }

                foreach (var emitter in emmiters)
                {
                    emitter.Emit();
                }
            }
        }

        /// <summary>
        /// deactivate this fragment piece
        /// </summary>
        public void Deactivate()
        {
            ExploderUtils.SetActive(gameObject, false);
            visible = false;
            activeObj = false;

            // turn off particles
            if (emmiters != null)
            {
                foreach (var emitter in emmiters)
                {
                    emitter.ClearParticles();
                }
            }
        }

        private Vector3 originalScale;
        private float visibilityCheckTimer;
        private float deactivateTimer;

        void Start()
        {
            visibilityCheckTimer = 1.0f;
            RefreshComponentsCache();
            visible = false;
        }

        void Update()
        {
            if (activeObj)
            {
                //
                // clamp velocity
                //
                if (rigidBody)
                {
                    if (rigidBody.velocity.sqrMagnitude > maxVelocity * maxVelocity)
                    {
                        var vel = rigidBody.velocity.normalized;
                        rigidBody.velocity = vel * maxVelocity;
                    }
                }
                else if (rigid2D)
                {
                    if (rigid2D.velocity.sqrMagnitude > maxVelocity * maxVelocity)
                    {
                        var vel = rigid2D.velocity.normalized;
                        rigid2D.velocity = vel * maxVelocity;
                    }
                }

                if (deactivateOptions == DeactivateOptions.Timeout)
                {
                    deactivateTimer -= Time.deltaTime;

                    if (deactivateTimer < 0.0f)
                    {
                        Sleep();
                        activeObj = false;
                        ExploderUtils.SetActiveRecursively(gameObject, false);

                        // return fragment to previous fadout state
                        switch (fadeoutOptions)
                        {
                            case FadeoutOptions.FadeoutAlpha:
                                break;
                        }
                    }
                    else
                    {
                        var t = deactivateTimer/deactivateTimeout;

                        if (emmiters != null)
                        {
                            foreach (var emitter in emmiters)
                            {
                                for (int i = 0; i < emitter.particles.Length; i++)
                                {
                                    var c = emitter.particles[i].color;
                                    c.a = 1.0f - t;
                                    emitter.particles[i].color = c;
                                }
                            }
                        }

                        switch (fadeoutOptions)
                        {
                            case FadeoutOptions.FadeoutAlpha:
                                if (meshRenderer.material && meshRenderer.material.HasProperty("_Color"))
                                {
                                    var color = meshRenderer.material.color;
                                    color.a = t;
                                    meshRenderer.material.color = color;
                                }
                                break;

                            case FadeoutOptions.ScaleDown:
                                gameObject.transform.localScale = originalScale*t;
                                break;
                        }
                    }
                }

                visibilityCheckTimer -= Time.deltaTime;

                if (visibilityCheckTimer < 0.0f && UnityEngine.Camera.main)
                {
                    var viewportPoint = UnityEngine.Camera.main.WorldToViewportPoint(transform.position);

                    if (viewportPoint.z < 0 || viewportPoint.x < 0 || viewportPoint.y < 0 ||
                        viewportPoint.x > 1 || viewportPoint.y > 1)
                    {
                        if (deactivateOptions == DeactivateOptions.OutsideOfCamera)
                        {
                            Sleep();
                            activeObj = false;
                            ExploderUtils.SetActiveRecursively(gameObject, false);
                        }

                        visible = false;
                    }
                    else
                    {
                        visible = true;
                    }

                    visibilityCheckTimer = Random.Range(0.1f, 0.3f);

                    if (explodable)
                    {
                        var size = collider.bounds.size;

                        if (Mathf.Max(size.x, size.y, size.z) < minSizeToExplode)
                        {
                            tag = string.Empty;
                        }
                    }
                }
            }
        }
    }
}
