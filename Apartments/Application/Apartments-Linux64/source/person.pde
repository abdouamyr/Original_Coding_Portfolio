class Person {
  //  color myColor;
  int x, y, happiness, hunger, health;
  float moveSpeed, originalMovementSpeed;
  boolean movingLeft, stop, inConversation, inElevator;
  Person (int xPos, int yPos) {
    //   myColor = bodyColor; 
    x = xPos;
    y = yPos;
    originalMovementSpeed = random(1, 3);
    moveSpeed = originalMovementSpeed;
    happiness = 100;
    if (0 > random(-1, 1)) {
      movingLeft = true;
    } 
    stop = false;
    inConversation = false;
    inElevator = false;
  }

  void drawPerson (int xPos, int yPos) {
    //    x = xPos;
    if (stop) {
      x = xPos;
    }
    y = yPos;
    if (!inElevator) {
      movePerson();
    }
    int happyColor = (int) map (happiness, 0, 100, 0, 255);
    fill(255 - happyColor, happyColor, 0);
    stroke(255);
    rect(x, y, apartmentScale/16, apartmentScale/8);
    ellipse(x, y - apartmentScale/10, apartmentScale/16, apartmentScale/16);
    if (inConversation) {
      talk();
    }
  }

  void movePerson () {
    moveSpeed = originalMovementSpeed * apartmentScale/100;
    if (moveSpeed < originalMovementSpeed) {
      moveSpeed = originalMovementSpeed;
    }
    if (happiness < 1) {
      moveSpeed = 1;
    }
    if (stop) {
      moveSpeed = 0;
    }
    if (movingLeft) {
      x-= moveSpeed;
    } else {
      x+= moveSpeed;
    }
    if (x < screenWidth/2 - apartmentScale/2) {
      movingLeft = false;
    } else if (x > screenWidth/2 + apartmentScale/2) {
      movingLeft = true;
    }
  }

  void talk () {
    fill(255);
    noStroke();
    ellipse(x, y - apartmentScale/4, apartmentScale/8, apartmentScale/16);
    stroke(255);
    line(x, y - apartmentScale/4, x, y - apartmentScale/6);
  }
}

