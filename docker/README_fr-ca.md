[English Version](README.md)

# Exécution du service d'émission

Pour exécuter le projet, vous devrez ouvrir deux terminaux :

- un terminal dans le répertoire [ngrok](./ngrok) : exécutez le script [start-ngrok.sh](./ngrok/start-ngrok.sh), ceci initialisera un tunnel ngrok qui sera utilisé pour transmettre les demandes à votre agent provenant de votre cellulaire qui est sur Internet.

- un terminal dans le répertoire [docker](./docker) : exécutez la commande `./manage build` pour compiler les pré-requis de l'image qui exécute les services, puis `./manage start`. Ceci démarrera tous les services définis dans [docker-compose.yml](./docker/docker-compose.yml).

Une fois démarrés, les services écouteront sur localhost au port suivant :

- `immunization-api-dev`: http://localhost:5001 - fonctionnant en mode développement avec rechargement à chaud.

- `issuer-api-dev`: http://localhost:5000 - fonctionnant en mode développement avec rechargement à chaud.

- `frontend`: http://localhost:4200 - fonctionnant en mode développement avec rechargement à chaud.

- `agent`: http://localhost:8024 (admin API).

- `keycloak`: http://localhost:8081 - utilisez `admin/admin` pour accéder à la console d'administration.

- `maildev`: http://localhost:8050

- `db` et `wallet` sont des bases de données postgres et écoute respectivement sur les ports `5432` et `5434`.


Vous pouvez toujours exécuter `./manage -h` pour obtenir plus d'informations sur l'utilisation du script, vous référer aux valeurs des variables d'environnement définies et consommées par `docker-compose`.

**Remarque:** lors du démarrage du projet, le fichier `.env` contient la clé privé utilisé par le portefeuille de l'agent est généré [docker](./docker). Ne modifiez pas ce fichier manuellement, car son contenu est utilisé par les services pour faire persister les informations de l'agent et sera traité automatiquement par le script `manage`.