[![img](https://img.shields.io/badge/Lifecycle-Experimental-339999)](https://github.com/bcgov/repomountie/blob/master/doc/lifecycle-badges.md)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

---
[Version Française](README_fr-ca.md)

# DTS Verifiable Credential Issuer Service

Digital Trust Verifiable Credential Issuer Service

## Repository Structure
- `immunization-api`: contains the .Net Core 5.0 back-end code that defines the business logic for patient immunization information.

- `issuer-api`: contains the .Net Core 3.1 back-end code that defines the business logic for the DTS VC issuer.

- `angular-frontend`: a Angular frontend that provides users with a user interface to request Verifiable Credentials.

- `docker`: configurations and script to run the services in Docker on localhost.

- `ngrok`: configurations tp open an ngrok tunnel to some of the services, required in order to expose the agent instance to the internet.

## Pre-Requisites

Please ensure the following pre-requisites are met before attempting to run the project:

- Docker

- ngrok

- jq

## Getting Started

Please refer to the readme in the [docker](./docker/README.md) folder.
