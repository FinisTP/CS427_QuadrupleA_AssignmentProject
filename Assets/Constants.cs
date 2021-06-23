using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const int NUMBER_OF_PLAYERS = 1;
    public const int MAX_FRAME_RATE = 60;

    public const int ASPECT_RATIO_WIDTH = 16;
    public const int ASPECT_RATIO_HEIGHT = 9;

    public const int PIXEL_PER_TILE = 16;
}

/* 
 * [NAMING CONVENTION - REF: JETBRAINS RESHARPER]
 * public class UpperCaseClassNames {}
 * 
 * void UpperCaseMethodNames()
 * 
 * public int camelCasePublicVariable
 * float camelCaseLocalVariable
 * private/protected string _underscoredCamelCasePrivateVariable
 * 
 * public interface IInterfaceStartWithI {}
 * public const int ALL_CAPITALS_FOR_CONST
 * 
 * public enum Direction {UP, DOWN, LEFT, RIGHT}
 * 
 * bool underscore_for_debug_functions_or_variables
 */
