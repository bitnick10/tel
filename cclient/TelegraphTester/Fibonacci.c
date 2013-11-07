#include "Fibonacci.h"
#include <Telegraph.h>
int Fibonacci(int n)
{
    int i;
    int prev = 1;
    int prevprev = 1;
    int temp;
    for(i = 2; i < n; i++)
    {
		temp=prev;
		prev=prev+prevprev;
		prevprev=temp;
    }	
	//TELE_DEBUG_INIT();
	TELE_DEBUG_PRINTF0("in Fibonacci!\n");
	//TELE_DEBUG_DONE();
	return prev;
}