# ECAP
Hi there,

The following shot shows the server and multiple clients running:


To run it on your environment:

1. Clone the repository: 
`git clone https://github.com/TheOtherAccount/ECAP.git`
`cd ECAP`

cd pythonserver
docker build -t pythonserver .
docker run -it --rm --name=pythonserver --network-alias=ecap_server --network=ecap_network pythonserver


cd dotnetclient
docker build -t dotnetclient .
docker run -d --rm --network=ecap_network --name=dotnetclient dotnetclient
cls
docker attach dotnetclient