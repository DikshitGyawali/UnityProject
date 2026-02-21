using UnityEngine;

public class EyeManipulation : MonoBehaviour
{
    public PlayerMovement playerMovement;
    [SerializeField] private Transform eye;
    public Transform pupil;
    [SerializeField] private float maxPupilDistance;
    [SerializeField] private float maxEyeDistanceX;
    [SerializeField] private float followSpeed;
    public bool rotatePupil = true;
    public bool eyeInControl= false;

    void Update()
    {
        if (rotatePupil)
        {
            pupil.localEulerAngles += 100 * Time.deltaTime * new Vector3(pupil.localEulerAngles.x,pupil.localEulerAngles.y,6);
        }
        else if (!rotatePupil)
        {
            pupil.localEulerAngles = new Vector3(pupil.localEulerAngles.x,pupil.localEulerAngles.y,-90);
        }
    }
            
    void LateUpdate()
    {
        Vector3 newEyePosition;
        Vector3 newPupilPosition;
        if (eyeInControl){
        Vector3 direction = (playerMovement.transform.position - this.transform.position).normalized;

        newEyePosition = new Vector3(direction.x * maxEyeDistanceX, eye.localPosition.y, eye.localPosition.z);

        newPupilPosition = new Vector3(direction.y * maxPupilDistance, pupil.localPosition.y, pupil.localPosition.z);
        }
        else
        {
            newEyePosition = new Vector3(0, eye.localPosition.y, eye.localPosition.z);
            newPupilPosition = new Vector3(maxPupilDistance, pupil.localPosition.y, pupil.localPosition.z);
        }
        eye.localPosition = Vector3.Lerp(eye.localPosition, newEyePosition, followSpeed * Time.deltaTime);
        pupil.localPosition = Vector3.Lerp(pupil.localPosition, newPupilPosition, followSpeed * Time.deltaTime);
    }
}
