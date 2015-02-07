// Version 1.3.9
// ©2013 Reindeer Games
// All rights reserved
// Redistribution of source code without permission not allowed

using Exploder;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (ExploderObject)), CanEditMultipleObjects]
public class EditorExploderObject : UnityEditor.Editor
{
    SerializedProperty radius;
    SerializedProperty force;
    SerializedProperty targetFragments;
    SerializedProperty frameBudget;
    SerializedProperty useForceVector;
    SerializedProperty forceVector;
    SerializedProperty ignoreTag;
    SerializedProperty meshColliders;
    SerializedProperty explodeSelf;
    SerializedProperty disableRadiusScan;
    SerializedProperty hideSelf;
    SerializedProperty deleteOriginalObjects;
    SerializedProperty uniformDistribution;
    SerializedProperty splitMeshIslands;
    SerializedProperty openMeshCutting;
    SerializedProperty user2dPhysics;
    SerializedProperty deactivateOn;
    SerializedProperty deactivateTimeout;
    SerializedProperty fadeout;
    SerializedProperty explodableFragments;
    SerializedProperty poolSize;
    SerializedProperty layer;
    SerializedProperty maxVelocity;
    SerializedProperty maxAngularVelocity;
    SerializedProperty inheritParentPhysics;
    SerializedProperty mass;
    SerializedProperty useGravity;
    SerializedProperty disableColliders;
    SerializedProperty angularVelocity;
    SerializedProperty randomAngularVelocity;
    SerializedProperty angularVelocityVector;
    SerializedProperty freezePositionX;
    SerializedProperty freezePositionY;
    SerializedProperty freezePositionZ;
    SerializedProperty freezeRotationX;
    SerializedProperty freezeRotationY;
    SerializedProperty freezeRotationZ;
    SerializedProperty explosionSound;
    SerializedProperty fragmentHitSound;
    SerializedProperty fragmentHitSoundTimeout;
    SerializedProperty fragmentParticles;
    SerializedProperty fragmentParticlesMax;
    SerializedProperty fragmentMaterial;

    private void OnEnable()
    {
        radius = serializedObject.FindProperty("Radius");
        force = serializedObject.FindProperty("Force");
        targetFragments = serializedObject.FindProperty("TargetFragments");
        frameBudget = serializedObject.FindProperty("FrameBudget");
        useForceVector = serializedObject.FindProperty("UseForceVector");
        forceVector = serializedObject.FindProperty("ForceVector");
        ignoreTag = serializedObject.FindProperty("DontUseTag");
        meshColliders = serializedObject.FindProperty("MeshColliders");
        explodeSelf = serializedObject.FindProperty("ExplodeSelf");
        disableRadiusScan = serializedObject.FindProperty("DisableRadiusScan");
        hideSelf = serializedObject.FindProperty("HideSelf");
        deleteOriginalObjects = serializedObject.FindProperty("DestroyOriginalObject");
        uniformDistribution = serializedObject.FindProperty("UniformFragmentDistribution");
        splitMeshIslands = serializedObject.FindProperty("SplitMeshIslands");
        openMeshCutting = serializedObject.FindProperty("AllowOpenMeshCutting");
        user2dPhysics = serializedObject.FindProperty("Use2DCollision");
        deactivateOn = serializedObject.FindProperty("DeactivateOptions");
        deactivateTimeout = serializedObject.FindProperty("DeactivateTimeout");
        fadeout = serializedObject.FindProperty("FadeoutOptions");
        explodableFragments = serializedObject.FindProperty("ExplodeFragments");
        poolSize = serializedObject.FindProperty("FragmentPoolSize");
        layer = serializedObject.FindProperty("FragmentOptions.Layer");
        maxVelocity = serializedObject.FindProperty("FragmentOptions.MaxVelocity");
        maxAngularVelocity = serializedObject.FindProperty("FragmentOptions.MaxAngularVelocity");
        inheritParentPhysics = serializedObject.FindProperty("FragmentOptions.InheritParentPhysicsProperty");
        mass = serializedObject.FindProperty("FragmentOptions.Mass");
        useGravity = serializedObject.FindProperty("FragmentOptions.UseGravity");
        disableColliders = serializedObject.FindProperty("FragmentOptions.DisableColliders");
        angularVelocity = serializedObject.FindProperty("FragmentOptions.AngularVelocity");
        randomAngularVelocity = serializedObject.FindProperty("FragmentOptions.RandomAngularVelocityVector");
        angularVelocityVector = serializedObject.FindProperty("FragmentOptions.AngularVelocityVector");
        freezePositionX = serializedObject.FindProperty("FragmentOptions.FreezePositionX");
        freezePositionY = serializedObject.FindProperty("FragmentOptions.FreezePositionY");
        freezePositionZ = serializedObject.FindProperty("FragmentOptions.FreezePositionZ");
        freezeRotationX = serializedObject.FindProperty("FragmentOptions.FreezeRotationX");
        freezeRotationY = serializedObject.FindProperty("FragmentOptions.FreezeRotationY");
        freezeRotationZ = serializedObject.FindProperty("FragmentOptions.FreezeRotationZ");
        fragmentMaterial = serializedObject.FindProperty("FragmentOptions.FragmentMaterial");
        explosionSound = serializedObject.FindProperty("SFXOptions.ExplosionSoundClip");
        fragmentHitSound = serializedObject.FindProperty("SFXOptions.FragmentSoundClip");
        fragmentHitSoundTimeout = serializedObject.FindProperty("SFXOptions.HitSoundTimeout");
        fragmentParticles = serializedObject.FindProperty("SFXOptions.FragmentEmitter");
        fragmentParticlesMax = serializedObject.FindProperty("SFXOptions.EmitersMax");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var exploder = this.target as ExploderObject;

        if (exploder)
        {
            var change = false;

            EditorExploderUtils.Separator("Main Settings", 20);
            EditorGUILayout.Space();

            GUI.enabled = !(exploder.ExplodeSelf && exploder.DisableRadiusScan);
            change |= EditorExploderUtils.SliderEdit("Radius", "Explosion radius.", 0.0f, 100, radius);
            GUI.enabled = true;

            change |= EditorExploderUtils.SliderEdit("Force", "Force of explosion.", 0.0f, 100, force);
            change |= EditorExploderUtils.SliderEdit("Target Fragments", "Number of target fragments.", 0, 500, targetFragments);
            change |= EditorExploderUtils.SliderEdit("Frame Budget [ms]", "Time budget in [ms] for processing explosion calculation in one frame.", 0.0f, 60.0f, frameBudget);
            change |= EditorExploderUtils.Toggle("Use Force Vector", "Use this vector as a direction of explosion.", useForceVector);

            if (exploder.UseForceVector)
            {
                change |= EditorExploderUtils.Vector3("", "Use this vector as a direction of explosion.", forceVector);
            }

            change |= EditorExploderUtils.Toggle("Ignore Tag", "Ignore Exploder tag on object, use Explodable component instead.", ignoreTag);
            change |= EditorExploderUtils.Toggle("Mesh colliders", "Use mesh colliders for all fragments.", meshColliders);

            change |= EditorExploderUtils.Toggle("Explode self", "Explode this game object.", explodeSelf);
//            else
//            {
//                change |= EditorExploderUtils.Toggle2("Explode self", "Explode this game object.", "", "Disable radius",
//                                                      explodeSelf, disableRadiusScan);
//            }

            if (exploder.ExplodeSelf)
            {
                change |= EditorExploderUtils.Toggle("Disable radius", "Disable scanning for objects in radius.", disableRadiusScan);
            }

            change |= EditorExploderUtils.Toggle("Hide self", "Hide this game object after explosion.", hideSelf);
            change |= EditorExploderUtils.Toggle("Delete original object", "Delete original game object after explosion.", deleteOriginalObjects);
            change |= EditorExploderUtils.Toggle("Uniform distribution", "Uniformly distribute fragments inside the radius.", uniformDistribution);
            change |= EditorExploderUtils.Toggle("Split mesh islands", "Split non-connecting part of the mesh into separate fragments.", splitMeshIslands);
            change |= EditorExploderUtils.Toggle("Open mesh cutting", "Enable explosion of non-closed mesh.", openMeshCutting);
            change |= EditorExploderUtils.Toggle("Use 2D physics", "Enable 2D explosion.", user2dPhysics);

            EditorGUILayout.Space();
            EditorExploderUtils.Separator("Deactivation options", 20);
            EditorGUILayout.Space();

            EditorExploderUtils.EnumSelection("Deactivate on", "Options for fragment deactivation.", exploder.DeactivateOptions, deactivateOn, ref change);

            if (exploder.DeactivateOptions == DeactivateOptions.Timeout)
            {
                change |= EditorExploderUtils.SliderEdit("Deactivate Timeout [s]", "Time in [s] to deactivate fragments.", 0.0f, 60.0f, deactivateTimeout);
                EditorExploderUtils.EnumSelection("FadeOut", "Option for fragments to fadeout during deactivation timeout.", exploder.FadeoutOptions, fadeout, ref change);
            }

            EditorGUILayout.Space();
            EditorExploderUtils.Separator("Fragment options", 20);
            EditorGUILayout.Space();

            change |= EditorExploderUtils.Toggle("Explodable fragments", "Enable fragments to be exploded again.", explodableFragments);
            change |= EditorExploderUtils.SliderEdit("Pool Size", "Size of the fragment pool, this value should be higher than TargetFragments.", 0, 500, poolSize);

            change |= EditorExploderUtils.String("Layer", "Layer of the fragment game object.", layer);
            change |= EditorExploderUtils.SliderEdit("MaxVelocity", "Maximal velocity that fragment can have.", 0.0f, 100.0f, maxVelocity);
            change |= EditorExploderUtils.SliderEdit("MaxAngularVelocity", "Maximal angular velocity that fragment can have.", 0.0f, 30.0f, maxAngularVelocity);
            change |= EditorExploderUtils.Toggle("Inherit parent physics", "Use the same physics settings as in original game object.", inheritParentPhysics);
            change |= EditorExploderUtils.SliderEdit("Mass", "Mass property of every fragment.", 0.0f, 100.0f, mass);
            change |= EditorExploderUtils.Toggle("Use gravity", "Apply gravity to fragment.", useGravity);

            change |= EditorExploderUtils.Toggle("Disable colliders", "Disable colliders of all fragments.", disableColliders);

            change |= EditorExploderUtils.SliderEdit("Angular velocity", "Angular velocity of fragments.", 0.0f, 100.0f, angularVelocity);
            change |= EditorExploderUtils.Toggle("Random angular vector", "Randomize rotation of fragments.", randomAngularVelocity);

            if (!exploder.FragmentOptions.RandomAngularVelocityVector)
            {
                change |= EditorExploderUtils.Vector3("", "Use this vector as a angular velocity vector.", angularVelocityVector);
            }

            change |= EditorExploderUtils.Toggle3("Freeze Position", "Freeze position of the fragment in selected axis.",
                                                  "x", "y", "z", freezePositionX,
                                                  freezePositionY,
                                                  freezePositionZ);

            change |= EditorExploderUtils.Toggle3("Freeze Rotation", "Freeze rotation of the fragment in selected axis.",
                                      "x", "y", "z", freezeRotationX,
                                      freezeRotationY,
                                      freezeRotationZ);

            change |= EditorExploderUtils.ObjectSelection<Material>("Material", "Optional material for fragments.",
                                                                    fragmentMaterial);

            EditorGUILayout.Space();
            EditorExploderUtils.Separator("SFX options", 20);
            EditorGUILayout.Space();

            change |= EditorExploderUtils.ObjectSelection<AudioClip>("Explosion sound", "Sound effect played on explosion.", explosionSound);
            change |= EditorExploderUtils.ObjectSelection<AudioClip>("Fragment hit sound", "Sound effect when the fragment hits another collider (wall, floor).", fragmentHitSound);

            if (exploder.SFXOptions.FragmentSoundClip)
            {
                change |= EditorExploderUtils.SliderEdit("Hit sound timeout", "Timeout between sound effects.",
                                                         0.0f, 1.0f, fragmentHitSoundTimeout);
            }

            change |= EditorExploderUtils.ObjectSelection<GameObject>("Fragment particles", "Particle effect that will start to emit from each fragment after explosion.", fragmentParticles);

            if (exploder.SFXOptions.FragmentEmitter)
            {
                change |= EditorExploderUtils.IntEdit("Maximum emitters", "Maximumal number of emmiters.", 0, 1000,
                                                      fragmentParticlesMax);
            }

            if (change)
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(exploder);
            }

            EditorGUILayout.Separator();
        }
    }
}
