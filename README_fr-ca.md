[![img](https://img.shields.io/badge/Lifecycle-Experimental-339999)](https://github.com/bcgov/repomountie/blob/master/doc/lifecycle-badges.md)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

---
[English Version](README.md)

# Service d'émission d'attestations vérifiables

## Structure du dépôt de code

- `immunization-api`: contient le code d'arrière boutique en ".Net Core 5.0" qui définit la logique de gestion des informations relatives à la vaccination des patients.

- `issuer-api`: contient le code d'arrière boutique en ".Net Core 3.1" qui définit la logique affaire de l'émetteur d'attestation vérifiable.

- `angular-frontend`: contient le code frontal en "Angular" qui fournit aux utilisateurs une interface pour demander des attestations d'identité vérifiables.

- `docker`: les configurations et le script pour exécuter les services en conteneur sur votre environnement local.

- `ngrok`: configurations pour ouvrir un tunnel "ngrok" vers certains des services, nécessaires pour exposer l'instance local de l'agent à l'internet.

## Pré-requis

Veuillez vous assurer que les conditions préalables suivantes sont remplies avant de tenter d'exécuter le projet :

- Docker

- ngrok

- jq

## Pour commencer

Veuillez vous référer au fichier readme dans le répertoire [docker](./docker/README.md).
petit test
