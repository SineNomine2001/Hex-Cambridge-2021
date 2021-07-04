import UnityEngine as unity

objects = unity.Object.FindObjectsOfType(unity.GameObject)
for go in objects:
    unity.Debug.Log(go.name)