# Checkout Challenge - Payment Gateway

This application was created as a minimal solution for the challenge proposed by the company checkout.com

Throughout the interview process, I spoke with different managers/engineers that have mentioned about the heavy use of microservices inside the company. So I have architected this solution to be as closest as possible to a full fledged microservice application.

---

This solution contains:
- 3 Services
    - Identity Service
    - Bank Service
    - Payment Service

- 2 forms of deploying
    - A .bat file to load the three services mentioned above
    - A group of kubernetes files to register all services based on my docker hub.

- An HTTP aggregator
    - A blazor project that can communicates with all services and forward any JWT that are authorized through a register/login page.
    - This one was created to simulate a possible web application consuming the Payment Gateway.

---

## Features

### Authentication and authrorization

The identity service is responsible for issuing JWTs. Those tokens have a lifetime of one hour.
A good approach would be to have a refresh token strategy, to avoid that malicious users capture valid JWTs and use the application as the user for an entire hour and also multiple logins.

### Payment gateway

The payment gateway depends on the bank service as mentioned above.
The most basic scenarios are
- A merchant,  with a valid jwt, register a transaction to the API.
    - If all fields are valid, the gateway process that and send back a message informing about a successful or unsucessful message.
    - If the fields are invalid, the gateway will reject the transaction and let the user know which fields are incorrect.

- A merchant can view only his own transactions (based on the email of login, which is validated through the JWT).

### Bank

The bank service is a simple service that should be talking with a valid credit card operator or bank API.
- Currently it only receives a transaction and randomly decides if that should be approved or not.

### Web Aggregator

The web aggregator is a good feature to simulate an application that would consume the services described above
- The web aggregator also counts with a [Swagger](images\Swagger.png) endpoint, in order to better visualize the API and it's endpoints.

---

## Requirements

- .NET Core SDK 5 - [read more](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
- Docker (optional) - [read more](https://docs.docker.com/desktop/windows/install/)
- Kubernetes (optional) - [read more](https://kubernetes.io/docs/tasks/tools/install-kubectl-windows/)
- Insomnia (optional) - [read more](https://insomnia.rest/download)
    - Similar to Postman for endpoints testing

---

## Setup

There are two options to startup the whole application

### #1 - .BAT File

You can navigate to the file through [here](deploy/windows/).
In order to run it, just double click the bat file.

It will open 3 terminals, one for each service.

- When you want to close them, just press `CTRL+C` in all terminals and the application will shutdown successfully.

This option will load the application as 'Development', so the services will be available on:
- Bank Service -> http://localhost:6000/api/transactions
- Identity Service -> http://localhost:5004/api/auth
- Payment Service -> http://localhost:5000/api/payment


### #2 - K8S (Kubernetes)

You can navigate to the k8s [folder](deploy\kubernetes) and run:

```
kubectl apply -f .
```

This option will load the application as 'Production'. I have included a nginx k8s file, that is acting as a reverse proxy, so the endpoints are the following:
- Bank Service -> http://acme.com/api/transactions
- Identity Service -> http://acme.com/api/auth
- Payment Service -> http://acme.com/api/payment

All up-to-date docker images are hosted on my personal DockerHub (kpedromonico).
- kpedromonico/bankservice
- kpedromonico/identityservice
- kpedromonico/paymentservice

### Web Aggregator

If you want to load the HttpAggregator.
Open a terminal instance, navigate [here](src/PaymentGateway.HttpAggregator) and run:

```
dotnet run
```

It should load the following [window](images/webaggregator-login.png).

### Unit Tests

To run the unit tests navigate [here](src\Services\Payment\Payment.UnitTests) and run the following command in your terminal of choice.

```
dotnet test
```

---

# Points of improvement

## Authentication

Currently no password hashing mechanism is in place.

There could be email confirmation once the user did register. That would another layer of protection.

### Refresh JWT Tokens

In order to avoid multiple logins and a long JWT lifetime a refresh token approach could be used. [More details here](https://auth0.com/blog/refresh-tokens-what-are-they-and-when-to-use-them/#Refresh-Token-Rotation)

## Database

For simplicity / lower resource consumption purposes, I have decided to use an InMemory Database for all services.

We could have use multiple databases due to decoupling between services.
For example:
- The identity service could be connected to a `Microsoft SQL Server`/`Azure SQL DB`, due to the tight relationship between entities present on RDBMS. (Since identity system doesn't change as often as other application modules)
- The Payment service could make use of a Document Database like `MongoDB` or `CosmosDB`, since it can be easily scalable and due to the nature of the data present on this service not being tighly coupled and providing versioning of the documents.

## Logging

I could have added a logging functionality and even a log store like the ELK stack for log processing.

Or maybe some cloud services like `AppInsights`, at least to track the high level logs.

## Currency be coming from a service

A currency service could have been added where all the currencies would have been maintained, limiting the user choices.

## Services communication & Resilience

Currently, the communication between the bank and payments service occurs through HTTP.

A gRPC could be have used instead due to its easier scalability and performance. In case of an unavailability of the gRPC channels, then HTTP could be triggered and used till everything is restored.

Also there is no circuit breaker in case of the bank service is down, which means that all transactions couldn't be completed.

In order to keep the system flowing. I thought in sending bank the transactions with the property 'Approved' marked as null, which means that the transactions were not approved nor rejected. A background service could easily capture those transaction in the database and reprocess them soon as the bank services are back available.

## Testing

I have added only tests to the domain and service layers in the Payment Service. A production application should have a test coverage higher than this.

## Metrics Calculation

Having some metrics like test code coverage or performance ones are vital for the future improvement in an application.


## API Versioning

It is a good practice that microservices should keep versions of its payloads/dtos in order to support backwards compatility and avoid down time of different services that depend of them (whenever a new deploy is done).