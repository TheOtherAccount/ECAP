import threading, os

from Getch import Getch
from ECAPServer import ECAPServer

ecapServer = ECAPServer(6060)

connectionsThread = threading.Thread(target = ecapServer.acceptConnections)
connectionsThread.start()

print('ECAP Python Server Started.\r\n')
print('Please start typing or hit ESC to exit.')

getch = Getch()

while True:
	keyCode = ord(getch())
	if(keyCode == 27):
		os._exit(1)
	else:
		keyChar = chr(keyCode)
		if(keyChar.isprintable()):
			print(keyChar, end='')
			ecapServer.sendMessage(keyChar)