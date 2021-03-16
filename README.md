cd pythonserver
docker build -t pythonserver .
docker run -it --rm --name=pythonserver --network-alias=ecap_server --network=ecap_network pythonserver


cd dotnetclient
docker build -t dotnetclient .
docker run -d --rm --network=ecap_network --name=dotnetclient dotnetclient
cls
docker attach dotnetclient