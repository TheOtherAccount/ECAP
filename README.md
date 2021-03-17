# ECAP
Hi there,

The following shot shows the server and multiple clients running:


To run it on your environment:

1. Open a CMD and clone the repository: 
```sh
git clone https://github.com/TheOtherAccount/ECAP.git
```

2. Create a network: 
```sh
docker network create ecap_network
```

3. Build the server and run it on current terminal: 
```sh
docker build -t pythonserver ECAP\PythonServer\
docker run -it --rm --name=pythonserver --network-alias=ecap_server --network=ecap_network pythonserver
```

Now you have the server running but no clients yet; To build an image for the client app, please use the following:

4. Open a new CMD and build an image for the client app:
```sh
docker build -t dotnetclient ECAP\DotNetClient\
```

Now you can run as many client apps as you want by using the following command:

5. For each client app you want to run.. open a new CMD and then:
```sh
docker run -it --rm --network=ecap_network dotnetclient
```

Now when you go to the server and hit any printable key you should see it on all the clients running.