docker build -t pythonserver . 
docker run -it --rm --name=pythonserver --network-alias=ecap_host --network=ecap_network pythonserver



docker build -t dotnetclient .
docker run -d --rm --network=ecap_network --name=dotnetclient dotnetclient
docker attach dotnetclient