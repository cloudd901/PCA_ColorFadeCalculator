# PCA_ColorFadeCalculator

Custom fading generator.

<b>Settings</b>

- Variables
  - Steps = Contains all colors between two colors. (read – List<Color>)
<br>
  <b>Functions</b>
  
    ColorFadeCalculator()
    ColorFadeCalculator(Color fromColor, Color toColor)
    ColorFadeCalculator(Color fromColor, Color toColor, int targetSteps = 60)
    ColorFadeCalculator(Color fromColor, Color toColor, int targetSteps = 60, bool transparentFading = false)
    ColorFadeCalculator(Color fromColor, Color toColor, int targetSteps = 60, int accuracy = 1)
    GetMidColor(Color color1, Color color2)
  
- ColorFadeCalculator
  - fromColor = First Color. (Color)
  - toColor = Second Color. (Color)
  - targetSteps = How many steps between fromColor and toColor. (int)
    - This will determine the final size of Steps List variable.
  - accuracy = Skip processing color steps if delay is an issue. (bool)
    - 1 will process every color between fromColor and toColor.
    - 2 will process every other color between fromColor and toColor.
    - etc.
  - transparentFading = Include Alpha channel when fading. (int)

- GetMidColor
  - color1 = First Color. (Color)
  - color2 = Second Color. (Color)
<br>
Simple Example:
  
    label.Backcolor = Color.Red;
    ColorFadeCalculator cfc = ColorFadeCalculator(Color.Red, Color.Blue);
    for (int i = 0; i < cfc.Steps.Count; i++)
    {
        label.Backcolor = cfc.Steps[i];
    }
