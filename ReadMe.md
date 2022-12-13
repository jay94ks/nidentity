# N-Identity
Let's create a convenient custom CA. `N-Identity` allows you to create your own Root CA with simple `genesis.json` file. And even more, this has a browser app that shows you all certificates on your own Root CA and it generates sub CA or leaf certificates by just clicks. and supports OCSP responder endpoint, automatically generated CRL endpoint and CER distribution point.

## Example Server
### Genesis Option file.
```
{
	"required_keys": [
		{
			"cert_type": 0, 
			"key_algorithm": "rsa-2048",
			"key_purposes": 7,
			"expiration_hrs": 876000,
			"subject": "CN=NIdentity Root CA",
			"issuer": "CN=NIdentity Root CA",
			"with_ocsp": true,
			"with_crl_dists": false,
			"with_ca_issuers": false
		}
	]
}
```
This `genesis.json` file creates a certificate and private key with a subject of `NIdentity Root CA` and outputs a pfx file with random hex characters to the data folder. Only the certificates created by this file will work as Root CA, and after that, depending on the server execution option, generating Root CA certificates will be disallowed. (`cert_type: 0` - Root, `cert_type: 1` - Immediate, `cert_type: 2` - Leaf)

### docker-compose.yml
```
version: '3.4'
services:
  nidentity:
    image: 'jay94ks/nidentity:latest'
    container_name: 'nidentity'
    restart: always
    environment:
      MYSQL_DB: 'nidentity'
      MYSQL_USER: 'nidentity'
      MYSQL_PASS: 'nidentity1!'
      MYSQL_HOST: 'nidentity-mysql'
      MYSQL_PORT: 3306
      SSL_CERT_FILE: 'data/https.pfx'
      CRL_TERM: 30
      CER_TERM: 30
      MAX_CACHE_KEYS: 1024
      RUN_AS_SUPER: 0
      GENESIS_CMDS: 'data/genesis.json'
      HTTP_BASE_URL: 'http://127.0.0.1:7000/'
    volumes:
      - './data:/app/data'
    ports:
      - '7000:7000'
      - '7001:7001'
    networks:
      - 'nidentity-net'
      
  nidentity-mysql:
    image: 'mysql:latest'
    container_name: 'nidentity-mysql'
    restart: always
    environment:
      MYSQL_DATABASE: 'nidentity'
      MYSQL_USER: 'nidentity'
      MYSQL_PASSWORD: 'nidentity1!'
      MYSQL_ROOT_PASSWORD: 'nidentity1!'
    volumes:
      - './db:/var/lib/mysql'
    ports:
      - '3359:3306'
    command:
      - '--character-set-client-handshake=FALSE'
      - '--character-set-server=utf8mb4'
      - '--collation-server=utf8mb4_unicode_ci'
    networks:
      - 'nidentity-net'

networks:
  nidentity-net:
    driver: overlay
    attachable: true

```

## Limitations
### Key Algorithm supports
Currently, `N-Identity` supports only `RSA` algorithm and its key length should be one of `2048` or `4096`. due to I couldn't find how to convert `bouncycastle` library generated `ECDSA` parameters to `.NET core`s cryptography APIs. (`key_algorithm: rsa-2048` or `key_algorithm: rsa-4096`)

Even if the Eliptical private key 'D' and the public key 'X' and 'Y' parameters are calculated in the normal way, there is a problem that '.NET core' does not recognize, but I am not good at cryptography, so I am not able to solve it. If anyone finds a way to fix this, please let me know.

### No externally generated CA certificates
The design of this software itself is able to recognize external CA certificates, and makes it possible to create sub-certificates based on those certificates. but `importer` has not been implemented due to lack of free time, and plans are only.

## Association APIs
`NIdentity` implements a JSON-based management protocol that separates commands and execution results. The protocol is fairly simple, easy to use, and has a uniform response structure so parsing isn't too difficult.

```
{
    "type": "command"
}
```
And, the command written in this form produces the following execution result.

```
{
    "success": false,
    "reason": "the input JSON is not valid command",
    "reason_kind": "Argument"
}
```

### Security notion.
All `command` messages sent by protocols that do not operate based on SSL are divided into sensitive and general. `Sensitive` commands cannot be executed without SSL and a client-side certificate, the certificate specified must have appropriate permissions and private key. That is, it must not be a Leaf certificate. 

### Connector Library (NIdentity.Connector)
This library provides just wrapper that pre-written codes to execute command through `HTTP`, `HTTPS` and `WebSocket`. (See below)

```
1. .NET CLI: dotnet add package NIdentity.Connector
2. Package Manager: NuGet\Install-Package NIdentity.Connector
3. Package Reference: <PackageReference Include="NIdentity.Connector" Version="1.0.0" />
```

### HTTP, HTTPS, WebSocket
Basically, `NIdentity` provides a command processing endpoint mapped to the `api/infra/live` path. No other complex procedures are required, and no need to memorize different routes. Just disable server side certificate verification (HTTPS, WSS), set CA certificate with private key as SSL client certificate and send JSON formatted command.

And, non-sensitive commands can be used without an SSL client certificate. Finally, running sensitive commands is prohibited in a Cleartext HTTP environment rather than HTTPS.
