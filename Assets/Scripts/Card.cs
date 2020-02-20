using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardDirection { AxisX, AxisZ }

public class Card : MonoBehaviour
{
    private float movimentationDistance = 2f;
    public float velocity = 1.2f;
    public bool isMoving = true;
    public GameObject cardBlock;
    private CardDirection direction = CardDirection.AxisX;
    private bool movingUp = false;
    

    void Update()
    {
        UpdatePosition();
    }

    public Vector2 GetPositionXZ()
    {
        return new Vector2(transform.position.x, transform.position.z);
    }

    public Color GetColor()
    {
        var renderer = cardBlock.GetComponent<Renderer>();
        return renderer.material.color;
    }

    public void SetColor(Color newColor)
    {
        var renderer = cardBlock.GetComponent<Renderer>();
        renderer.material.color = newColor;
        UpdateVelocityBasedOnColor();
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public void SetDirection(CardDirection newDirection)
    {
        direction = newDirection;
    }

    public void StopMovement()
    {
        isMoving = false;
    }

    private void UpdateVelocityBasedOnColor()
    {
        float nearRed, nearBlack, maxMultiplier;
        Color color = GetColor();
        nearRed = color.r - color.g - color.b;
        nearBlack = 1f - (color.r + color.g + color.b);
        maxMultiplier = Mathf.Max(nearRed, nearBlack) + 1f;
        velocity *= Mathf.Clamp(maxMultiplier, 1f, 2f);
    }

    public void ActivatePhysics()
    {
        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.AddTorque(new Vector3(1f, 0f, 1f), ForceMode.VelocityChange);
        StartCoroutine(SelfDestruct());
    }

    public void SetPosition(Vector3 referencePosition)
    {
        transform.localPosition = referencePosition;
        if (direction == CardDirection.AxisX)
            transform.localPosition += new Vector3(movimentationDistance, 0f, 0f);
        else
            transform.localPosition += new Vector3(0f, 0f, movimentationDistance);
    }

    private void UpdatePosition()
    {
        if (isMoving)
        {
            float currentOffset = GetOffset();
            if (movingUp)
            {
                currentOffset += velocity * Time.deltaTime;
                if (currentOffset > movimentationDistance)
                    movingUp = false;
            }
            else
            {
                currentOffset -= velocity * Time.deltaTime;
                if (currentOffset < -movimentationDistance)
                    movingUp = true;
            }
            SetOffset(currentOffset);
        }
    }

    private float GetOffset()
    {
        if (direction == CardDirection.AxisX)
            return transform.localPosition.x;
        else
            return transform.localPosition.z;
    }

    private void SetOffset(float offset)
    {
        Vector3 newPosition = transform.localPosition;
        if (direction == CardDirection.AxisX)
            newPosition.x = offset;
        else
            newPosition.z = offset;
        transform.localPosition = newPosition;
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5f);
        if (transform.position.y < 0f)
            Destroy(gameObject);
    }
}
