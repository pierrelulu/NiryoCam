void setup() {
  Serial.begin(9600); // Doit correspondre au baud rate utilisé dans le programme C#
}

void loop() {
  if (Serial.available() > 0) {
    int receivedValue = Serial.parseInt(); // Lire l'entier envoyé
    int response;

    // Vérifier si la valeur est entre 1 et 10
    if (receivedValue >= 1 && receivedValue <= 10) {
      response = 1; // Dans la plage
    } else {
      response = 0; // Hors de la plage
    }

    // Envoyer la réponse au programme C#
    Serial.println(response);
  }
}
