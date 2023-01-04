using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CardinalDirections
{
    up, down, left, right
}
public class Resources : MonoBehaviour
{
    public static Resources sharedInstance;
    public Dictionary<CardinalDirections, Vector2> cardinalDirections;

    private void Awake()
    {
        if (sharedInstance == null) sharedInstance = this;
        else Destroy(this);
        cardinalDirections = new Dictionary<CardinalDirections, Vector2>();
        cardinalDirections.Add(CardinalDirections.up, Vector2.up);
        cardinalDirections.Add(CardinalDirections.right, Vector2.right);
        cardinalDirections.Add(CardinalDirections.left, Vector2.left);
        cardinalDirections.Add(CardinalDirections.down, Vector2.down);
    }
    public Vector2 GetRandomCardinalDirection()
    {
        int nextDirectionIndex = Random.Range(0, cardinalDirections.Count);
        Vector2 randomCardinalDirection = cardinalDirections[(CardinalDirections)nextDirectionIndex];
        return randomCardinalDirection;
    }    
    
    public Vector2 RandomVector2()
    {
        float x = Random.Range(-1f, 1f), y = Random.Range(-1f, 1f);
        Vector2 randomDirection = new Vector2(x,y);
        return randomDirection;
    }
    
    public Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector2.Lerp(start, end, t);

        return new Vector2(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t));
    }

    //Convert a vector into a cardinal direction.
    public Vector2 GetCardinalDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
                return Vector2.right;
            else
                return Vector2.left;
        }
        else
        {
            if (direction.y > 0)
                return Vector2.up;
            else
                return Vector2.down;
        }
    }
}