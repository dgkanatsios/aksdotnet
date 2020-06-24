NS=docker.io/dgkanatsios
VERSION=0.2-startupfails
IMAGE_NAME=aksdotnet

build:
	docker build -t $(NS)/$(IMAGE_NAME):$(VERSION) .

push:
	docker push $(NS)/$(IMAGE_NAME):$(VERSION)