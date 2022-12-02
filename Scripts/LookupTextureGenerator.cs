/*
 * MIT License
 * 
 * Copyright (c) 2022 Loïck Rivemale
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class LookupTextureGenerator : EditorWindow
{
    private static int _ColorAmount = 4;

    private string _lookupTextureName = "texture name";
    private string _texturePath = "";

    public Color[] _originalColors = new Color[_ColorAmount];
    public Color[] _equivalentColors =new Color[_ColorAmount];

    private bool folded = false;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Lookup texture editor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        LookupTextureGenerator window = (LookupTextureGenerator)EditorWindow.GetWindow(typeof(LookupTextureGenerator));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        _lookupTextureName = EditorGUILayout.TextField(new GUIContent("Lookup texture name", "texture name is [this].png"), _lookupTextureName);
        _texturePath = EditorGUILayout.TextField(new GUIContent("Lookup texture save path", "paste the whole path, must end with '/'"), _texturePath);

        folded = EditorGUILayout.Foldout(folded, "colors");

        if (folded)
        {
            for (int i = 0; i < _ColorAmount; i++)
            {
                //EditorGUILayout.LabelField(i.ToString());
                EditorGUILayout.BeginHorizontal();
                _originalColors[i] = EditorGUILayout.ColorField(_originalColors[i]);
                _equivalentColors[i] = EditorGUILayout.ColorField(_equivalentColors[i]);
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+"))
        {
            AddNewPair();
        }

        if (GUILayout.Button("-"))
        {
            RemovePair();
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Generate"))
        {
            GenerateTexture();
        }

        if (GUILayout.Button("Clear all colors"))
        {
            for (int i = 0; i < _ColorAmount; i++)
            {
                _originalColors[i] = Color.white;
                _equivalentColors[i] = Color.white;
            }
        }
    }

    private void AddNewPair()
    {
        List<Color> tmp = new List<Color>(_originalColors);

        tmp.Add(Color.white);
        _originalColors = tmp.ToArray();
        tmp = null;

        tmp = new List<Color>(_equivalentColors);
        tmp.Add(Color.white);
        _equivalentColors = tmp.ToArray();

        _ColorAmount++;
    }

    private void RemovePair()
    {
        List<Color> tmp = new List<Color>(_originalColors);

        tmp.RemoveAt(tmp.Count - 1);
        _originalColors = tmp.ToArray();
        tmp.Clear();

        tmp = new List<Color>(_equivalentColors);
        tmp.RemoveAt(tmp.Count - 1);
        _equivalentColors = tmp.ToArray();

        _ColorAmount--;

    }

    private void GenerateTexture()
    {
        Texture2D lookupTable = new Texture2D(_ColorAmount * 2, 1);

        for (int i = 0; i < _ColorAmount * 2; i += 2)
        {
            lookupTable.SetPixel(i, 0, _originalColors[i / 2]);
            lookupTable.SetPixel(i + 1, 0, _equivalentColors[i / 2]);
        }

        lookupTable.Apply();
        
        byte[] bytes = ImageConversion.EncodeArrayToPNG(lookupTable.GetRawTextureData(), lookupTable.graphicsFormat, (uint)_ColorAmount * 2, (uint)1);
        DestroyImmediate(lookupTable);
        File.WriteAllBytes(String.Join(string.Empty, new string[]
        {
            _texturePath,
            _lookupTextureName,
            ".png"
        }), bytes);

        Debug.Log("<color=green>" + String.Join(string.Empty, new string[]
        {
            "file saved at : [",
            _texturePath,
            _lookupTextureName,
            ".png] successfully !"
        }) + "</color>");

    }
}
