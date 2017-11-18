using UnityEngine;
using System.Collections;

public class StopFastForwardingWhenDisabled : MonoBehaviour
{
    void OnDisable()
    {
        VNSceneManager.scene_manager.Stop_Fast_Forward();
    }
}
