#include "Fibonacci.h"
#include <Telegraph.h>
void TestStopwatch()
{
	int i;
	double d=0.0;
	TELE_DEBUG_INIT();
	TELE_DEBUG_STOPWATCH_START();
	for(i=0;i<10000;i++)
	{
		d+=Fibonacci(i);
	}
	TELE_DEBUG_STOPWATCH_STOP("10,000 times = %lld\n");
	TELE_DEBUG_DONE();
}