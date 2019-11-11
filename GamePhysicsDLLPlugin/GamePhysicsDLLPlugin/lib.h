#ifndef LIB_H
#define LIB_H

#ifdef GANEPHYSICSDLLPLUGIN_EXPORT
#define GAMEPHYSICSDLLPLUGIN_SYMBOL __declspec(dllexport)
#else
#ifdef GANEPHYSICSDLLPLUGIN_IMPORT
#define GAMEPHYSICSDLLPLUGIN_SYMBOL __declspec(dllimport)
#else
#define GAMEPHYSICSDLLPLUGIN_SYMBOL
#endif
#endif

#endif