using UnityEngine;

namespace Camera
{
    public class Camera : MonoBehaviour
    {
        // right click : enable mouse camera movement
        // wasd : basic movement
        // q/e : increase/decrease height
        // shift : Makes camera accelerate
        // space : Moves camera on X and Z axis only.  So camera doesn't gain any height

        public float cameraSpeed = 100.0f;
        public float mouseSensitivity = 0.25f;

        Vector3 _lastMousePosition;

        void Update()
        {
            ProcessMouseInput();
            ProcessKeyboardInput();
        }

        void ProcessMouseInput()
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                _lastMousePosition = Input.mousePosition - _lastMousePosition;
                _lastMousePosition = new Vector3(-_lastMousePosition.y * mouseSensitivity, _lastMousePosition.x * mouseSensitivity, 0);
                _lastMousePosition = new Vector3(transform.eulerAngles.x + _lastMousePosition.x, transform.eulerAngles.y + _lastMousePosition.y, 0);

                transform.eulerAngles = _lastMousePosition;
            }

            _lastMousePosition = Input.mousePosition;
        }

        void ProcessKeyboardInput()
        {
            Vector3 position = GetNextPosition();

            if (position.sqrMagnitude > 0)
            {
                position = position * cameraSpeed * Time.deltaTime;
                transform.Translate(position);
            }
        }

        Vector3 GetNextPosition()
        {
            Vector3 velocity = new Vector3();

            if (Input.GetKey(KeyCode.W))
            {
                velocity += new Vector3(0, 0, 1);
            }

            if (Input.GetKey(KeyCode.S))
            {
                velocity += new Vector3(0, 0, -1);
            }

            if (Input.GetKey(KeyCode.A))
            {
                velocity += new Vector3(-1, 0, 0);
            }

            if (Input.GetKey(KeyCode.D))
            {
                velocity += new Vector3(1, 0, 0);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                velocity += new Vector3(0, 1, 0);
            }

            if (Input.GetKey(KeyCode.E))
            {
                velocity += new Vector3(0, -1, 0);
            }

            return velocity;
        }
    }
}