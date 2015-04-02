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
		w.Header().Set("Access-Control-Allow-Origin", "*")
		fmt.Println(r.RemoteAddr, r.Method, r.URL)
		// for k, v := range r.Header {
		// 	fmt.Println(k, v)
		// }
		file, fileheader, err := r.FormFile("file")
		for k, v := range fileheader.Header {
			fmt.Println(k, v)
		}
		defer file.Close()
		out, err := os.Create("C:/tmp/" + fileheader.Filename)
		if err != nil {
			fmt.Fprintf(w, "Unable to create the file for writing. Check your write access privilege")
			return
		}
		defer out.Close()

		// write the content from POST to the file
		_, err = io.Copy(out, file)
		if err != nil {
			fmt.Fprintln(w, err)
			return
		}

		fmt.Println(fileheader.Filename, "has been saved to", out.Name())
		fmt.Fprintf(w, "File uploaded successfully : ")
		fmt.Fprintf(w, fileheader.Filename)
	})
	port := ":27000"
	s := &http.Server{
		Addr: "192.168.1.170" + port,
	}
	fmt.Println("http server at " + s.Addr)
	err := s.ListenAndServe()
	if err != nil {
		fmt.Println(fmt.Sprintf("ListenAndServe: ", err))
	}
}
func main() {
	go server()
	go HTTPImageServer()
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
