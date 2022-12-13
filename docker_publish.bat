
docker build -f ./Dockerfile_X509 -t jay94ks/nidentity:v1.0 .
docker push jay94ks/nidentity:v1.0

docker tag jay94ks/nidentity:v1.0 jay94ks/nidentity:latest
docker push jay94ks/nidentity:latest

pause