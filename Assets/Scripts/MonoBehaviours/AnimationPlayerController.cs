using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayerController : MonoBehaviour
{
    [Header("Waving settings")]
    [SerializeField] private float weightAdditivelayer = 0.75f;
    [SerializeField] private float walkSpeed = 4;  
    private float wavingTime;
    private bool isWaving = false;   
    private bool isDead = false;   
   
    private Animator playerAnimator;    
    private int upperBodyLayerIndex;
    private float horizontalSpeed;
    private float currentWeightAdditiveLayer;     

    private void Start()
    {
        wavingTime = 0;      
        playerAnimator = GetComponent<Animator>();
        upperBodyLayerIndex = playerAnimator.GetLayerIndex("UpperBodyAdditive");        
    }    

    private void Update()
    {                     
        playerAnimator.SetFloat("Velocity", horizontalSpeed);

        if (horizontalSpeed >= walkSpeed)
            isWaving = false;

        if (isWaving)
        {
            wavingTime -= Time.deltaTime;
            if (wavingTime <= 0)
                ResetWavingAnim();
        }

        else if (wavingTime > 0 && isWaving == false)       
            ResetWavingAnim();        
    }

    public void SetHorizontalSpeed(float horizontalSpeed)
    {
        this.horizontalSpeed = horizontalSpeed;
    }

    public void PlayDeathAnim()
    {
        isDead = true;
        playerAnimator.SetTrigger("IsDead");
    }

    public void PlayWavingAnim()
    {
        if (isWaving == false && horizontalSpeed <= walkSpeed && isDead == false)
        {
            isWaving = true;
            currentWeightAdditiveLayer = weightAdditivelayer;
            playerAnimator.Play(0,upperBodyLayerIndex);
            playerAnimator.SetLayerWeight(upperBodyLayerIndex,currentWeightAdditiveLayer);            
            wavingTime = playerAnimator.GetCurrentAnimatorStateInfo(upperBodyLayerIndex).length;
        }
    }

    private void ResetWavingAnim()
    {
        if (wavingTime > 0)
        {
            isWaving = false;
            wavingTime -= Time.deltaTime;
            currentWeightAdditiveLayer -= Time.deltaTime;
            if (currentWeightAdditiveLayer <= 0)
                wavingTime = 0;
        }
        else
        {            
            currentWeightAdditiveLayer = 0;            
            isWaving = false;            
        }       
        playerAnimator.SetLayerWeight(upperBodyLayerIndex, currentWeightAdditiveLayer);
    }   
}
