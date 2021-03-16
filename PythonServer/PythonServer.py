from Getch import Getch

from collections import deque
import threading, os, socket

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

print('ECAP Python Server Started.\r\n')
print('Please start typing or hit ESC to exit.')

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
			try:
				client.sendall(message.encode("ascii"))
			except:
				pass
		message = getNextMessage()

	isSendingMessages = False

getch = Getch()

while True:
	keyCode = ord(getch())
	keyChar = chr(keyCode)
	if(keyChar.isprintable()):
		print(keyChar, end='')
		if(len(connectedClients) > 0):
			messageQueue.append(keyChar)	
			if(isSendingMessages == False):
				sendMessages()
	elif(keyCode == 27):
		os._exit(1)