package main

import (
	"bufio"
	"context"
	"fmt"
	"joshhills/email-grpc/tutorialpb"
	"os"

	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
)

func main() {
	reader := bufio.NewReader(os.Stdin)

	fmt.Print("Enter email address: ")
	address, _ := reader.ReadString('\n')

	fmt.Print("Enter subject: ")
	subject, _ := reader.ReadString('\n')

	fmt.Print("Enter message content: ")
	message, _ := reader.ReadString('\n')

	fmt.Print(address, subject, message)

	opts := []grpc.DialOption{grpc.WithTransportCredentials(insecure.NewCredentials())}

	conn, err := grpc.Dial("localhost:5158", opts...)

	if err != nil {
		panic(err)
	}

	if err == nil && conn != nil {
		defer conn.Close()
	}

	client := tutorialpb.NewEmailClient(conn)

	emailReq := tutorialpb.EmailRequest{Address: address, Subject: subject, Content: message}
	res, err := client.SendEmail(context.Background(), &emailReq)

	if err != nil {
		panic(err)
	}

	fmt.Println(res)
}
