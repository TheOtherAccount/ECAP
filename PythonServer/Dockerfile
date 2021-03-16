FROM python:3

WORKDIR /usr/src/app

RUN pip install keyboard

COPY PythonServer/PythonServer.py .

CMD [ "python", "-u", "./PythonServer.py" ]
