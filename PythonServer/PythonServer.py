import msvcrt
import socket
from collections import deque

portNumber = 6060

connectedClients = []
messageQueue = deque()

theStocket = socket.socket()
theStocket.bind(('', portNumber))
theStocket.listen()

print('Please start typing.. and hit ESC when you want to exit.')

#while True:
#	conn, addr = theStocket.accept()
#	connectedClients.append(conn)

#conn.sendall('5'.encode("utf-16-le"))



while True:
	theKey = msvcrt.getch()
	keyNumber = ord(theKey)
	if(keyNumber == 27):
		break
	elif(keyNumber > 0 and keyNumber < 128 and keyNumber != 13):
		theChar = chr(keyNumber)
		messageQueue.append(theChar)
		print(theChar, end='')
		sendMessages()