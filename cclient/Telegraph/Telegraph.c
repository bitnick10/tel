#include "Telegraph.h"

int TELE_SendBuffer(TELE_MARK mark, const void *buf, int buflen);

void TELE_Stopwatch_Start()
{
    LARGE_INTEGER li;
    QueryPerformanceCounter(&li);
    tele_handle->Stopwatch.timeStart = li.QuadPart;
}
LONGLONG TELE_Stopwatch_Stop()
{
    LARGE_INTEGER li;
    QueryPerformanceCounter(&li);
    return li.QuadPart - tele_handle->Stopwatch.timeStart ;
}

void TELE_Init( )
{
    SOCKADDR_IN servAddr;
    WSADATA wsaData;
    if(WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
    {
        exit(0);
    }
    tele_handle = (TELE_HANDLE)malloc(sizeof(TELE_STRUCT));
    tele_handle->buf = (char*) malloc(TELE_SEND_BUFFER_SIZE);
    tele_handle->socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
    tele_handle->Text.pText = (char*) malloc(1024);
    if(tele_handle->socket == INVALID_SOCKET)
    {
        printf(" Failed socket() \n");
        return ;
    }
    servAddr.sin_family = AF_INET;
    servAddr.sin_port = htons(11000);
    servAddr.sin_addr.S_un.S_addr = inet_addr("127.0.0.1");
    if(connect(tele_handle->socket, (PSOCKADDR)&servAddr, sizeof(servAddr)) == -1)
    {
        printf(" Failed connect() \n");
        system("pause");
        return ;
    }
}
void TELE_Done()
{
    free(tele_handle->buf);
    tele_handle->buf = NULL;
    closesocket(tele_handle->socket );
    free(tele_handle);
    tele_handle = NULL;
    WSACleanup();
}

void TELE_Draw_Clear()
{
    TELE_SendBuffer(TELE_DRAW_CLEAR, NULL, 0);
}
void TELE_Draw_Add(double x, double y, int color)
{
    struct
    {
        double x, y;
        int color;
    } data;
    data.x = x;
    data.y = y;
    data.color = color;
    TELE_SendBuffer(TELE_DRAW_ADD, &data, sizeof(data));
}
void TELE_Draw_Display()
{
    TELE_SendBuffer(TELE_DRAW_DISPLAY, NULL, 0);
}

void TELE_Text_Display(const char* sz)
{
    TELE_SendBuffer(TELE_TEXT_NORMAL, sz, strlen(sz));
}
int TELE_SendBuffer(TELE_MARK mark, const void *buf, int buflen)
{
    int iResult;
    int len = buflen + 8 ;
    TELE_SEND_STRUCT ss;
    ss.size = sizeof(mark) + buflen;
    ss.mark = mark;
    memcpy( tele_handle->buf , &ss, 8);
    memcpy( tele_handle->buf + 8, buf, buflen);
    iResult = send(tele_handle->socket, tele_handle->buf, len , NULL);
    if(iResult == SOCKET_ERROR  )
    {
        printf("send error\n");
        getchar();
    }
    return 0;
}