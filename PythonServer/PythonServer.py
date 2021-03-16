from Getch import Getch

from collections import deque
import threading
import socket

portNumber = 6060

isSendingMessages = False
connectedClients = []

global failedClients
failedClients = []

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

def removeFailedClients():
	global failedClients

	for failedClient in failedClients:
		connectedClients.remove(failedClient)
	failedClients = [];

def sendMessages():
	isSendingMessages = True

	message = getNextMessage()

	while(message != None):
		for client in connectedClients:
			try:
				client.sendall(message.encode("ascii"))
			except:
				failedClients.append(client)
		message = getNextMessage()
		removeFailedClients()

	isSendingMessages = False

getch = Getch()

while True:
	theChar = chr(ord(getch()))
	if(theChar.isprintable()):
		print(theChar, end='')
		if(len(connectedClients) > 0):
			messageQueue.append(theChar)	
			if(isSendingMessages == False):
				sendMessages()