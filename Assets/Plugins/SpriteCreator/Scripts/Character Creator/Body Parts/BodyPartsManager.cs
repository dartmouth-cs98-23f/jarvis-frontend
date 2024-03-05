// Code written by tutmo (youtube.com/tutmo)
// For help, check out the tutorial - https://youtu.be/PNWK5o9l54w

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BodyPartsManager : MonoBehaviour
{
    // ~~ 1. Updates All Animations to Match Player Selections

    [SerializeField] private SO_CharacterBody characterBody;

    // String Arrays
    [SerializeField] private string[] bodyPartTypes;
    [SerializeField] private string[] characterStates;
    [SerializeField] private string[] characterDirections;
    
    // Animation
    private Animator animator;
    private AnimationClip animationClip;
    private AnimatorOverrideController animatorOverrideController;
    private AnimationClipOverrides defaultAnimationClips;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "CharacterCreator")
        {
            // Set animator
            animator = GetComponent<Animator>();
            animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverrideController;

            defaultAnimationClips = new AnimationClipOverrides(animatorOverrideController.overridesCount);
            animatorOverrideController.GetOverrides(defaultAnimationClips);

            // Set body part animations
            UpdateBodyParts();
        }
    }

    public void UpdateBodyParts()
    {
        // Override default animation clips with character body parts
        for (int partIndex = 0; partIndex < bodyPartTypes.Length; partIndex++)
        {
            // Get current body part
            string partType = bodyPartTypes[partIndex];
            // Get current body part ID
            string partID = characterBody.characterBodyParts[partIndex].bodyPart.bodyPartAnimationID.ToString();
            Debug.Log("partType " + partType + " partID " + partID + " Time " + Time.time);
            
            for (int stateIndex = 0; stateIndex < characterStates.Length; stateIndex++)
            {
                string state = characterStates[stateIndex];
                for (int directionIndex = 0; directionIndex < characterDirections.Length; directionIndex++)
                {
                    string direction = characterDirections[directionIndex];

                    // Get players animation from player body
                    // ***NOTE: Unless Changed Here, Animation Naming Must Be: "[Type]_[Index]_[state]_[direction]" (Ex. Body_0_idle_down)
                    animationClip = Resources.Load<AnimationClip>("Player Animations/" + partType + "/" + partType + "_" + partID + "_" + state + "_" + direction);
                    Debug.Log(animationClip);

                    // Override default animation
                    defaultAnimationClips[partType + "_" + 0 + "_" + state + "_" + direction] = animationClip;
                    // Debug.Log("DAC " + defaultAnimationClips[partType + "_" + 0 + "_" + state + "_" + direction]);
                    Debug.Log("DAC " + defaultAnimationClips["Torso_0_idle_up"]);
                }
            }
        }

        // Apply updated animations
        animatorOverrideController.ApplyOverrides(defaultAnimationClips);
    }

    public void SetSprite(List<int> indices){
        animator = GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        defaultAnimationClips = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(defaultAnimationClips);

        // Override default animation clips with character body parts
        // Debug.Log("Setting sprite with indices: " + indices[0] + ", " + indices[1] + ", " + indices[2] + ", " + indices[3]);
        if (indices == null) {
            indices = new List<int>() { 0, 0, 0, 0 };
            Debug.Log("Provided body part sprite values were null. Setting to default values.");
        }
        for (int partIndex = 0; partIndex < bodyPartTypes.Length; partIndex++)
        {
            // Get current body part
            string partType = bodyPartTypes[partIndex];
            // Get current body part ID
            string partID = indices[partIndex].ToString();
            
            for (int stateIndex = 0; stateIndex < characterStates.Length; stateIndex++)
            {
                string state = characterStates[stateIndex];
                for (int directionIndex = 0; directionIndex < characterDirections.Length; directionIndex++)
                {
                    string direction = characterDirections[directionIndex];

                    // Get players animation from player body
                    // ***NOTE: Unless Changed Here, Animation Naming Must Be: "[Type]_[Index]_[state]_[direction]" (Ex. Body_0_idle_down)
                    animationClip = Resources.Load<AnimationClip>("Player Animations/" + partType + "/" + partType + "_" + partID + "_" + state + "_" + direction);

                    // Override default animation
                    defaultAnimationClips[partType + "_" + 0 + "_" + state + "_" + direction] = animationClip;
                }
            }
        }

        // Apply updated animations
        animatorOverrideController.ApplyOverrides(defaultAnimationClips);
    }

    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
    {
        public AnimationClipOverrides(int capacity) : base(capacity) { }

        public AnimationClip this[string name]
        {
            get { return this.Find(x => x.Key.name.Equals(name)).Value; }
            set
            {
                int index = this.FindIndex(x => x.Key.name.Equals(name));
                if (index != -1)
                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
        }
    }
}