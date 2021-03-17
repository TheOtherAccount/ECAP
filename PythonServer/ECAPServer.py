import socket

from collections import deque

class ECAPServer:
	"""TCP/IP Server that supports multiple clients."""

	def __init__(self, port):
		self.__portNumber = port
		self.__isSendingMessages = False
		self.__connectedClients = []
		self.__messageQueue = deque()

		self.__theStocket = socket.socket()
		self.__theStocket.bind(('', self.__portNumber))
		self.__theStocket.listen()

	def acceptConnections(self):
		"""Starts to accept client connections and store them in a list."""

		while True:
			conn, addr = self.__theStocket.accept()
			self.__connectedClients.append(conn)

	def sendMessage(self, theMessage):
		"""If there is any connected clients.. it queues the message and notifies the object to send it now or later."""

		if(len(self.__connectedClients) > 0):
			self.__messageQueue.append(theMessage)

			if(self.__isSendingMessages == False):
				self.__sendMessages()

	def __sendMessages(self):
		"""Keeps sending the messages in the queue until it is empty."""
		
		self.__isSendingMessages = True

		message = self.__getNextMessage()

		while(message != None):
			for client in self.__connectedClients:
				try:
					client.sendall(message.encode("ascii"))
				except:
					pass
			message = self.__getNextMessage()
		self.__isSendingMessages = False

	def __getNextMessage(self):
		"""Returns the next message in the queue if any."""

		if(len(self.__messageQueue) > 0):
			return self.__messageQueue.popleft()