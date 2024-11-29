#define PIN_START 12
#define PIN_CAPTURE 13

#define LED_START 8
#define LED_CAPTURE 7

enum Infos
{
    NO_INFO = 0x00,
    READY_START = 0x01,
    READY_CAPTURE = 0x02,
    GO_START = 0x03,
    GO_CAPTURE = 0x04,

    NB_INFOS
};

enum States
{
    IDLING = 0,
    WAITING_START,
    MOVING,
    WAITING_CAP,

    NB_STATES
};

States current_state;

void setup() {
    Serial.begin(9600); // Doit correspondre au baud rate utilisé dans le programme C#

    pinMode(PIN_START, OUTPUT);
    pinMode(PIN_CAPTURE, OUTPUT);

    pinMode(LED_START, INPUT);
    pinMode(LED_CAPTURE, INPUT);

    digitalWrite(PIN_START, LOW); // Initialisation à l'état bas
    digitalWrite(PIN_CAPTURE, LOW);

    current_state = IDLING;
}

void loop() {  
    if (Serial.available() > 0) {
        byte receivedInfo = Serial.read(); // Lire les données du port série
        
        switch (receivedInfo) {
            case GO_START:
                if(current_state == WAITING_START){
                  digitalWrite(PIN_START, HIGH); // Activer PIN_START
                  current_state = MOVING; // Mettre à jour l'état
                }
                break;

            case GO_CAPTURE:
                if(current_state == WAITING_CAP){
                  digitalWrite(PIN_CAPTURE, HIGH); // Activer PIN_CAPTURE
                  current_state = MOVING; // Mettre à jour l'état
                }
                break;

            default:
                break;
        }
    }
    
    // Vérifier les LED et envoyer les informations correspondantes
    else if (digitalRead(LED_START) == HIGH && current_state != WAITING_START) {
        current_state = WAITING_START;
        Serial.write(READY_START); // Signaler que nous sommes prêts pour le démarrage
    }
    else if (digitalRead(LED_CAPTURE) == HIGH && current_state == MOVING) {
        current_state = WAITING_CAP;
        Serial.write(READY_CAPTURE); // Signaler que nous sommes prêts pour la capture
    }

    delay(150); // Petite pause pour éviter une boucle trop rapide
    digitalWrite(PIN_START, LOW); // Activer PIN_START
    digitalWrite(PIN_CAPTURE, LOW); // Activer PIN_START
}
