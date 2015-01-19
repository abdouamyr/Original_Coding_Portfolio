class Elevator {
  int x, floorNumber;
  float y, startY, toMove;
  float percent, speed;
  boolean unloaded = false;
  boolean floorInputInProgress = false;
  Floor currentApartment, lastUnloadFloor;
  ArrayList<Person> riders = new ArrayList<Person>();
  Elevator (int xPos, Floor startFloor) {
    currentApartment = startFloor;
    lastUnloadFloor = currentApartment;
    x = xPos;
    y = currentApartment.posY;
    startY = y;
    toMove = y;
    percent = 0;
    speed = 0.005;
    floorNumber = 2;
  }  
  void moveElevator () { 
    if (lastUnloadFloor != currentApartment) {
      unloaded = false;
    }
    if (keyPressed && keyCode == ALT) {
      unload();
    }
    if (!floorInputInProgress) {
      if (currentApartment != apartments[50 - floorNumber]) {
        currentApartment.elevator = false;
        currentApartment = apartments[50 - floorNumber];
        startY = y;
        toMove = currentApartment.posY - startY;
        percent = 0;
      }
      if (percent <= 1.0) {
        percent+= speed;
        y = startY + (toMove * percent);
      } else {
        y = currentApartment.posY;  
        currentApartment.elevator = true;
        //        unload();
      }
    }
    x = screenWidth/2 - apartmentScale;
  }

  void drawElevator () {

    textSize(0.4 * apartmentScale);
    strokeWeight(0.05 * apartmentScale);
    stroke(0);
    line(x - apartmentScale/2, -apartmentScale/2, x + apartmentScale/2, -apartmentScale/2);
    line(x - apartmentScale/2, apartmentHeight - apartmentScale/2, x + apartmentScale/2, apartmentHeight - apartmentScale/2);
    line (screenWidth/2 - apartmentScale, -apartmentScale/2, screenWidth/2 - apartmentScale, apartmentHeight - apartmentScale/2);
    stroke(255);
    strokeWeight(1);
    fill(0);
    stroke(255);
    rect(x, y, apartmentScale, apartmentScale); 
    if (floorInputInProgress) {
      fill(255, 0, 0);
    } else {
      fill(0, 255, 0);
    }
    rect(x - apartmentScale/4, y - apartmentScale/4, apartmentScale/8, apartmentScale/8);
    fill(255);
    text(floorNumber, x - apartmentScale/8, y - apartmentScale/8);
    for (int i = 0; i < riders.size (); i++) {
      riders.get(i).drawPerson(x - apartmentScale/3 + (apartmentScale * i/8), (int) (y + apartmentScale/3));
    }
    //    text(10 * (50 - floorNumber), x - apartmentScale/8, y + apartmentScale * 3/8);
  }

  void setFloor () {
    int prevNum = floorNumber;
    if (key == '0' || key == '1' || key == '2' || key == '3' || key == '4' || key == '5' || key == '6' || key == '7' || key == '8' || key == '9') {
      if (floorInputInProgress) {
        floorNumber = 10 * prevNum + ((int) key - 48);
        if (50 - floorNumber > 49 || 50 - floorNumber < 0) {
          floorNumber = prevNum;
          floorInputInProgress = false;
        }
      } else {
        floorNumber = (int) key - 48;
        floorInputInProgress = true;
      }
    }
    if (keyCode == SHIFT) {
      if (50 - floorNumber > 49 || 50 - floorNumber < 0) {
        floorNumber = prevNum;
      }
      floorInputInProgress = false;
    }
  }

  void unload () {
    unloaded = true;
    lastUnloadFloor = currentApartment;
    for (int i = 0; i < riders.size (); i++) {
      riders.get(i).stop = false;
      riders.get(i).inElevator = false;
      riders.get(i).happiness += 50;
      currentApartment.tenants.add(riders.get(i));
      currentApartment.population++;
      riders.remove(i);
    }
  }
}

