#include "foo.h"

foo::foo(int f_new) 
	: f(f_new)
{

}

int foo::Foo(int bar)
{
	return (bar + f);
}