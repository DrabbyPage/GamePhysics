// BelcherGamePhysicsDLL.cpp : Defines the exported functions for the DLL.
//

#include "pch.h"
#include "framework.h"
#include "GamePhysicsDLLPlugin.h"


// This is an example of an exported variable
GAMEPHYSICSDLL_API int nGamePhysicsDLL=0;

// This is an example of an exported function.
GAMEPHYSICSDLL_API int fnGamePhysicsDLL(void)
{
    return 0;
}

// This is the constructor of a class that has been exported.
GamePhysicsDLL::GamePhysicsDLL()
{
    return;
}
