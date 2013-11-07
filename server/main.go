package main

import (
	"fmt"
	"net"
	"time"

	"github.com/bitnick10/goa/log4go"
)

var ip net.IP
var addr *net.TCPAddr

func check(err error, f func()) {
	if err != nil {
		f()
	}
}

func init() {
	ip = net.ParseIP("127.0.0.1")
	addr = &net.TCPAddr{ip, 11000, ""}
}
func client() {
	conn, err := net.DialTCP("tcp", nil, addr)
	if err != nil {
		log4go.Error(err)
		return
	}
	// defer client.Close()
	conn.Write([]byte("hello server!"))
	time.Sleep(time.Second)
	conn.Write([]byte("hello server!"))
	conn.Close()
	return
	buf := make([]byte, 1024)
	c, err := conn.Read(buf)
	if err != nil {
		fmt.Println(err.Error())
		return
	}
	fmt.Println(string(buf[0:c]))
}
func server() {
	listener, err := net.ListenTCP("tcp", addr)
	if err != nil {
		log4go.Error(err)
		return
	}
	for {
		conn, err := listener.AcceptTCP()
		if err != nil {
			log4go.Error(err)
			continue
		}
		go handleConnection(conn)
	}
}
func main() {
	go server()
	go client()
	var input string
	fmt.Scanln(&input)
	fmt.Println("done")
}

func handleConnection(conn *net.TCPConn) {
	// addr := conn.RemoteAddr()
	for {
		buf := make([]byte, 256)
		_, err := conn.Read(buf)
		if err != nil {
			log4go.Error(err)
			conn.Close()
			return
		}
		log4go.Info(string(buf))
	}
}
