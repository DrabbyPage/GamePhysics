


// The following ifdef block is the standard way of creating macros which make exporting
// from a DLL simpler. All files within this DLL are compiled with the BELCHERGAMEPHYSICSDLL_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see
// BELCHERGAMEPHYSICSDLL_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.




#ifndef MYUNITYPLUGIN_H
#define MYUNITYPLUGIN_H

#include "lib.h"

#ifdef  __cplusplus
extern "C"
{
#else

#endif // ! _cplusplus

GAMEPHYSICSDLLPLUGIN_SYMBOL int InitFoo(int f_new);
GAMEPHYSICSDLLPLUGIN_SYMBOL int DoFoo(int bar);
GAMEPHYSICSDLLPLUGIN_SYMBOL int TermFoo();

#ifdef  __cplusplus
}
#else

#endif // ! _cplusplus


#endif