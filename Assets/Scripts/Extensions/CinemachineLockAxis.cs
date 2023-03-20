using UnityEngine;
using Cinemachine;
 
/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")] // Hide in menu
public class LockCameraAxis : CinemachineExtension
{
    [Tooltip("Lock the camera's axis position to this value")]
    public float m_Position;
    public char m_Axis;
 
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            switch (m_Axis)
            {
                case 'X':
                    pos.x = m_Position;
                    break;
                case 'Y':
                    pos.y = m_Position;
                    break;
                case 'Z':
                    pos.z = m_Position;
                    break;
                    
            }
            state.RawPosition = pos;
        }
    }
}
