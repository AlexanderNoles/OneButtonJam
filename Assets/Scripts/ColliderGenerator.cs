using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

#if UNITY_EDITOR
public class ColliderGenerator : MonoBehaviour
{
    PolygonCollider2D ColliderInstance;
    SpriteRenderer sr;
    Texture2D tex;
    public bool automaticallyGenerate;

    private void Start()
    {
        if (automaticallyGenerate)
        {
            GenerateCollider();
        }
    }

    public void GenerateCollider()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr.sprite.texture.isReadable == false)
        {
            Debug.Log($"Sprite on \"{gameObject.name}\" does not have read/write enabled");
            return;
        }
        tex = sr.sprite.texture;

        ColliderInstance = GetComponent<PolygonCollider2D>();
        if(ColliderInstance == null)    //Check to see if there is a collider on the object
        {
            ColliderInstance = gameObject.AddComponent<PolygonCollider2D>();          //Add a collider if there is none
        }       
       
        List<Vector2> allCorners = GetCoordinatesOfCorners();
        List<List<Vector2>> orderedCorners = OrderCorners(allCorners);  //Get all paths
        List<List<Vector2>> formattedToSprite = FormatToCollider(orderedCorners); //Formatt all vector2s in the path
        //Apply the scaled points
        applyPaths(formattedToSprite);
        //Finally offset the collider so it is correctly placed on top of the game object
        ColliderInstance.offset = new Vector2(-formattedToSprite[0][0].x - (sr.sprite.bounds.size.x / 2), -formattedToSprite[0][0].y + (sr.sprite.bounds.size.y / 2));
    }

    public void applyPaths(List<List<Vector2>> paths)
    {
        ColliderInstance.pathCount = paths.Count;
        for(int i = 0; i<paths.Count; i++)
        {
            ColliderInstance.SetPath(i, paths[i].ToArray());
        }
    }

    public List<List<Vector2>> FormatToCollider(List<List<Vector2>> corners)
    {
        applyPaths(corners);
        float colliderLength = ColliderInstance.bounds.size.x;
        List<List<Vector2>> inflatedPoints = new List<List<Vector2>>();

        for (int i = 0; i < corners.Count; i++)
        {
            List<Vector2> inflatedPoint = new List<Vector2>();

            for(int j = 0; j < corners[i].Count; j++)
            {
                Vector2 inflatedPath = new Vector2(corners[i][j].x, corners[i][j].y);  //Inflate all the points so the correctly reflect the shape of the gameobject rather than a 
                if (inflatedPath.x < colliderLength / 2)                               //scaled down version of it. This needs to be done as the points set as corners on the collider 
                {                                                                     //are actually the centers of the corner pixels.
                    inflatedPath.x -= 0.5f;
                }
                else if (inflatedPath.x >= colliderLength / 2)
                {
                    inflatedPath.x += 0.5f;
                }
                if (inflatedPath.y <= 0)
                {
                    inflatedPath.y -= 0.5f;
                }
                else if (inflatedPath.y > 0)
                {
                    inflatedPath.y += 0.5f;
                }

                inflatedPoint.Add(inflatedPath);
            }

            inflatedPoints.Add(inflatedPoint);
        }

        applyPaths(inflatedPoints); //Apply the newly inflated points so that the scale factor can be calculated correctly
        float scaleFactorX = sr.sprite.bounds.size.x / ColliderInstance.bounds.size.x;
        float scaleFactorY = sr.sprite.bounds.size.y / ColliderInstance.bounds.size.y;
        List<List<Vector2>> scaledPoints = new List<List<Vector2>>();

        for (int i = 0; i < inflatedPoints.Count; i++)
        {
            List<Vector2> scaledPoint = new List<Vector2>();
            for(int j = 0; j < inflatedPoints[i].Count; j++)
            {
                Vector2 scaledPath = new Vector2(inflatedPoints[i][j].x * scaleFactorX, inflatedPoints[i][j].y * scaleFactorY);  //Scale all points down to the size of the object
                scaledPoint.Add(scaledPath);
            }         
            scaledPoints.Add(scaledPoint);
        }
        
        return scaledPoints;
    }

    public List<List<Vector2>> OrderCorners(List<Vector2> corners)
    {
        List<List<Vector2>> toReturn = new List<List<Vector2>>();
        List<Vector2> currentPath = new List<Vector2>();
        bool vertical = true;
        int currentIndex = 0;
        Vector2 startingPos = corners[currentIndex];
        Vector2 currentPos = startingPos;
        int originalCornersLength = corners.Count;
        //Starting at the first corner in the current corners list create a new path
        while (toReturn.Count < originalCornersLength)
        {
            Vector2 toBeAdded = new Vector2();
            float currentHighest = float.MaxValue;            
            //Run through all remaing corners to see if any can be connected to the current corner
            for(int j = 0; j < corners.Count; j++)
            {
                if (vertical)
                {
                    if(corners[j].x == currentPos.x && Mathf.Abs(corners[j].y - currentPos.y) < currentHighest && corners[j] != currentPos &&
                        !EmptySpace(currentPos, corners[j]))
                    {
                        toBeAdded = corners[j];
                        currentIndex = j;
                        currentHighest = Mathf.Abs(corners[j].y - currentPos.y);
                    }
                }
                else if (!vertical)
                {
                    if(corners[j].y == currentPos.y && Mathf.Abs(corners[j].x - currentPos.x) < currentHighest && corners[j] != currentPos &&
                        !EmptySpace(currentPos, corners[j]))
                    {
                        toBeAdded = corners[j];
                        currentIndex = j;
                        currentHighest = Mathf.Abs(corners[j].x - currentPos.x);
                    }
                }
            }
            currentPos = toBeAdded;      
            corners.RemoveAt(currentIndex);
            //Add current pos to current path
            currentPath.Add(currentPos);

            if (toBeAdded == startingPos)    //If path is complete
            {
                //Add path and reset
                toReturn.Add(currentPath);
                currentIndex = 0;
                if(corners.Count > 0)   //If there is anything left in corners
                {
                    startingPos = corners[currentIndex];
                }
                else
                {
                    break;  //This break isn't technically neccessary if the code is running correctly but it helps with code comprehension
                }
                currentPath = new List<Vector2>();
                currentPos = startingPos;
            }

            vertical = !vertical;
        }

        return toReturn;
    }

    private bool EmptySpace(Vector2 pos1, Vector2 pos2)
    {
        Vector2 direction = (pos2 - pos1).normalized;
        float distance = Vector2.Distance(pos1,pos2);
        for(int i = 0; i < distance; i++)
        {
            Vector2 newPos = pos1 + (direction * i);
            if (tex.GetPixel((int)newPos.x,(int)newPos.y).a == 0)
            {
                return true;
            }
        }
        return false;
    }

    public List<Vector2> GetCoordinatesOfCorners()
    {
        List<Vector2> toReturn = new List<Vector2>();
        //Get all corners
        for(int x = 0; x < tex.width; x++)
        {
            for(int y = 0; y < tex.height; y++)
            {
                //Check to see if pixel is on edge
                if(tex.GetPixel(x,y).a != 0)
                {
                    //This system to find all the corners doesn't work with specific diagonal edges (however, it should not need to be used for that)
                    int numOfEmptys = NumOfEmptyPixelsSurrounding(x,y);
                    if(numOfEmptys == 1 || numOfEmptys == 4 || numOfEmptys == 5)
                    {
                        toReturn.Add(new Vector2(x,y));
                    }
                }
            }
        }
        return toReturn;
    }

    private int NumOfEmptyPixelsSurrounding(int x, int y)
    {
        int toReturn = 0;
        for (int i = -1; i < 2; i++) //i == x   //I can't call them x and y because of the FOR loops in the GetCoordinatesOfCorners function
        {
            for(int j = -1; j < 2; j++) //j == y
            {
                if(tex.GetPixel(x+i,y+j).a == 0 || x+i < 0 || x+i > tex.width -1 || y+j < 0 || y+j > tex.height -1)
                {
                    toReturn += 1;
                }
            }
        }
        return toReturn;
    }
}

[CustomEditor(typeof(ColliderGenerator))]
[CanEditMultipleObjects]
public class ColliderGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ColliderGenerator script = (ColliderGenerator)target;
        if(GUILayout.Button("Generate Collider"))
        {
            script.GenerateCollider();
        }
    }
}
#endif