# UI_DefaultWithLookupTexture
A simple UI shader that uses a lookup texture.

#Description

A UI shader based on the original Unity UI shader but it takes a lookup texture as an input and display the Graphic with the corresponding color from the lookup texture.
It also contains an editor script that helps creating lookup tables.
It aims at quickly iterating themes in already complex UIs without changing all the graphics properties by hand. Instead create your lookup texture and change your UI element material's shader and voilà.

#How to use

Use the LookupTextureGenerator to generate a lookup texture.
	1 - Open the window "Lookup texture editor"("Window/Lookup texture editor").
	2 - Setup your array of color pairs.
		The color on the left is the color you will fill in the unity component graphic.color.
		The color on the right will be the color used instead.
	3 - Fill the name (shouldn't contains any extension).
	4 - Fill in the path (should end with '\').
		e.g : "C:\Unity Projects\MyProject\Assets\Textures\"
	5 - Press save it.

Create a material using UI_DefaultWithLookupTable as shader.

If you want the UI to be world spaced and drawn on top of meshes, change UI_DefaultWithLookupTable.shader line 67 from "ZTest [unity_GUIZTestMode]" into "ZTest Off".

Enjoy !
