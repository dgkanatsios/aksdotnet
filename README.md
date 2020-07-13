# aksdotnet
Kubernetes deployment samples for AKS and .NET Core

## Images

Repo is [here](https://hub.docker.com/repository/docker/dgkanatsios/aksdotnet)

- docker.io/dgkanatsios:0.1 - normal working image tagged 0.1
- docker.io/dgkanatsios:0.2 - normal working image tagged 0.2
- docker.io/dgkanatsios:0.2-startupfails - app fails to start
- docker.io/dgkanatsios:0.2-healthfails - health probe fails after 10 seconds

## Script

Run `watch -n 0.5 kubectl get pods -o wide --sort-by=.metadata.creationTimestamp`

### Make sure cluster is there and working fine

- `kubectl cluster-info`: get info about AKS cluster
- `kubectl get nodes -o wide`: get info about nodes

### Imperative creation of deployment

- `kubectl create deploy nginx --image=nginx`: create an nginx deployment
- `kubectl expose deploy nginx --port=80 --type=LoadBalancer`: expose it to the outside world
- `kubectl get deploy nginx -o yaml`: see some YAML for the deployment
- `kubectl delete svc nginx && kubectl delete deploy nginx`: cleanup

### Declarative creation of deployment

#### Deployment creation

- `kubectl apply -f deploy.yaml`: this will create version 0.1
- `kubectl get po --show-labels`: observe that all Pods have the same label as the Deployment template
- `kubectl get rs --show-labels`: same for the replica set
- `kubectl describe rs ....`: check the details for the replica set
- `kubectl describe po | grep -C 3 'Controlled By'`: see that the Pods "belong" to the Replica Set

#### Verify that the service is working

- `kubectl get svc`: get the public IP and check IP/hello from a browser to verify that the deployment works
- `kubectl run -it busybox --rm --image=busybox sh` and `wget -q0- aksdotnet-deployment/hello`: see that version 0.1 is returned
- Get the Pod name in the browser, do `kubectl exec -it PodName -- bash` to connect to the Pod. Run `apt update && apt install net-tools tcpdump procps`, then...
    - `ps -aux` to see the running processes (observer that the .NET process has a PID of 1). Try also running `top` for real-time information
    - `netstat -tulpn` to see the open ports from processes
    - `tcpdump port 80` and refresh the browser to see the packets coming in
- Do `kubectl logs` for one of the pods to see logs of incoming requests

#### ReplicaSet reconciliation loop

- `kubectl get po`: get the pod names
- `kubectl delete po ....`: delete a random Pod
- `kubectl get po`: verify that a new Pod was created
- `kubectl describe rs`: check the ReplicaSet events

#### Scaling

- `kubectl scale deployment aksdotnet-deployment --replicas=10`: this will scale the deployment to 10 replicas
- `kubectl describe deploy`: check the events about replica set being scaled
- `kubectl describe rs`: check the events about the Pods being created

#### maxUnavailable and updating with a wrong image

- `kubectl edit deploy aksdotnet-deployment`: update tag to a non-existent one (alternatively you can do `kubectl set image deployment aksdotnet-deployment aksdotnet docker.io/dgkanatsios/aksdotnet:wrong-tag --record` or edit the yaml file and then `kubectl apply -f deploy.yaml --record`)
- `kubectl get rs` and `kubectl rollout status deploy aksdotnet-deployment`: see what happens with maxUnavailable
- `kubectl rollout history deploy aksdotnet-deployment`: see the deployment history
- `kubectl rollout history deploy aksdotnet-deployment --revision=2`: see details about a specific revision
- `kubectl rollout undo deploy aksdotnet-deployment`: undo the last deployment
- `kubectl get deploy aksdotnet-deployment`: see that everything is good again

#### Updating to a correct version

- `kubectl edit deploy aksdotnet-deployment`: update tag to 0.2 - Pod template changes so an upgrade is triggered
- `kubectl rollout status deployment aksdotnet-deployment`: see the status of the deployment
- `echo $?`: get the last exit code (of `kubectl rollout status deploy`) and see that it is not zero
- `kubectl describe deploy aksdotnet-deployment`: see that the version was upgraded to 0.2
- `kubectl rollout history deployment aksdotnet-deployment`: see all deployments
- `kubectl get rs`: see that there is one ReplicaSet per deployment update

#### Updating to a version with an entrypoint that will error

- `kubectl edit deploy aksdotnet-deployment`: change tag to 0.2-startupfails and observe what happens with the pods (`kubectl get po -w`)

#### Updating to a version with a liveness probe that will error

- `kubectl edit deploy aksdotnet-deployment`: change tag to 0.2-healthfails and observe what happens with the pods (`kubectl get po -w`)

## I WANNA DO MOARRRR KUBECTL

For more kubectl fun, check out my [CKAD-exercises](https://github.com/dgkanatsios/ckad-exercises) repo.