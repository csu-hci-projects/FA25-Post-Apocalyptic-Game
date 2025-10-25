using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public GameObject playerPhase;
    public GameObject attackPhase;

    public void ShowPlayerPhase(){
        playerPhase.SetActive(true);
        attackPhase.SetActive(false);
    }

    public void ShowAttackPhase(){
        playerPhase.SetActive(false);
        attackPhase.SetActive(true);
    }
}
