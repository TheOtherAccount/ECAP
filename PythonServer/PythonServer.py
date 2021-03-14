import msvcrt
import socket
from collections import deque
import threading
import sys

portNumber = 6060

isSendingMessages = False
connectedClients = []
messageQueue = deque()

theStocket = socket.socket()
theStocket.bind(('', portNumber))
theStocket.listen()

def acceptConnections():
	while True:
		conn, addr = theStocket.accept()
		connectedClients.append(conn)

print('Please start typing..')

connectionsThread = threading.Thread(target=acceptConnections)
connectionsThread.start()

def getNextMessage():
	if(len(messageQueue) > 0):
		return messageQueue.popleft()

def sendMessages():
	isSendingMessages = True

	message = getNextMessage()

	while(message != None):
		for client in connectedClients:
			client.sendall(message.encode("ascii"))
		message = getNextMessage()

	isSendingMessages = False


while True:
	theChar = chr(ord(msvcrt.getch()))
	if(theChar.isalnum()):
		print(theChar, end='')
		if(len(connectedClients) > 0):
			messageQueue.append(theChar)	
			if(isSendingMessages == False):
				sendMessages()
