using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using System.Runtime.InteropServices;

public class MyUnityPlugin
{
    [DllImport("GamePhysicsDLLPlugin")]
    public static extern int Initfoo(int f_new = 0);
    [DllImport("GamePhysicsDLLPlugin")]
    public static extern int Dofoo(int bar = 0);
    [DllImport("GamePhysicsDLLPlugin")]
    public static extern int Termfoo();
}
