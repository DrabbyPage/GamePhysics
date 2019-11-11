#ifndef GAMEPHYSICSDLLPLUGIN_H
#define GAMEPHYSICSDLLPLUGIN_H

#include "lib.h"

#ifdef __cplusplus
extern "C"
{
#else // !__cplusplus
#endif


GAMEPHYSICSDLLPLUGIN_SYMBOL int Initfoo(int f_new);
GAMEPHYSICSDLLPLUGIN_SYMBOL int Dofoo(int bar);
GAMEPHYSICSDLLPLUGIN_SYMBOL int Termfoo();

#ifdef __cplusplus
}
#else // !__cplusplus
#endif

#endif