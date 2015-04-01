package main

import (
	"fmt"
	"io"
	"net"
	"net/http"
	"os"
	"time"
)

func check(err error, f func()) {
	if err != nil {
		f()
	}
}

func init() {

}
func client() {
	ip := net.ParseIP("192.168.1.170")
	//ip := net.ParseIP("127.0.0.1")
	addr := &net.TCPAddr{ip, 11000, ""}

	conn, err := net.DialTCP("tcp", nil, addr)
	if err != nil {
		fmt.Println("DialTCP", err)
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
		fmt.Println("read", err.Error())
		return
	}
	fmt.Println(string(buf[0:c]))
}
func server() {
	tcpAddr, _ := net.ResolveTCPAddr("tcp4", ":11000")
	listener, err := net.ListenTCP("tcp", tcpAddr)
	if err != nil {
		fmt.Println(err)
		return
	}
	for {
		conn, err := listener.AcceptTCP()
		if err != nil {
			fmt.Println("tcp listener error")
			continue
		}
		go handleConnection(conn)
	}
}
func HTTPImageServer() {
	http.HandleFunc("/image", func(w http.ResponseWriter, r *http.Request) {
		file, header, err := r.FormFile("file")
		defer file.Close()
		out, err := os.Create("/tmp/uploadedfile")
		if err != nil {
			fmt.Fprintf(w, "Unable to create the file for writing. Check your write access privilege")
			return
		}
		defer out.Close()

		// write the content from POST to the file
		_, err = io.Copy(out, file)
		if err != nil {
			fmt.Fprintln(w, err)
		}

		fmt.Fprintf(w, "File uploaded successfully : ")
		fmt.Fprintf(w, header.Filename)
	})
	if err := http.ListenAndServe(":8080", nil); err != nil {
		fmt.Println("http server err")
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
		buf := make([]byte, 65535)
		_, err := conn.Read(buf)
		if err != nil {
			fmt.Println("handleConnection", err)
			conn.Close()
			return
		}
		//log4go.Info(string(buf))
		fmt.Println(string(buf))
		//fmt.Green().Println(string(buf))
	}
}
