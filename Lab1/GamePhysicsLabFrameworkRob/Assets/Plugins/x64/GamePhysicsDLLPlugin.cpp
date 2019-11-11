#include "GamePhysicsDLLPlugin.h"

#include "foo.h"

foo* inst = 0;

int Initfoo(int f_new)
{
	if (!inst)
	{
		inst = new foo(f_new);
		return 1;
	}
	return 0;
}
int Dofoo(int bar)
{
	if (inst)
	{
		int result = inst->Foo(bar);
		return result;

	}
	return 0;
}
int Termfoo()
{
	if (inst)
	{
		delete inst;
		inst = 0;
		return 1;
	}
	return 0;
}